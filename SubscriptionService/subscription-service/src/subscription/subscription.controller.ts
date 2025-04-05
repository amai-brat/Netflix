import { BadRequestException, Body, Controller, Delete, Get, InternalServerErrorException, NotFoundException, Param, ParseIntPipe, Post, Query, Req, Request, UseGuards } from '@nestjs/common';
import { SubscriptionService } from './subscription.service';
import { Subscription } from '../entities/subscription.entity';
import { UserSubscription } from '../entities/user_subscription.entity';
import { JwtAuthGuard } from '../../auth/jwt-auth.guard';
import { ApiTags, ApiOkResponse, ApiInternalServerErrorResponse, ApiUnauthorizedResponse, ApiBadRequestResponse, ApiNotFoundResponse, ApiBearerAuth, ApiBody, ApiCreatedResponse, ApiProperty } from '@nestjs/swagger';
import { BuySubscriptionDto } from './subscription.dto';

@ApiTags('subscription')
@Controller('/subscription')
export class SubscriptionController {
    constructor(private readonly subscriptionService: SubscriptionService) {}


    @Get('/getAllSubscriptions')
    @ApiOkResponse({type: [Subscription], description: 'Subscriptions have been successfully received'})
    @ApiInternalServerErrorResponse({description: 'Internal server error'})
    async getAllSubscriptions(): Promise<Subscription[]> {
        return await this.subscriptionService.getAllSubscriptions();
    }


    @Get('/getSubscriptionById')
    @ApiOkResponse({type: Subscription, description: 'Subscription has been successfully received'})
    @ApiBadRequestResponse({description: "Provided request query was wrong"})
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
    @ApiBearerAuth()
    @ApiOkResponse({type: [Subscription], description: "User's subscriptions have been successfully received"})
    @ApiUnauthorizedResponse({description: "User is not authorized - JWT token was not correct/was not provided."})
    async getUserSubscriptions(@Request() req): Promise<UserSubscription[]>{
        const userId = req.user.id;

        try{  
            return await this.subscriptionService.getBoughtSubscriptionsByUserId(userId);
        }
        catch{
            throw new BadRequestException("Invalid operation!")
        }
    }

    @UseGuards(JwtAuthGuard)
    @Get('/getCurrentUserSubscriptions')
    @ApiBearerAuth()
    @ApiOkResponse({type: [Subscription], description: "User's subscriptions have been successfully received"})
    @ApiUnauthorizedResponse({description: "User is not authorized - JWT token was not correct/was not provided."})
    async getCurrentUserSubscriptions(@Request() req): Promise<UserSubscription[]>{
        const userId = req.user.id;

        try{
            return await this.subscriptionService.getCurrentSubscriptionsByUserId(userId);
        }
        catch{
            throw new BadRequestException("Invalid operation!")
        }
    }


    @UseGuards(JwtAuthGuard)
    @Post('/buySubscription')
    @ApiBearerAuth()
    @ApiBody({ type: BuySubscriptionDto })
    @ApiCreatedResponse({type: UserSubscription, description: "Subscription was successfully bought"})
    @ApiBadRequestResponse({description: "Provided request body was wrong"})
    @ApiUnauthorizedResponse({description: "User is not authorized - JWT token was not correct/was not provided."})
    async buySubscription(@Request() req, @Body() body: BuySubscriptionDto): Promise<UserSubscription> {
        const userId = req.user.id;
        
        const subscriptionId = req.body.subscriptionId;
        if (!subscriptionId || typeof subscriptionId !== 'number'){
            throw new BadRequestException("Invalid request body!");
        }

        try {  
            return await this.subscriptionService.processSubscriptionPurchase(userId, body);
        }
        catch (err) {
            throw new InternalServerErrorException("Smth went wrong")
        }
    }

    @UseGuards(JwtAuthGuard)
    @ApiBearerAuth()
    @ApiOkResponse({ schema: {type: 'object', properties: { status: {type: 'number' } }} })
    @Get(':id/status')
    async getSubscriptionStatus(@Req() req, @Param('id') id: number) {
        const userSubscription = await this.subscriptionService.getStatus(req.user.id, id);
        return {
            status: userSubscription.status,
        };
    }

    
    @UseGuards(JwtAuthGuard)
    @Delete('/cancelSubscription')
    @ApiBearerAuth()
    @ApiBody({ schema: { type: 'object', properties: { subscriptionId: { type: 'number' }}}})
    @ApiOkResponse({description: "User's subscription was successfully canceled"})
    @ApiBadRequestResponse({description: "Provided request body was wrong"})
    @ApiNotFoundResponse({description: "User does not have subscription with id, that was provided in request body"})
    @ApiUnauthorizedResponse({description: "User is not authorized - JWT token was not correct/was not provided."})
    async cancelSubscription(@Request() req): Promise<void>{
        const userId = req.user.id;
        
        const subscriptionId = req.body.subscriptionId;
        if (!subscriptionId || typeof subscriptionId !== 'number'){
            throw new BadRequestException("Invalid request body!");
        }

        try{
            await this.subscriptionService.cancelSubscription(userId, subscriptionId);
        }
        catch{
            throw new NotFoundException("User does not have such subscription");
        }
    }
}
