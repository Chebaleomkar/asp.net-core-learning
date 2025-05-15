namespace GameStoreApi.Dtos;

public record class UpdateGameDto(
    string? name ,
    string? Genre ,
    decimal? Price ,
    DateOnly? ReleaseDate
);