using BattleShip.API;
using Battleship.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Game game = new Game();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/start", ([FromBody] PositionsBateauxRequest body) =>
{
    // Appeler la méthode startGame pour créer les deux grilles (manuelle et aléatoire)
    var (grilles, idParty) = game.startGame(body.PositionsBateaux);

    // Retourner les deux grilles et l'id de la partie
    return TypedResults.Ok(
    new
    {
        grilles = new[]
        {
            new
            {
                positionsBateaux = grilles[0].PositionsBateaux
            },
            new
            {
                positionsBateaux = grilles[1].PositionsBateaux
            }
        },
        idParty
    }
);

});


app.MapGet("/atk/{x}/{y}/ia", ([FromRoute] string x, [FromRoute] string y) =>
{
    (bool touche, bool toucheIa, List  <string> shotByPlayer, List<string> shotByIa, string winner) = game.atkWithIa(x, y);
    return TypedResults.Ok(new { touche, toucheIa, shotByPlayer, shotByIa, winner });

});




app.Run();
