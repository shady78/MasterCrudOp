namespace MasterCrudOp.DTOs.Requests;

public record UpdateMovieDto(string Title, string genre , DateTimeOffset ReleaseDate, double Rating);