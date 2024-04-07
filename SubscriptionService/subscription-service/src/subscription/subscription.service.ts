import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { User } from "../entities/user.entity";
import { Subscription } from "../entities/subscription.entity";
import { UserSubscription } from "../entities/user_subscription.entity";

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
        return this.subscriptionRepository.find();
    }

    async getSubscriptionById(id: number){
        return this.subscriptionRepository.findOneByOrFail({ id: id });
    }

    async getBoughtSubscriptionsByNickname(userNickname: string){
        const user = await this.userRepository.findOneByOrFail({ nickname: userNickname })

        return this.userSubscriptionRepository.find({ where: { userId: user.id } });
    }

    async processSubscriptionPurchase(userNickname: string, subscriptionId: number){
        const user = await this.userRepository.findOneByOrFail({ nickname: userNickname });
        
        const userSubscription = new UserSubscription();
        userSubscription.boughtAt = new Date();
        userSubscription.expiresAt = new Date(userSubscription.boughtAt);
        userSubscription.expiresAt.setDate(userSubscription.boughtAt.getDate() + 30);
        userSubscription.subscriptionId = subscriptionId;
        userSubscription.userId = user.id;

        return this.userSubscriptionRepository.save(userSubscription);
    }

    async cancelSubscription(userNickname: string, subscriptionId: number){
        const user = await this.userRepository.findOneByOrFail({ nickname: userNickname })
        const userSubscription = await this.userSubscriptionRepository.findOneByOrFail({ 
            userId: user.id, 
            subscriptionId: subscriptionId 
        });
        
        this.userSubscriptionRepository.remove(userSubscription);
    }
}