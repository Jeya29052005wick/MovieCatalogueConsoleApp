
using MovieCatalogueEFCore.Models;

namespace MovieCatalogue.Services;

public interface IMovieService
{
    Task AddMovieAsync(Movie movie);

    Task<List<Movie>> GetAllMoviesAsync();

    Task<Movie?> GetMovieByIdAsync(int id);

    Task UpdateMovieAsync(int id, Movie updatedMovie);

    Task DeleteMovieAsync(int id);

    Task<List<Movie>> SearchMoviesAsync(string search);

    Task<List<Movie>> FilterMoviesByGenreAsync(string genre);
}
