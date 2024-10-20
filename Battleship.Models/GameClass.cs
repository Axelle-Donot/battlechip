using Battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models
{
    public class Game
    {
        BatailleNavale[] grilles = { };
        List<string> shotByIa = new List<string>();
        List<string> shotByPlayer = new List<string>();
        int toucheCount = 0;
        int toucheIaCount = 0;
        string winner = "_NONE_";
        public string? idParty;  // Rendue nullable pour éviter l'erreur

        public Game() {
            idParty = "";  // Initialisation pour éviter le warning
        }

        public (BatailleNavale[], string) startGame(Dictionary<string, List<string>> positionsBateauxManuelles)
        {
            shotByIa.Clear();
            shotByPlayer.Clear();
            toucheCount = 0;
            toucheIaCount = 0;
            winner = "_NONE_";

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] stringChars = new char[5];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            this.idParty = new string(stringChars);
            Console.WriteLine("Chaîne aléatoire : " + idParty);

            // Créer la grille manuelle
            grilles = new BatailleNavale[] 
            { 
                new BatailleNavale(new List<Dictionary<string, List<string>>> { positionsBateauxManuelles }), // grille manuelle
                new BatailleNavale() // grille IA générée aléatoirement
            };

            return (grilles, idParty);
        }

        public (bool, bool, List<string>, List<string>, string) atkWithIa(string x, string y)
        {
            var touche = false;

            // Parcourir les positions de la grille de l'IA (grilles[1])
            foreach (var bateau in grilles[1].PositionsBateaux)
            {
                foreach (var coord in bateau.Value) // Utilisation correcte de bateau.Value pour accéder à la liste de coordonnées
                {
                    if (coord == x + y)
                    {
                        touche = true;
                    }
                    Console.WriteLine($"  Coordonnée: {coord}");
                }
            }

            Console.WriteLine(x);
            Console.WriteLine(y);
            var positionsBateaux = grilles[0].PositionsBateaux;
            shotByPlayer.Add(x + y);

            string[] xCoord = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" }; // Correction de la déclaration du tableau
            var isAlreadyShot = false;
            do
            {
                isAlreadyShot = false;

                var yCoord = Random.Shared.Next(10).ToString();  // Convertir yCoord en string pour corriger l'erreur CS1503
                var xIndex = Random.Shared.Next(xCoord.Length);
                foreach (var e in shotByIa)
                {
                    if (e == (xCoord[xIndex] + yCoord))
                    {
                        isAlreadyShot = true;
                    }
                    Console.WriteLine(e + " / " + xCoord[xIndex] + yCoord + " / " + isAlreadyShot);
                }
                if (!isAlreadyShot)
                {
                    shotByIa.Add(xCoord[xIndex] + yCoord);
                }
                Console.WriteLine("next-------------------------");
            } while (isAlreadyShot);

            var toucheIa = false;

            // Parcourir les positions de la grille du joueur (grilles[0])
            foreach (var bateau in grilles[0].PositionsBateaux)
            {
                foreach (var coord in bateau.Value) // Utilisation correcte de bateau.Value pour accéder à la liste de coordonnées
                {
                    if (coord == shotByIa.Last())
                    {
                        toucheIa = true;
                    }
                    Console.WriteLine($"  Coordonnée: {coord}");
                }
            }

            if (touche)
            {
                toucheCount++;
            }
            if (toucheIa)
            {
                toucheIaCount++;
            }

            if (toucheCount == 15)
            {
                winner = "_PLAYER_";
            }
            else if (toucheIaCount == 15)
            {
                winner = "_IA_";
            }

            return (touche, toucheIa, shotByPlayer, shotByIa, winner);
        }
    }
}
