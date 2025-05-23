using GameStoreApi.Data;
using GameStoreApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var  connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.MapGamesEndpoints();
app.MapGenreEndpoints();
app.MigrateDb();


app.Run();