﻿namespace Domain;

public class FavouriteContent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public DateTime AddedAt { get; set; }
}