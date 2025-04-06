import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { SubscriptionController } from './subscription/subscription.controller';
import { SubscriptionService } from './subscription/subscription.service';
import { JwtStrategy } from '../auth/jwt.strategy';
import { User } from './entities/user.entity';
import { Subscription } from './entities/subscription.entity';
import { UserSubscription } from './entities/user_subscription.entity';
import {ConfigModule, ConfigService} from "@nestjs/config";
import { ClientsModule, Transport } from '@nestjs/microservices';
import { join } from 'path';

@Module({
    imports: [
        TypeOrmModule.forRootAsync({
            imports: [ConfigModule],
            inject: [ConfigService],
            useFactory: (configService: ConfigService) => ({
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
        ClientsModule.registerAsync([
            {
                name: 'PAYMENT_PACKAGE',
                inject: [ConfigService], 
                imports: [ConfigModule],
                useFactory: (configService: ConfigService) => ({
                    transport: Transport.GRPC,
                    options: {
                        url: configService.get<string>('PAYMENT_SERVICE_URL'),
                        package: 'payment',
                        protoPath: join(__dirname, 'proto/payment.proto')
                    },
                })
            },
        ])
    ],
    controllers: [SubscriptionController],
    providers: [SubscriptionService, JwtStrategy],
})
export class AppModule {}