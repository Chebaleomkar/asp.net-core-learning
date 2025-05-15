namespace GameStoreApi.Dtos;

public record class GameDto(
    int Id ,
    string name ,
    string Genre ,
    decimal Price ,
    DateOnly ReleaseDate,
    bool isActive
);