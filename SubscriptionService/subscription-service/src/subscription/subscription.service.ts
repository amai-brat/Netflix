import {Injectable} from "@nestjs/common";
import {InjectRepository} from "@nestjs/typeorm";
import {Repository} from "typeorm";
import {User} from "../entities/user.entity";
import {Subscription} from "../entities/subscription.entity";
import {UserSubscription} from "../entities/user_subscription.entity";

@Injectable()
export class SubscriptionService {
    constructor(
        @InjectRepository(User)
        private readonly userRepository: Repository<User>,

        @InjectRepository(Subscription)
        private readonly subscriptionRepository: Repository<Subscription>,

        @InjectRepository(UserSubscription)
        private readonly userSubscriptionRepository: Repository<UserSubscription>
    ) {}

    async getAllSubscriptions(){
        return await this.subscriptionRepository.find();
    }

    async getSubscriptionById(id: number){
        return await this.subscriptionRepository.findOneByOrFail({ id: id });
    }

    async getBoughtSubscriptionsByUserId(userId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId })

        const subscriptions = await this.userSubscriptionRepository.findBy({ userId: user.id });

        return Array.isArray(subscriptions) ? subscriptions: [subscriptions];
    }

    async getCurrentSubscriptionsByUserId(userId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId })

        const subscriptions = await this.userSubscriptionRepository.findBy({ userId: user.id });
        return subscriptions.filter(sub => sub.expiresAt > new Date());
    }

    async processSubscriptionPurchase(userId: number, subscriptionId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId });
        
        const userSubscription = this.userSubscriptionRepository.create();
        userSubscription.boughtAt = new Date();
        userSubscription.expiresAt = new Date(userSubscription.boughtAt);
        userSubscription.expiresAt.setDate(userSubscription.boughtAt.getDate() + 30);
        userSubscription.subscriptionId = subscriptionId;
        userSubscription.userId = user.id;

        return await this.userSubscriptionRepository.save(userSubscription);
    }

    async cancelSubscription(userId: number, subscriptionId: number){
        const user = await this.userRepository.findOneByOrFail({ id: userId })
        const userSubscription = await this.userSubscriptionRepository.findOneByOrFail({ 
            userId: user.id, 
            subscriptionId: subscriptionId 
        });
        
        await this.userSubscriptionRepository.remove(userSubscription);
    }
}