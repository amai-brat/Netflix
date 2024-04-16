namespace Application.Services.Abstractions;

public interface IPasswordHasher
{
    public string Hash(string input);
    public bool Verify(string input, string hashString);
}