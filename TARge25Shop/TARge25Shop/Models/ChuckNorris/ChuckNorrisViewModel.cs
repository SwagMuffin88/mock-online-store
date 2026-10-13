namespace TARge25Shop.Models.ChuckNorris;

public class ChuckNorrisViewModel
{
    public List<string> Categories { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string Id { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}