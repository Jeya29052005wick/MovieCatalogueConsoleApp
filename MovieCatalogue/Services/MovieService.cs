
using MovieCatalogue.Repositories;
using MovieCatalogueEFCore.Models;

namespace MovieCatalogue.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    // Add Movie
    public async Task AddMovieAsync(Movie movie)
    {
        ValidateMovie(movie);

        movie.Title = movie.Title.Trim();
        movie.Genre = movie.Genre.Trim();
        movie.Language = movie.Language.Trim();

        await _repository.AddAsync(movie);
    }

    // Get All Movies
    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await _repository.GetAllAsync();
    }

    // Get Movie By ID
    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        return await _repository.GetByIdAsync(id);
    }

    // Update Movie
    public async Task UpdateMovieAsync(int id, Movie updatedMovie)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        ValidateMovie(updatedMovie);

        var existingMovie = await _repository.GetByIdAsync(id);

        if (existingMovie == null)
            throw new KeyNotFoundException("Movie not found.");

        existingMovie.Title = updatedMovie.Title.Trim();
        existingMovie.Genre = updatedMovie.Genre.Trim();
        existingMovie.Language = updatedMovie.Language.Trim();
        existingMovie.DurationMinutes = updatedMovie.DurationMinutes;
        existingMovie.ReleaseYear = updatedMovie.ReleaseYear;

        await _repository.UpdateAsync(existingMovie);
    }

    // Delete Movie
    public async Task DeleteMovieAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var movie = await _repository.GetByIdAsync(id);

        if (movie == null)
            throw new KeyNotFoundException("Movie not found.");

        await _repository.DeleteAsync(movie);
    }

    // Search Movie By Title
    public async Task<List<Movie>> SearchMoviesAsync(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
            throw new ArgumentException("Search text cannot be empty.");

        return await _repository.SearchByTitleAsync(search.Trim());
    }

    // Filter Movie By Genre
    public async Task<List<Movie>> FilterMoviesByGenreAsync(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Genre cannot be empty.");

        return await _repository.FilterByGenreAsync(genre.Trim());
    }

    // Movie Validation
    private static void ValidateMovie(Movie movie)
    {
        if (movie == null)
            throw new ArgumentException("Movie details are required.");

        if (string.IsNullOrWhiteSpace(movie.Title))
            throw new ArgumentException("Movie title is required.");

        if (movie.Title.Trim().Length > 150)
            throw new ArgumentException("Title cannot exceed 150 characters.");

        if (string.IsNullOrWhiteSpace(movie.Genre))
            throw new ArgumentException("Genre is required.");

        if (movie.Genre.Trim().Length > 50)
            throw new ArgumentException("Genre cannot exceed 50 characters.");

        if (string.IsNullOrWhiteSpace(movie.Language))
            throw new ArgumentException("Language is required.");

        if (movie.Language.Trim().Length > 50)
            throw new ArgumentException("Language cannot exceed 50 characters.");

        if (movie.DurationMinutes <= 0)
            throw new ArgumentException("Duration must be greater than zero.");

        int currentYear = DateTime.Now.Year;

        if (movie.ReleaseYear < 1888 || movie.ReleaseYear > currentYear + 5)
            throw new ArgumentException("Invalid release year.");
    }
}
