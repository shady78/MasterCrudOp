using MasterCrudOp.DTOs.Requests;
using MasterCrudOp.Exceptions;
using MasterCrudOp.Services;

namespace MasterCrudOp.Endpoints;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
    {
        var movieApi = routes.MapGroup("/api/movies").WithTags("Movies");

        // POST /api/movies => create
        movieApi.MapPost("/", async (IMovieService service, CreateMovieDto command) =>
        {
            var movie = await service.CreateMovieAsync(command);
            return TypedResults.Created($"/api/movie/{movie.Id}", movie);
        });

        //GET /api/movies => get all
        movieApi.MapGet("/", async (IMovieService service) =>
        {
            var movies = await service.GetAllMoviesAsync();
            return TypedResults.Ok(movies);
        });

        // GET /api/movies/{id} => get by id
        movieApi.MapGet("/{id}", async (IMovieService service, Guid id) =>
        {
              var movie = await service.GetMovieByIdAsync(id);
            
            //return movie is null
            //? (IResult)TypedResults.NotFound(new { Message = $"Movie with ID {id} Not Found." })
            //: TypedResults.Ok(movie);

            return movie is null
             ? throw new NotFoundException("movie", id)
             : TypedResults.Ok(movie);
           
        });

        // PUT /api/movies/{id} => update
        movieApi.MapPut("/{id}", async (IMovieService service, Guid id, UpdateMovieDto command) =>
        {
            await service.UpdateMovieAsync(id, command);
            return TypedResults.NoContent();
        });

        // DELETE /api/movies/{id} => delete
        movieApi.MapDelete("/{id}", async (IMovieService service, Guid id) =>
        {
            await service.DeleteMovieAsync(id);
            return TypedResults.NoContent();
        });
    }
}
