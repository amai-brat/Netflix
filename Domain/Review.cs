﻿namespace Domain;

public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public string Text { get; set; } = null!;
    public bool IsPositive { get; set; }
    public int Score { get; set; }
    public DateTime WrittenAt { get; set; }
    public List<Comment>? Comments { get; set; }
}