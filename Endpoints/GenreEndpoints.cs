using GameStoreApi.Data;
using GameStoreApi.Dtos;
using GameStoreApi.Entities;
using GameStoreApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.EndPoints;

public static class GenreEndpoints
{
    const string GetGenreEndpointName = "GetGenre";

    public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
    {
        var GENREGROUP = app.MapGroup("genre");

        GENREGROUP.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var genreList = await dbContext.Genres.AsNoTracking().ToArrayAsync();

            if (!genreList.Any())
            {
                return Results.NotFound(new { message = "No Genre found in DB" });
            }

            return Results.Ok(genreList);
        });

        GENREGROUP.MapGet("/{id}" , async (int id ,GameStoreContext dbContext) =>{
            Genre?  genre = await dbContext.Genres.FindAsync(id);
            return genre is null ? Results.NotFound(new {message = "No Genre found by id"}) : Results.Ok(genre);
        })
        .WithName(GetGenreEndpointName);

        GENREGROUP.MapPost("/add" , async (CreateGenreDto newGenre , GameStoreContext  dbContext) =>{
            Genre genre = newGenre.ToEntity();
            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();
            return Results.CreatedAtRoute(GetGenreEndpointName , new { id = genre.Id} , genre );
        }).WithParameterValidation();

        GENREGROUP.MapPatch("/{id}" , async(int id , UpdateGenreDto newGenre ,GameStoreContext dbContext  ) =>
        {
            Genre?  genre = await dbContext.Genres.FindAsync(id);
            if (genre is null) return Results.NotFound(new {message = "Genre not found by id to perform update operation"});

            genre.Name = newGenre.Name;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new { message = "Genre Updated successfully" , genre});

        }).WithParameterValidation();

        GENREGROUP.MapDelete("/{id}" , async(int id , GameStoreContext dbContext) => {
            Genre? genre = await dbContext.Genres.FindAsync(id);
            if (genre is null) return Results.NotFound(new {message = "Genre not found by id to perform delete operation"});

            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();
            return Results.Ok(new {message = "Genre Deleted successfully"});
        });

        return GENREGROUP;
    }
}
