namespace Application.Identity;

public class RefreshToken
{
    public int Id { get; set; }
    
    public long UserId { get; set; }
    
    public long AppUserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}