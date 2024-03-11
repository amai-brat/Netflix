﻿namespace ClassLibrary1;

public class ContentBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AgeRating? AgeRating { get; set; }
    public string Description { get; set; }
    public Type Type { get; set; }
    public string? Slogan { get; set; }
    public Ratings Ratings { get; set; }
    public string PosterUrl { get; set; }
    public string VideoUrl { get; set; }
    public TrailerInfo? TrailerInfo { get; set; }
    public List<Genre> Genres { get; set; }
    public CurrencyValue? Budget { get; set; }
    // TODO: как хранить постер большой в главной странице контента, мб что-то с logo связано в кинопоиске
    // TODO: Избранное
    // TODO: Рецензии
    // TODO: комменты и лайки
}