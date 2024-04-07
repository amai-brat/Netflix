import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { SubscriptionController } from './subscription/subscription.controller';
import { SubscriptionService } from './subscription/subscription.service';
import { JwtStrategy } from 'auth/jwt.strategy';
import { User } from './entities/user.entity';
import { Subscription } from './entities/subscription.entity';
import { UserSubscription } from './entities/user_subscription.entity';

@Module({
    imports: [
        TypeOrmModule.forRoot({
            type: 'postgres',
            host: 'localhost',
            port: 5432,
            username: "postgres",
            password: "admin",
            database: "Netflix",
            entities: [User, Subscription, UserSubscription]
        }),
        TypeOrmModule.forFeature([User, UserSubscription, Subscription])
    ],
    controllers: [SubscriptionController],
    providers: [SubscriptionService, JwtStrategy],
})
export class AppModule {}
