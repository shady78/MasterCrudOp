using MasterCrudOp.DTOs.Requests;
using MasterCrudOp.DTOs.Response;
using MasterCrudOp.Models;
using MasterCrudOp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterCrudOp.Services;

public class MovieService : IMovieService
{
    private readonly MovieDbContex _context;
    private readonly ILogger<MovieService> _logger;
    public MovieService(MovieDbContex context, ILogger<MovieService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto command)
    {
        var movie = Movie.Create(command.Title, command.genre, command.ReleaseDate, command.Rating);

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        // TODO : use Manual Mapping here 
        return new MovieDto(
        movie.Id,
        movie.Title,
        movie.Genre,
        movie.ReleasedDate,
        movie.Rating
     );
    }

    public async Task DeleteMovieAsync(Guid id)
    {
        var movieToDelete = await _context.Movies.FindAsync(id);
        if (movieToDelete is not null)
        {
            _context.Movies.Remove(movieToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        _logger.LogInformation("start get all movies");  /// Information
        _logger.LogCritical("oops");   // Fatal 
        _logger.LogDebug("nothing much to say");
        //_logger.LogInformation("Invoke {Event} with ID as {Id}", "SomeEvent", Guid.NewGuid());
        var ev = new { Name = "SomeEvent", Type = "Demo" };
        _logger.LogInformation("Invoke {Event}", ev);
        
        return await _context.Movies
             .AsNoTracking()
             .Select(movie => new MovieDto(
                 movie.Id,
                 movie.Title,
                 movie.Genre,
                 movie.ReleasedDate,
                 movie.Rating))
             .ToListAsync();
    }

    public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
    {
        var movie = await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie is null)
            return null;

        return new MovieDto(
            movie.Id,
            movie.Title,
            movie.Genre,
            movie.ReleasedDate,
            movie.Rating);
    }

    public async Task UpdateMovieAsync(Guid id, UpdateMovieDto command)
    {
        var movieToUpdate = await _context.Movies.FindAsync(id);
        if (movieToUpdate is null)
            throw new ArgumentNullException($"Invalid Movie Id.");

        //_context.Movies.Update(movieToUpdate);
        movieToUpdate.Update(command.Title, command.genre, command.ReleaseDate, command.Rating);
        await _context.SaveChangesAsync();
    }
}


/*
POST /api/movies => create

GET /api/movies => get all

GET /api/movies/{id} => get by id

PUT /api/movies/{id} => update

DELETE /api/movies/{id} => delete
 */