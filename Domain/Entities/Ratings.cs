﻿using Microsoft.EntityFrameworkCore;

namespace Domain;

[Owned]
public class Ratings
{
    public float? KinopoiskRating { get; set; }
    public float? ImdbRating { get; set; }
    public float? LocalRating { get; set; }
}