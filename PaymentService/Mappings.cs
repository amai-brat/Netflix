using Payment;
using PaymentService.Models;
using Card = Payment.Card;

namespace PaymentService;

public static class Mappings
{
    public static Models.Card ToCardEntity(this Card card)
    {
        return new Models.Card
        {
            CardNumber = card.CardNumber,
            CardOwner = card.CardOwner,
            ValidThru = card.ValidThru,
            Cvc = card.Cvc,
        };
    }
}