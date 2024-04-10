import { ApiProperty } from "@nestjs/swagger";
import { Entity, PrimaryColumn, Column } from "typeorm"

@Entity({ name: 'user_subscriptions' })
export class UserSubscription {
    @ApiProperty()
    @PrimaryColumn({ name: 'user_id' })
     userId: number;

    @ApiProperty()
    @PrimaryColumn({ name: 'subscription_id' })
    subscriptionId: number;

    @ApiProperty()
    @Column({ name: 'expires_at' })
    expiresAt: Date;

    @ApiProperty()
    @Column({ name: 'bought_at' })
    boughtAt: Date;
}