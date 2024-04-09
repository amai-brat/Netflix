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
        try{
            return await this.subscriptionService.getAllSubscriptions();
        }
        catch{
            throw new BadRequestException("Invalid operation!");
        }
    }

    @Get('/getSubscriptionById')
    async getSubscriptionById(@Query('subscriptionId', ParseIntPipe) subscriptionId: number): Promise<Subscription> {
        try{
            return await this.subscriptionService.getSubscriptionById(subscriptionId);
        }
        catch{
            throw new BadRequestException("Invalid operation!");
        }
    }

    @UseGuards(JwtAuthGuard)
    @Get('/getUserSubscriptions')
    async getUserSubscriptions(@Request() req): Promise<UserSubscription[]>{
        const userNickname = req.user.nickname;

        try{  
            return await this.subscriptionService.getBoughtSubscriptionsByNickname(userNickname);
        }
        catch{
            throw new BadRequestException("Invalid operation!")
        }
    }

    @UseGuards(JwtAuthGuard)
    @Post('/buySubscription')
    async buySubscription(@Request() req): Promise<UserSubscription> {
        const userNickname = req.user.nickname;
        
        const subscriptionId = req.body.subscriptionId;
        if (!subscriptionId || typeof subscriptionId !== 'number'){
            throw new BadRequestException("Invalid request body!");
        }

        try{  
            return await this.subscriptionService.processSubscriptionPurchase(userNickname, subscriptionId);
        }
        catch{
            throw new BadRequestException("Invalid operation!")
        }
    }

    @UseGuards(JwtAuthGuard)
    @Delete('/cancelSubscription')
    async cancelSubscription(@Request() req): Promise<void>{
        const userNickname = req.user.nickname;
        
        const subscriptionId = req.body.subscriptionId;
        if (!subscriptionId || typeof subscriptionId !== 'number'){
            throw new BadRequestException("Invalid request body!");
        }

        try{
            await this.subscriptionService.cancelSubscription(userNickname, subscriptionId);
        }
        catch{
            throw new BadRequestException("Invalid operation!")
        }
    }
}
