import { Test, TestingModule } from '@nestjs/testing';
import { SubscriptionService } from './subscription.service';
import { getRepositoryToken } from '@nestjs/typeorm';
import { User } from '../entities/user.entity';
import { Subscription } from '../entities/subscription.entity';
import { UserSubscription, UserSubscriptionStatus } from '../entities/user_subscription.entity';
import { EntityNotFoundError } from 'typeorm';
import { ClientsModule, Transport } from '@nestjs/microservices';
import { join } from 'path';

describe('SubscriptionService', () => {
    let service: SubscriptionService;

    const fakeUsers: User[] = [
        { id: 1, nickname: "mockUser" },
        { id: 2, nickname: "mockUser2" }
    ]
    const fakeSubscriptions: Subscription[] = [
        { id: 1, name: "Фильмы", description: "Фильмы подписка", max_resolution: 720, price: 228 },
        { id: 2, name: "Мультфильмы", description: "Мультфильмы подписка", max_resolution: 1080, price: 322 }
    ]
    const fakeUserSubscriptions: UserSubscription[] = [
        new UserSubscription(1, 2, 1, new Date(), new Date(), "", 1)
    ]
    
    const mockUserRepository = {
        findOneByOrFail: jest.fn().mockImplementation(async ( findOptions: { id: number }) => {
            let user = fakeUsers.find(us => us.id == findOptions.id);

            if (!user){
                throw new EntityNotFoundError(User, findOptions.id);
            }

            return Promise.resolve(user);
        })
    }
    const mockSubscriptionRepository = {
        find: jest.fn().mockImplementation(async () => Promise.resolve(fakeSubscriptions)),
        findOneByOrFail: jest.fn().mockImplementation(async ( findOptions: { id: number }) => {
            let subscription = fakeSubscriptions.find(s => s.id == findOptions.id);
            if (!subscription){
                throw new EntityNotFoundError(Subscription, findOptions.id);
            }

            return Promise.resolve(subscription);
        }),
    }
    const mockUserSubscriptionRepository = {
        create: jest.fn().mockImplementation((us) => {us.id = 228; us.status = 1; return us;}),
        save: jest.fn().mockImplementation(async (userSubscription: UserSubscription) => {
            let savedElementIndex: number;
            
            let existingElementIndex = fakeUserSubscriptions.findIndex(us => us.userId == userSubscription.userId
                && us.subscriptionId == userSubscription.subscriptionId);
            
            if (existingElementIndex != -1){
                fakeUserSubscriptions[existingElementIndex] = userSubscription;
                savedElementIndex = existingElementIndex;
            }
            else {
                savedElementIndex = fakeUserSubscriptions.push(userSubscription) - 1;
            }

            return Promise.resolve(fakeUserSubscriptions[savedElementIndex]);
        }),
        remove: jest.fn().mockImplementation(async (userSubscription: UserSubscription) => {
            let removingElementIndex = fakeUserSubscriptions.indexOf(userSubscription);
            fakeUserSubscriptions.splice(removingElementIndex, 1);
        }),
        findBy: jest.fn().mockImplementation(async ( findOptions: { userId: number, subscriptionId: number }) => {
            let userSubscription;
            if (findOptions.subscriptionId !== undefined) {
                userSubscription = fakeUserSubscriptions.filter(s => s.userId == findOptions.userId && s.subscriptionId == findOptions.subscriptionId);
            }
            else {
                userSubscription = fakeUserSubscriptions.filter(s => s.userId == findOptions.userId)
            };
            return Promise.resolve(userSubscription);
        }),
        findOneByOrFail: jest.fn().mockImplementation(async ( findOptions: { id: number }) => {
            let userSubscription = fakeUserSubscriptions.find(us =>  us.id === findOptions.id);
            if (!userSubscription){
                throw new EntityNotFoundError(Subscription, findOptions);
            }

            return Promise.resolve(userSubscription);
        }),
    }

    beforeEach(async() => {
        const module: TestingModule = await Test.createTestingModule({
            imports: [
                ClientsModule.register([
                    {
                      name: 'PAYMENT_PACKAGE',
                      transport: Transport.GRPC,
                      options: {
                        package: 'payment',
                        protoPath: join(__dirname, '../proto/payment.proto'),
                      },
                    },
                  ]),
            ],
            providers: [
                SubscriptionService,
                {
                    provide: getRepositoryToken(User),
                    useValue: mockUserRepository
                },
                {
                    provide: getRepositoryToken(Subscription),
                    useValue: mockSubscriptionRepository
                },
                {
                    provide: getRepositoryToken(UserSubscription),
                    useValue: mockUserSubscriptionRepository
                }
            ],
        }).compile();

        service = module.get<SubscriptionService>(SubscriptionService);
    });
    

    it('should be defined', () => {
        expect(service).toBeDefined();
    });

    it('should return all subscriptions', async () => {
        const result = await service.getAllSubscriptions();

        expect(result).toEqual(fakeSubscriptions);
    })

    it('should return subscription with selected id', async() => {
        const randomSubscriptionId = fakeSubscriptions[Math.floor(Math.random() * fakeSubscriptions.length)].id;
        
        const expectedSubscription = fakeSubscriptions.find(s => s.id = randomSubscriptionId);

        const result = await service.getSubscriptionById(randomSubscriptionId);

        expect(result).toEqual(expectedSubscription);
    })

    it('should save purchased subscription and return it', async() => {
        const randomSubscriptionId = fakeSubscriptions[Math.floor(Math.random() * fakeSubscriptions.length)].id;
        const user = fakeUsers[1];
        
        const expectedUserSubscription = UserSubscription.create();
        expectedUserSubscription.subscriptionId = randomSubscriptionId;
        expectedUserSubscription.userId = user.id;
        expectedUserSubscription.transactionId = undefined;

        const result = await service.processSubscriptionPurchase(user.id, {subscriptionId: randomSubscriptionId, card: null});

        expect(fakeUserSubscriptions).toContainEqual({
            ...expectedUserSubscription,
            id: expect.any(Number),
            boughtAt: expect.any(Date),
            expiresAt: expect.any(Date),
            status: expect.any(Number),
            transactionId: null
        })

        const {transactionId, ...rest} = result;
        expect(rest).toEqual({
            ...expectedUserSubscription,
            id: expect.any(Number),
            expiresAt: expect.any(Date),
            boughtAt: expect.any(Date),
            status: expect.any(Number)
        });
    });

    it('should return all bought subscriptions by user nickname', async()=>{
        const randomUser = fakeUsers[Math.floor(Math.random() * fakeUsers.length)];

        const expectedUserSubscriptions = fakeUserSubscriptions.filter(us => us.userId == randomUser.id);

        const result = await service.getBoughtSubscriptionsByUserId(randomUser.id);

        expect(result).toEqual(expectedUserSubscriptions);
    });

    it('should cancel subscription', async() => {
        const randomUser = fakeUsers[0];
        const randomSubscriptionId = fakeSubscriptions[Math.floor(Math.random() * fakeSubscriptions.length)].id;
        await service.processSubscriptionPurchase(randomUser.id, {subscriptionId: randomSubscriptionId, card: null});

        await service.cancelSubscription(randomUser.id, randomSubscriptionId);

        expect(fakeUserSubscriptions).toContainEqual({
            id: expect.any(Number),
            transactionId: null,
            subscriptionId: randomSubscriptionId,
            userId: randomUser.id,
            boughtAt: expect.any(Date),
            expiresAt: expect.any(Date),
            status: UserSubscriptionStatus.CANCELLED
        });
    })
})
