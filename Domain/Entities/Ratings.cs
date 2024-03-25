using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Owned]
public class Ratings
{
    public float? KinopoiskRating { get; set; }
    public float? ImdbRating { get; set; }
    public float? LocalRating { get; set; }
}