import { BadRequestException, Controller, Delete, Get, ParseIntPipe, Post, Query, Request, UseGuards } from '@nestjs/common';
import { SubscriptionService } from './subscription.service';
import { Subscription } from '../entities/subscription.entity';
import { UserSubscription } from '../entities/user_subscription.entity';
import { JwtAuthGuard } from '../../auth/jwt-auth.guard';

@Controller('/subscription')
export class SubscriptionController {
    constructor(private readonly subscriptionService: SubscriptionService) {}

    @Get('/getAllSubscriptions')
    async getAllSubscriptions(): Promise<Subscription[]> {
        return await this.subscriptionService.getAllSubscriptions();
    }

    @Get('/getSubscriptionById')
    async getSubscriptionById(@Query('subscriptionId', ParseIntPipe) subscriptionId: number): Promise<Subscription> {
        return await this.subscriptionService.getSubscriptionById(subscriptionId);
    }

    @UseGuards(JwtAuthGuard)
    @Get('/getUserSubscriptions')
    async getUserSubscriptions(@Request() req): Promise<UserSubscription[]>{
        const userNickname = req.user.nickname;

        return await this.subscriptionService.getBoughtSubscriptionsByNickname(userNickname);
    }

    @UseGuards(JwtAuthGuard)
    @Post('/buySubscription')
    async buySubscription(@Request() req): Promise<UserSubscription> {
        const userNickname = req.user.nickname;
        
        const subscriptionId = req.body.subscriptionId;
        if (!subscriptionId || typeof subscriptionId !== 'number'){
            throw new BadRequestException("Invalid request body!");
        }

        return await this.subscriptionService.processSubscriptionPurchase(userNickname, subscriptionId);
    }

    @UseGuards(JwtAuthGuard)
    @Delete('/cancelSubscription')
    async cancelSubscription(@Request() req): Promise<void>{
        const userNickname = req.user.nickname;
        
        const subscriptionId = req.body.subscriptionId;
        if (!subscriptionId || typeof subscriptionId !== 'number'){
            throw new BadRequestException("Invalid request body!");
        }

        await this.subscriptionService.cancelSubscription(userNickname, subscriptionId);
    }
}
