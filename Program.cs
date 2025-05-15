
using GameStoreApi.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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


app.MapGet("/", () => "Hello World!");

app.MapGet("/games" , () => games );


app.MapGet("/games/{id}" , 
(int id) => games.Find(g => g.Id == id) ).WithName(GetGameEndpointName);

app.MapGet("/games/active", () => games.Where(g => g.isActive).ToList());

app.MapGet("/games/inActive" , () => games.Where(g => !g.isActive).ToList() );

//POST 

app.MapPost("/games/add" , (CreateGameDto newGame) =>{
    GameDto game = new (
        games.Count + 1,
        newGame.name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate,
        true
    );

    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName ,new {id  = game.Id} , game );
});

app.MapPatch("/games/{id}", (int id, UpdateGameDto updatedFields) =>
{
    if(id <= -1){
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
        name = updatedFields.name ?? game.name,
        Genre = updatedFields.Genre ?? game.Genre,
        Price = updatedFields.Price ?? game.Price,
        ReleaseDate = updatedFields.ReleaseDate ?? game.ReleaseDate,
    };

    // Replace in the list
    var index = games.FindIndex(g => g.Id == id);
    games[index] = updatedGame;

    return Results.Ok(updatedGame);
});


app.MapDelete("/games/{id}" , (int id) =>{
    if(id == -1){
        return Results.NotFound();
    }
    var game = games.Find(g => g.Id == id);
    if(game  is  null){
        return Results.NotFound();
    }
    games.Remove(game);
    return Results.NoContent();
});

app.Run();