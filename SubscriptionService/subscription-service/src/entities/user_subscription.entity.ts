import { ApiProperty } from "@nestjs/swagger";
import { Entity, PrimaryColumn, Column, PrimaryGeneratedColumn } from "typeorm"

@Entity({ name: 'user_subscriptions' })
export class UserSubscription { 
    @ApiProperty()
    @PrimaryGeneratedColumn({ name: 'id' })
    id: number;

    @ApiProperty()
    @Column({ name: 'user_id' })
    userId: number;

    @ApiProperty()
    @Column({ name: 'subscription_id' })
    subscriptionId: number;

    @ApiProperty()
    @Column({ name: 'expires_at' })
    expiresAt: Date;

    @ApiProperty()
    @Column({ name: 'bought_at' })
    boughtAt: Date;

    @ApiProperty()
    @Column({ name: 'transaction_id', type: "uuid" })
    transactionId: string | null

    @ApiProperty()
    @Column({ name: 'status' })
    status: UserSubscriptionStatus;
}

export enum UserSubscriptionStatus {
    "PENDING" = 0,
    "COMPLETED" = 1,
    "FAILED" = 2
}