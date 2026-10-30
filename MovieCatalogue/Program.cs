
using Microsoft.Extensions.DependencyInjection;
using MovieCatalogue.Repositories;
using MovieCatalogue.Services;
using MovieCatalogueEFCore.Data;
using MovieCatalogueEFCore.Models;

namespace MovieCatalogue;

public class Program
{
    public static async Task Main(string[] args)
    {
        // 1. Configure Dependency Injection
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>();

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var movieService = scope.ServiceProvider
            .GetRequiredService<IMovieService>();

        // 2. Console Application
        bool running = true;

        Console.WriteLine("======================================");
        Console.WriteLine("       MOVIE CATALOGUE SYSTEM");
        Console.WriteLine("======================================");

        while (running)
        {
            Console.WriteLine("\n1. Add Movie");
            Console.WriteLine("2. View All Movies");
            Console.WriteLine("3. Get Movie By ID");
            Console.WriteLine("4. Update Movie");
            Console.WriteLine("5. Delete Movie");
            Console.WriteLine("6. Search Movie By Title");
            Console.WriteLine("7. Filter Movies By Genre");
            Console.WriteLine("8. Exit");

            Console.Write("\nEnter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AddMovie(movieService);
                        break;

                    case "2":
                        await ViewAllMovies(movieService);
                        break;

                    case "3":
                        await GetMovieById(movieService);
                        break;

                    case "4":
                        await UpdateMovie(movieService);
                        break;

                    case "5":
                        await DeleteMovie(movieService);
                        break;

                    case "6":
                        await SearchMovies(movieService);
                        break;

                    case "7":
                        await FilterMovies(movieService);
                        break;

                    case "8":
                        running = false;
                        Console.WriteLine("Thank you for using Movie Catalogue!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Not Found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // 1. Add Movie
    private static async Task AddMovie(IMovieService service)
    {
        Console.Write("\nEnter movie title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Enter genre: ");
        string genre = Console.ReadLine() ?? "";

        Console.Write("Enter language: ");
        string language = Console.ReadLine() ?? "";

        Console.Write("Enter duration in minutes: ");

        if (!int.TryParse(Console.ReadLine(), out int duration))
        {
            Console.WriteLine("Invalid duration.");
            return;
        }

        Console.Write("Enter release year: ");

        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("Invalid release year.");
            return;
        }

        var movie = new Movie
        {
            Title = title,
            Genre = genre,
            Language = language,
            DurationMinutes = duration,
            ReleaseYear = year
        };

        await service.AddMovieAsync(movie);

        Console.WriteLine("Movie added successfully!");
    }

    // 2. View All Movies
    private static async Task ViewAllMovies(IMovieService service)
    {
        var movies = await service.GetAllMoviesAsync();

        DisplayMovies(movies);
    }

    // 3. Get Movie By ID
    private static async Task GetMovieById(IMovieService service)
    {
        Console.Write("\nEnter movie ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var movie = await service.GetMovieByIdAsync(id);

        if (movie == null)
        {
            Console.WriteLine("Movie not found.");
            return;
        }

        DisplayMovie(movie);
    }

    // 4. Update Movie
    private static async Task UpdateMovie(IMovieService service)
    {
        Console.Write("\nEnter movie ID to update: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var existingMovie = await service.GetMovieByIdAsync(id);

        if (existingMovie == null)
        {
            Console.WriteLine("Movie not found.");
            return;
        }

        Console.Write("Enter new title: ");
        existingMovie.Title = Console.ReadLine() ?? "";

        Console.Write("Enter new genre: ");
        existingMovie.Genre = Console.ReadLine() ?? "";

        Console.Write("Enter new language: ");
        existingMovie.Language = Console.ReadLine() ?? "";

        Console.Write("Enter new duration: ");

        if (!int.TryParse(Console.ReadLine(), out int duration))
        {
            Console.WriteLine("Invalid duration.");
            return;
        }

        existingMovie.DurationMinutes = duration;

        Console.Write("Enter new release year: ");

        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("Invalid release year.");
            return;
        }

        existingMovie.ReleaseYear = year;

        await service.UpdateMovieAsync(id, existingMovie);

        Console.WriteLine("Movie updated successfully!");
    }

    // 5. Delete Movie
    private static async Task DeleteMovie(IMovieService service)
    {
        Console.Write("\nEnter movie ID to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        await service.DeleteMovieAsync(id);

        Console.WriteLine("Movie deleted successfully!");
    }

    // 6. Search Movie By Title
    private static async Task SearchMovies(IMovieService service)
    {
        Console.Write("\nEnter movie title to search: ");

        string search = Console.ReadLine() ?? "";

        var movies = await service.SearchMoviesAsync(search);

        DisplayMovies(movies);
    }

    // 7. Filter Movies By Genre
    private static async Task FilterMovies(IMovieService service)
    {
        Console.Write("\nEnter genre: ");

        string genre = Console.ReadLine() ?? "";

        var movies = await service.FilterMoviesByGenreAsync(genre);

        DisplayMovies(movies);
    }

    // Display multiple movies
    private static void DisplayMovies(List<Movie> movies)
    {
        if (movies.Count == 0)
        {
            Console.WriteLine("No movies found.");
            return;
        }

        Console.WriteLine("\n========== MOVIE LIST ==========");

        foreach (var movie in movies)
        {
            DisplayMovie(movie);
        }
    }

    // Display one movie
    private static void DisplayMovie(Movie movie)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"ID       : {movie.Id}");
        Console.WriteLine($"Title    : {movie.Title}");
        Console.WriteLine($"Genre    : {movie.Genre}");
        Console.WriteLine($"Language : {movie.Language}");
        Console.WriteLine($"Duration : {movie.DurationMinutes} minutes");
        Console.WriteLine($"Year     : {movie.ReleaseYear}");
        Console.WriteLine("--------------------------------");
    }
}
