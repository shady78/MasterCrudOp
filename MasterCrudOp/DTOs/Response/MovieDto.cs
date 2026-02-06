namespace MasterCrudOp.DTOs.Response;

public record MovieDto(Guid Id , string Title, string Genre , DateTimeOffset ReleaseDate , double Rating);