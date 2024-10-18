using BattleShip.API;
using Battleship.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using BattleShip.Models;

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



app.MapGet("/start", () =>
{
    string idParty;
    (BatailleNavale[] grilles, idParty) = game.startGame();
    return TypedResults.Ok(
       new { grilles, idParty}
        );
});

app.MapGet("/atk/{x}/{y}/ia", ([FromRoute] string x, [FromRoute] string y) =>
{
    (bool touche, bool toucheIa, List  <string> shotByPlayer, List<string> shotByIa, string winner) = game.atkWithIa(x, y);
    return TypedResults.Ok(new { touche, toucheIa, shotByPlayer, shotByIa, winner });

});




app.Run();
