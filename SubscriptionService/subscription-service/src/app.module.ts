import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { SubscriptionController } from './subscription/subscription.controller';
import { SubscriptionService } from './subscription/subscription.service';
import { JwtStrategy } from '../auth/jwt.strategy';
import { User } from './entities/user.entity';
import { Subscription } from './entities/subscription.entity';
import { UserSubscription } from './entities/user_subscription.entity';
import {ConfigModule, ConfigService} from "@nestjs/config";

@Module({
    imports: [
        TypeOrmModule.forRootAsync({
            imports: [ConfigModule],
            inject: [ConfigService],
            useFactory: (configService: ConfigService) => (
                {
                type: 'postgres',
                url: configService.get<string>('DATABASE_CONNECTION_STRING_GENERAL'),
                entities: [User, Subscription, UserSubscription],
                synchronize: false,
            }),
        }),
        TypeOrmModule.forFeature([User, UserSubscription, Subscription]),
        ConfigModule.forRoot({
            isGlobal: true,
        }),
    ],
    controllers: [SubscriptionController],
    providers: [SubscriptionService, JwtStrategy],
})
export class AppModule {}