using GameStoreApi.Dtos;
using GameStoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto game)
    {
        return new Game()
        {
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate

        };
    }

    public static GameSummaryDto ToGameSummaryDto(this Game game)
    {
        return new (
            game.Id,
            game.Name,
            game.genre!.Name,
            game.Price,
            game.ReleaseDate,
            game.isActive
        );
    }
    public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
        return new (
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate,
            game.isActive
        );
    }
}