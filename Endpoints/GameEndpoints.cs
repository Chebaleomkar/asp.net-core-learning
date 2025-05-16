using GameStoreApi.Data;
using GameStoreApi.Dtos;
using GameStoreApi.Entities;
using GameStoreApi.Mapping;

namespace GameStoreApi.EndPoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
    new (
        1,
        "Street Fighter II",
        "Fighting",
        10.5M,
        new DateOnly(1992, 7, 15),
        true
    ),
    new (
        2,
        "The Legend of Zelda: Ocarina of Time",
        "Adventure",
        59.99M,
        new DateOnly(1998, 11, 21),
        false
    ),
    new (
        3,
        "Final Fantasy VII",
        "RPG",
        49.99M,
        new DateOnly(1997, 1, 31),
        true
    ),
    new (
        4,
        "Super Mario 64",
        "Platformer",
        39.99M,
        new DateOnly(1996, 6, 23),
        false
    ),
    new (
        5,
        "Doom",
        "Shooter",
        29.99M,
        new DateOnly(1993, 12, 10),
        true
    ),
    new (
        6,
        "Age of Empires II",
        "Strategy",
        24.99M,
        new DateOnly(1999, 9, 30),
        false
    ),
];  

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var GAMEGROUP = app.MapGroup("games");

        GAMEGROUP.MapGet("/", () => games);


        GAMEGROUP.MapGet("/{id}",
        (int id) => games.Find(g => g.Id == id)).WithName(GetGameEndpointName);

        GAMEGROUP.MapGet("/active", () => games.Where(g => g.isActive).ToList());

        GAMEGROUP.MapGet("/inActive", () => games.Where(g => !g.isActive).ToList());

        GAMEGROUP.MapPost("/add", (CreateGameDto newGame , GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.genre = dbContext.Genres.Find(newGame.GenreId);

            

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDto gameDto = new (
                game.Id,
                game.Name,
                game.genre!.Name,
                game.Price,
                game.ReleaseDate,
                isActive : true
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToDto());
        })
        .WithParameterValidation();

        GAMEGROUP.MapPatch("/{id}", (int id, UpdateGameDto updatedFields) =>
        {
            if (id <= -1)
            {
                return Results.NotFound();
            }
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            // Create a new GameDto with updated fields or keep existing values
            var updatedGame = game with
            {
                Name = updatedFields.name ?? game.Name,
                Genre = updatedFields.Genre ?? game.Genre,
                Price = updatedFields.Price ?? game.Price,
                ReleaseDate = updatedFields.ReleaseDate ?? game.ReleaseDate,
            };

            // Replace in the list
            var index = games.FindIndex(g => g.Id == id);
            games[index] = updatedGame;

            return Results.Ok(updatedGame);
        })
        .WithParameterValidation();

        GAMEGROUP.MapDelete("/{id}", (int id) =>
        {
            if (id == -1)
            {
                return Results.NotFound();
            }
            var game = games.Find(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }
            games.Remove(game);
            return Results.NoContent();
        });

        return GAMEGROUP;
    }
}