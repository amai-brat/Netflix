using Microsoft.EntityFrameworkCore;

namespace Domain;

[Owned]
public class YearRange
{
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
}