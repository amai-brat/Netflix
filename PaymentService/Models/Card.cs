namespace PaymentService.Models;

public class Card
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = null!;
    public string CardOwner { get; set; } = null!;
    public string ValidThru { get; set; } = null!;
    public int Cvc { get; set; }
}