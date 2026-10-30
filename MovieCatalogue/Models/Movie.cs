
namespace MovieCatalogueEFCore.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public int ReleaseYear { get; set; }
}
