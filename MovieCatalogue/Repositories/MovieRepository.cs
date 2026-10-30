
using Microsoft.EntityFrameworkCore;
using MovieCatalogueEFCore.Data;
using MovieCatalogueEFCore.Models;

namespace MovieCatalogue.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _context;

    public MovieRepository(AppDbContext context)
    {
        _context = context;
    }

    // Add Movie
    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    // Get All Movies
    public async Task<List<Movie>> GetAllAsync()
    {
        return await _context.Movies
            .AsNoTracking()
            .ToListAsync();
    }

    // Get Movie By ID
    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    // Update Movie
    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    // Delete Movie
    public async Task DeleteAsync(Movie movie)
    {
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
    }

    // Search Movie By Title
    public async Task<List<Movie>> SearchByTitleAsync(string search)
    {
        return await _context.Movies
            .AsNoTracking()
            .Where(m => m.Title.Contains(search))
            .ToListAsync();
    }

    // Filter Movies By Genre
    public async Task<List<Movie>> FilterByGenreAsync(string genre)
    {
        return await _context.Movies
            .AsNoTracking()
            .Where(m => m.Genre == genre)
            .ToListAsync();
    }
}
