import { Entity, PrimaryGeneratedColumn, Column } from "typeorm"

@Entity({ name: 'users' })
export class User{
    @PrimaryGeneratedColumn()
    id: number;

    @Column()
    nickname: string;
}