using GameStoreApi.Data;
using GameStoreApi.Dtos;
using GameStoreApi.Entities;
using GameStoreApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.EndPoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [
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

        GAMEGROUP.MapGet("/", async (GameStoreContext dbContext) =>
        {
            GameSummaryDto[] allGames = await dbContext.Games
            .AsNoTracking()
            .Include(x => x.genre)
            .Select(g => g.ToGameSummaryDto())
            .ToArrayAsync();
            if (!allGames.Any())
            {
                return Results.NotFound(new { Message = "No games found in DB" });
            }

            return Results.Ok(allGames);
        });

        GAMEGROUP.MapGet("/{id}",async(int id, GameStoreContext dbContext) =>
            {
                Game? game = await dbContext.Games.FindAsync(id);
                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
            .WithName(GetGameEndpointName);

        GAMEGROUP.MapGet("/active", async (GameStoreContext dbContext) =>{
                GameSummaryDto[] activeGames = await dbContext.Games
                    .AsNoTracking()
                    .Where(x => x.isActive)
                    .Include(x => x.genre)
                    .Select(x => x.ToGameSummaryDto())
                    .ToArrayAsync();

                return activeGames.Length > 0
                    ? Results.Ok(activeGames)
                    : Results.NotFound(new { Message = "No active games found in DB" });
        });

        GAMEGROUP.MapGet("/inActive", async (GameStoreContext dbContext) =>
            {
                GameSummaryDto[] inActiveGames = await dbContext.Games
                .AsNoTracking()
                    .Where(x => !x.isActive)
                    .Include(x => x.genre)
                    .Select(x => x.ToGameSummaryDto())
                    .ToArrayAsync();
                return inActiveGames.Length > 0 
                    ? Results.Ok(new { message = "Inactive games fetched", data = inActiveGames })
                    : Results.NotFound(new { message = "No inactive games found in DB" });
            });

        GAMEGROUP.MapPost("/add", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        })
        .WithParameterValidation();

        GAMEGROUP.MapPatch("/{id}", (int id, UpdateGameDto updatedFields, GameStoreContext dbContext) =>
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

        GAMEGROUP.MapDelete("/{id}", async (int id , GameStoreContext dbContext ) =>{
            if(id <= 0){
                return Results.BadRequest(new {message= "Enter a valid id"});
            }
            
            var gameToDelete = await dbContext.Games.FindAsync(id);
            if(gameToDelete is null){
                return Results.NotFound( new { message = "Game not found"}  );
            }

            dbContext.Games.Remove(gameToDelete);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new {message= "Game deleted successfully"});
        });

        return GAMEGROUP;
    }
}