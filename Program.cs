using GameStoreApi.Data;
using GameStoreApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var  connectionString = "Data source=GameStore.db";

builder.Services.AddSqlite<GameStoreContext>(connectionString);



app.MapGet("/", () => "Hello World!");

app.MapGamesEndpoints();

app.Run();