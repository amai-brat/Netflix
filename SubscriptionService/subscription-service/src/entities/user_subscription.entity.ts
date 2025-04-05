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
    @Column({ name: 'transaction_id', type: "uuid", nullable: true })
    transactionId: string | null

    @ApiProperty()
    @Column({ name: 'status' })
    status: UserSubscriptionStatus;

    get isExpired() {
        return this.expiresAt < new Date();
    }

    constructor(
        id: number, 
        userId: number, 
        subscriptionId: number, 
        expiresAt: Date, 
        boughtAt: Date, 
        transactionId: string | null, 
        status: UserSubscriptionStatus) {
            this.id = id;
            this.userId = userId;
            this.subscriptionId = subscriptionId;
            this.expiresAt = expiresAt;
            this.boughtAt = boughtAt;
            this.transactionId = transactionId;
            this.status = status;
    }

    static create() {
        return new UserSubscription(0, 0, 0, new Date(), new Date(), "", 0);
    }
}

export enum UserSubscriptionStatus {
    "PENDING" = 0,
    "COMPLETED" = 1,
    "FAILED" = 2,
    "CANCELLED" = 3
}