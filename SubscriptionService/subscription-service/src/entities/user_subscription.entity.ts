import { Entity, PrimaryColumn, Column } from "typeorm"

@Entity({ name: 'user_subscriptions' })
export class UserSubscription {
    @PrimaryColumn({ name: 'user_id' })
     userId: number;

    @PrimaryColumn({ name: 'subscription_id' })
    subscriptionId: number;

    @Column({ name: 'expires_at' })
    expiresAt: Date;

    @Column({ name: 'bought_at' })
    boughtAt: Date;
}