namespace Domain;

public class SerialContent : ContentBase
{
    public List<SeasonInfo> SeasonInfos { get; set; }  = null!;
    public YearRange YearRange { get; set; }  = null!;
}