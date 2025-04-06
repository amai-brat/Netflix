import { ApiProperty } from '@nestjs/swagger';

class CardDto {
  @ApiProperty()
  cardNumber: string;

  @ApiProperty()
  cardOwner: string;

  @ApiProperty()
  validThru: string;

  @ApiProperty()
  cvc: number;
}

export class BuySubscriptionDto {
  @ApiProperty()
  subscriptionId: number;

  @ApiProperty()
  card: CardDto;
}
