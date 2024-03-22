using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Owned]
public class TrailerInfo
{
    public string Url { get; set; } = null!;
    public string Name { get; set; } = null!;
}