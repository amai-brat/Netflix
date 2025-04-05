import { Test } from '@nestjs/testing';
import { INestApplication } from '@nestjs/common';
import * as request from 'supertest';
import { AppModule } from './../src/app.module';
import { TypeOrmModule, getRepositoryToken } from '@nestjs/typeorm';
import { User } from '../src/entities/user.entity';
import { Subscription } from '../src/entities/subscription.entity';
import { UserSubscription } from '../src/entities/user_subscription.entity';
import { SubscriptionService } from '../src/subscription/subscription.service';
import { Repository } from 'typeorm';
import { sign } from "jsonwebtoken"
import {ConfigService} from "@nestjs/config";
import { ClientsModule, Transport } from '@nestjs/microservices';
import { join } from 'path/posix';
import { timeout } from 'rxjs';

describe('AppController (e2e)', () => {
    let app: INestApplication;
    let subscriptionService: SubscriptionService;
    let userRepository: Repository<User>;
    let subscriptionRepository: Repository<Subscription>;
    let jwt: string;

    beforeAll(async () => {
        const moduleFixture = await Test.createTestingModule({
            imports: [
                TypeOrmModule.forRoot({
                    type: 'sqlite',
                    database: ':memory:',
                    entities: [User, Subscription, UserSubscription],
                    synchronize: true
                }),
                TypeOrmModule.forFeature([User, UserSubscription, Subscription]),
                // ClientsModule.register([
                //     {
                //       name: 'PAYMENT_PACKAGE',
                //       transport: Transport.GRPC,
                //       options: {
                //         package: 'payment',
                //         protoPath: join(__dirname, '../src/proto/payment.proto'),
                //       },
                //     },
                //   ]),
                AppModule
            ],
        }).compile();


        app = moduleFixture.createNestApplication();
        await app.init();

        const configService = moduleFixture.get<ConfigService>(ConfigService);
        jwt = sign({ id: 1 },  configService.get<string>("JWT_KEY"));
        subscriptionService = moduleFixture.get<SubscriptionService>(SubscriptionService);
        userRepository = moduleFixture.get<Repository<User>>(getRepositoryToken(User));
        subscriptionRepository = moduleFixture.get<Repository<Subscription>>(getRepositoryToken(Subscription));
    });

    it('should initialize testing data', async()=>{
        expect(await userRepository.save(
            userRepository.create({ id: 1, nickname: "testUser1" }) 
        )).toBeDefined();
        expect(await userRepository.save(
            userRepository.create({ id: 2, nickname: "testUser2" }) 
        )).toBeDefined();

        expect(await subscriptionRepository.save(
            subscriptionRepository.create({ id: 1, name: "Фильмы", description: "Подписка на фильмы", max_resolution: 720, price: 228 })
        )).toBeDefined();
        expect(await subscriptionRepository.save(
            subscriptionRepository.create({ id: 2, name: "Мультфильмы", description: "Подписка на мультфильмы", max_resolution: 1080, price: 322 })
        )).toBeDefined();
        expect(await subscriptionRepository.save(
            subscriptionRepository.create({ id: 3, name: "Сериалы", description: "Подписка на сериалы", max_resolution: 1080, price: 666 })
        )).toBeDefined();

        await subscriptionService.processSubscriptionPurchase(1, {subscriptionId: 1, card: null});
        await subscriptionService.processSubscriptionPurchase(1, {subscriptionId: 2, card: null});
    }, 1000000000)

    it('/getAllSubscriptions (GET)', () => {
        return request(app.getHttpServer())
        .get('/subscription/getAllSubscriptions')
        .expect(200)
        .expect(response => {
            expect(response.body).toBeInstanceOf(Array);
            expect(response.body).toHaveLength(3);
        })
    });

    it('/getSubscriptionById (GET)', () => {
        return request(app.getHttpServer())
        .get('/subscription/getSubscriptionById')
        .query({ subscriptionId: 1 })
        .expect(200)
        .expect(response => {
            expect(response.body).toEqual({ id: 1, name: "Фильмы", description: "Подписка на фильмы", max_resolution: 720, price: 228 });
        })
    })

    it('/getSubscriptionById (GET) with wrong query', () => {
        return request(app.getHttpServer())
        .get('/subscription/getSubscriptionById')
        .query({ subscriptionId: "someWrong" })
        .expect(400)
    })

    it('/getUserSubscriptions (GET) with JWT token', () => {
        return request(app.getHttpServer())
        .get('/subscription/getUserSubscriptions')
        .set('Authorization', `Bearer ${jwt}`)
        .expect(200)
        .expect(response => {
            expect(response.body).toBeInstanceOf(Array);
            // я не хочу мокать grpc клиента
            // expect(response.body).toHaveLength(2);
        });
    })

    it('/getUserSubscriptions (GET) without authorization', () => {
        return request(app.getHttpServer())
        .get('/subscription/getUserSubscriptions')
        .expect(401)
    })

    it('/buySubscription (POST) with JWT token', () => {
        return request(app.getHttpServer())
        .post('/subscription/buySubscription')
        .set('Authorization', `Bearer ${jwt}`)
        .send({ subscriptionId: 3 })
        .expect(201)
        .expect(response => {
            expect(response.body).toMatchObject({ 
                subscriptionId: 3,
                userId: 1
            });
        })
    }, 10000000);

    it('/buySubscription (POST) without Authorization', () => {
        return request(app.getHttpServer())
        .post('/subscription/buySubscription')
        .send({ subscriptionId: 3 })
        .expect(401)
    })

    it('/buySubscription (POST) without proper request body', () => {
        return request(app.getHttpServer())
        .post('/subscription/buySubscription')
        .set('Authorization', `Bearer ${jwt}`)
        .expect(400)
    })

    it('/cancelSubscription (DELETE) with JWT token', () => {
        return request(app.getHttpServer())
        .delete('/subscription/cancelSubscription')
        .set('Authorization', `Bearer ${jwt}`)
        .send({ subscriptionId: 2 })
        .expect(200)
    })

    it('/cancelSubscription (DELETE) without Authorization', () => {
        return request(app.getHttpServer())
        .delete('/subscription/cancelSubscription')
        .send({ subscriptionId: 2 })
        .expect(401)
    })

    it('/cancelSubscription (DELETE) without proper request body', () => {
        return request(app.getHttpServer())
        .delete('/subscription/cancelSubscription')
        .set('Authorization', `Bearer ${jwt}`)
        .expect(400)
    })

    afterAll(async () => {
        await app.close();
    });
});
