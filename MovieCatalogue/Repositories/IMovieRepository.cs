using MovieCatalogueEFCore.Models;

namespace MovieCatalogue.Repositories;

public interface IMovieRepository
{
    Task AddAsync(Movie movie);

    Task<List<Movie>> GetAllAsync();

    Task<Movie?> GetByIdAsync(int id);

    Task UpdateAsync(Movie movie);

    Task DeleteAsync(Movie movie);

    Task<List<Movie>> SearchByTitleAsync(string search);

    Task<List<Movie>> FilterByGenreAsync(string genre);
}
