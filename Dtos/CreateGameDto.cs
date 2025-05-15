namespace GameStoreApi.Dtos;

public record  class CreateGameDto(
    string name ,
    string Genre ,
    decimal Price ,
    DateOnly ReleaseDate
);