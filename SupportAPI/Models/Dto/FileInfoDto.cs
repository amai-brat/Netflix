﻿
namespace SupportAPI.Models.Dto;

public class FileInfoDto
{
    public required string Type { get; set; }
    public required Uri Src { get; set; }
    public required string Name { get; set; }
}