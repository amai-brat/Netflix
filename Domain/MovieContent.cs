namespace ClassLibrary1;

public class MovieContent : ContentBase
{
    public int MovieLength { get; set; }
    public List<PersonInMovie> PersonsInMovie { get; set; }
    public int Year { get; set; }
    public Type Type { get; set; }
}