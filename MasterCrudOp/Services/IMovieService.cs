using MasterCrudOp.DTOs.Requests;
using MasterCrudOp.DTOs.Response;

namespace MasterCrudOp.Services;

public interface IMovieService
{
    // CRUD
    Task<MovieDto> CreateMovieAsync(CreateMovieDto command);
    Task UpdateMovieAsync(Guid id,UpdateMovieDto command);
    Task DeleteMovieAsync(Guid id);

    Task<MovieDto?> GetMovieByIdAsync(Guid id);
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
}
