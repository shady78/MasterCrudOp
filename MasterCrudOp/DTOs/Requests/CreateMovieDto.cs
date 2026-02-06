namespace MasterCrudOp.DTOs.Requests;

public record CreateMovieDto(string Title, string genre , DateTimeOffset ReleaseDate , double Rating);