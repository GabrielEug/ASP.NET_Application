using GameStore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "Elden Ring",
        "Roleplay",
        49.99M,
        new DateOnly(2021, 06, 20)),
    new (
        2,
        "Alan Wake",
        "Horror",
        79.99M,
        new DateOnly(2022, 05, 6)),
    new (
        3,
        "The Last Of Us",
        "Action",
        25.99M,
        new DateOnly(2018, 06, 18)),
    new (
        4,
        "Baldurs Gate",
        "Roleplay",
        80.99M,
        new DateOnly(2023, 02, 23))
];
// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => 
{
    GameDto? game = games.Find((game) => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDto newGame) => 
{
    
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    
    games.Add(game);

    return Results.AcceptedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
});
//app.MapGet("games/genre/{genre}", (string genre) => games.FindAll((game) => game.Genre == genre));

// PUT /games/1
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) => 
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }
    
    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/1
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});
app.Run();
