namespace Application.Features.Contents.Queries.GetContentsByFilter
{
    public class Filter
    {
        public string? Name { get; set; }
        public List<int>? Types {  get; set; }
        public List<int>? Genres { get; set; }
        public string? Country { get; set; }
        public int? ReleaseYearFrom { get; set; }
        public int? ReleaseYearTo { get; set; }
        public double? RatingFrom { get; set; }
        public double? RatingTo { get; set; }
    }
}
