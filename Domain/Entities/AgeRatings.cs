using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Owned]
public class AgeRatings
{
    public int Age { get; set; }
    public string? AgeMpaa { get; set; }
}