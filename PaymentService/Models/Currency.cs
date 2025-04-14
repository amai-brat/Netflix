namespace PaymentService.Models;

public class Currency
{
    public int Id { get; init; }

    private readonly string _name = null!;
    public string Name 
    {
        get => _name;
        init
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 10)
            {
                throw new ArgumentException("Currency name length should be between 3 and 10", nameof(value));
            }
            
            _name = value;
        }
    }

    public static Currency Create(int id, string name)
    {
        return new Currency { Id = id, Name = name };
    }
}