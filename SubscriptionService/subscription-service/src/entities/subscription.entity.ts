import { Entity, PrimaryGeneratedColumn, Column } from "typeorm"

@Entity({ name: "subscriptions" })
export class Subscription{
    @PrimaryGeneratedColumn()
    id: number;

    @Column()
    name: string;

    @Column()
    description: string;

    @Column()
    max_resolution: number;
}