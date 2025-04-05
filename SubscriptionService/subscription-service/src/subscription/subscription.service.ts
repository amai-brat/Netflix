import {BadRequestException, ForbiddenException, Inject, Injectable, Logger, OnModuleInit} from "@nestjs/common";
import {InjectRepository} from "@nestjs/typeorm";
import {MoreThan, Repository} from "typeorm";
import {User} from "../entities/user.entity";
import {Subscription} from "../entities/subscription.entity";
import {UserSubscription, UserSubscriptionStatus} from "../entities/user_subscription.entity";
import { BuySubscriptionDto } from "./subscription.dto";
import { ClientGrpc } from "@nestjs/microservices";
import { PaymentRequest, PaymentResponse, PaymentResponse_Status, PaymentService } from "../proto/payment";

@Injectable()
export class SubscriptionService implements OnModuleInit {
    private paymentServiceClient: PaymentService;
    private readonly logger = new Logger(SubscriptionService.name);

    constructor(
        @InjectRepository(User)
        private readonly userRepository: Repository<User>,

        @InjectRepository(Subscription)
        private readonly subscriptionRepository: Repository<Subscription>,

        @InjectRepository(UserSubscription)
        private readonly userSubscriptionRepository: Repository<UserSubscription>,

        @Inject('PAYMENT_PACKAGE')
        private grpcClient: ClientGrpc
    ) {}

    onModuleInit() {
        this.paymentServiceClient = this.grpcClient.getService<PaymentService>('PaymentService');
    }

    async getAllSubscriptions(){
        return await this.subscriptionRepository.find();
    }

    async getSubscriptionById(id: number){
        return await this.subscriptionRepository.findOneByOrFail({ id: id });
    }

    async getBoughtSubscriptionsByUserId(userId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId })

        const subscriptions = await this.userSubscriptionRepository.findBy({ userId: user.id, status: UserSubscriptionStatus.COMPLETED });

        return Array.isArray(subscriptions) ? subscriptions: [subscriptions];
    }

    async getCurrentSubscriptionsByUserId(userId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId })

        const subscriptions = await this.userSubscriptionRepository.findBy({ userId: user.id, status: UserSubscriptionStatus.COMPLETED });
        return subscriptions.filter(sub => !sub.isExpired);
    }

    async processSubscriptionPurchase(userId: number, dto: BuySubscriptionDto){
        const user = await this.userRepository.findOneByOrFail({ id: userId });
        const subscription = await this.subscriptionRepository.findOneByOrFail({id: dto.subscriptionId});

        const currentSubsriptions = await this.userSubscriptionRepository.findBy({
            subscriptionId: subscription.id, 
            userId: user.id, 
            expiresAt: MoreThan(new Date()),
            status: UserSubscriptionStatus.COMPLETED
        });

        if (currentSubsriptions && currentSubsriptions.length > 0) {
          throw new BadRequestException("There is active subscription");
        }

        const userSubscription = this.userSubscriptionRepository.create({
            boughtAt: new Date(),
            expiresAt: new Date(new Date().setDate(new Date().getDate() + 30)),
            subscriptionId: dto.subscriptionId,
            userId: user.id,
            status: UserSubscriptionStatus.PENDING,
            transactionId: null
        });

        const savedUserSub = await this.userSubscriptionRepository.save(userSubscription);
        this.handlePaymentProcessing(user.id, savedUserSub.id, dto, subscription.price).then(
            () => this.logger.log("Payment service was called"));
        
        return savedUserSub;
    }

    async getUserSubscription(userId: number, userSubscriptionId: number): Promise<UserSubscription> {
      const user = await this.userRepository.findOneByOrFail({ id: userId });
      const userSubscription = await this.userSubscriptionRepository.findOneByOrFail({ id: userSubscriptionId });
      if (userSubscription.userId !== user.id) {
        throw new ForbiddenException("You can't check others' subscription statuses");
      }

      return userSubscription;
    }

    async cancelSubscription(userId: number, subscriptionId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId })
        const userSubscriptions = await this.userSubscriptionRepository.findBy({ 
            userId: user.id, 
            subscriptionId: subscriptionId 
        });
        console.log(userSubscriptions)
        const toCancels = userSubscriptions
          .filter(x => 
            !x.isExpired && 
            x.status == UserSubscriptionStatus.COMPLETED
          ).sort(x => +x.expiresAt);

        if (toCancels.length > 1) {
          this.logger.error(`User ${userId} should have only one non-expired subscription of type ${subscriptionId} at the same time, but has ${toCancels.length}`);
        }

        // cancel only last subscription
        const toCancel = toCancels.at(-1);
        toCancel.status = UserSubscriptionStatus.CANCELLED;

        await this.userSubscriptionRepository.save(toCancel);
    }

    private async handlePaymentProcessing(
        userId: number,
        userSubscriptionId: number,
        dto: BuySubscriptionDto,
        amount: number,
      ) {
        try {
          const req: PaymentRequest = {
            card: dto.card,
            userId: userId,
            amount: amount,
          };
      
          const paymentStream = this.paymentServiceClient.ProcessPayment(req);
          
          const sub = paymentStream.subscribe({
            next: async (response: PaymentResponse) => {
              let canClose = false;

              const userSubscription = await this.userSubscriptionRepository.findOneByOrFail(
                { id: userSubscriptionId },
              );
      
              if (!userSubscription) return;
      
              if (response.transactionId) {
                userSubscription.transactionId = response.transactionId;
              }
      
              switch (response.status) {
                case PaymentResponse_Status.SUCCESS:
                  canClose = true;
                  userSubscription.status = UserSubscriptionStatus.COMPLETED;
                  break;
                case PaymentResponse_Status.FAILED:
                  canClose = true;
                  userSubscription.status = UserSubscriptionStatus.FAILED;
                  break;
                case PaymentResponse_Status.PENDING:
                  userSubscription.status = UserSubscriptionStatus.PENDING;
                  break;
              }
      
              await this.userSubscriptionRepository.save(userSubscription);
      
              if (response.status === PaymentResponse_Status.FAILED) {
                await this.paymentServiceClient.CompensatePayment({
                  transactionId: response.transactionId,
                });
              }

              if (canClose) sub.unsubscribe();
            },
            error: async (error) => {
              const userSubscription = await this.userSubscriptionRepository.findOneByOrFail(
                { id: userSubscriptionId },
              );
              if (userSubscription) {
                userSubscription.status = UserSubscriptionStatus.FAILED;
                await this.userSubscriptionRepository.save(userSubscription);
              }

              sub.unsubscribe();
            },
          });
        } catch (error) {
          const userSubscription = await this.userSubscriptionRepository.findOneByOrFail({
            id: userSubscriptionId,
          });
          if (userSubscription) {
            userSubscription.status = UserSubscriptionStatus.FAILED;
            await this.userSubscriptionRepository.save(userSubscription);
          }
        }
      }
}