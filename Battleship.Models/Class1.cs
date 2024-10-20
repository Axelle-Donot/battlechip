using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Models
{
    public class BatailleNavale
    {
        private const int TAILLE_GRILLE = 10;
        private static readonly (char lettre, int taille)[] bateaux = {
            ('A', 4), ('B', 3), ('C', 3), ('D', 2), ('E', 2), ('F', 1)
        };

        public List<Bateau> Bateaux { get; private set; }
        public Dictionary<string, List<string>> PositionsBateaux { get; private set; }

        // Constructeur pour le placement manuel
        public BatailleNavale(List<Dictionary<string, List<string>>> positionsBateaux)
        {
            Bateaux = new List<Bateau>();
            PositionsBateaux = new Dictionary<string, List<string>>();

            foreach (var positionBateau in positionsBateaux)
            {
                foreach (var bateauPosition in positionBateau)
                {
                    var lettre = bateauPosition.Key.Last();
                    var taille = bateaux.First(b => b.lettre == lettre).taille;

                    if (!PositionsBateaux.ContainsKey(bateauPosition.Key))
                    {
                        PositionsBateaux[bateauPosition.Key] = bateauPosition.Value;
                        var nouveauBateau = new Bateau(lettre, taille)
                        {
                            Positions = bateauPosition.Value
                        };
                        Bateaux.Add(nouveauBateau);
                    }
                }
            }
        }

        // Constructeur pour le placement aléatoire
        public BatailleNavale()
        {
            Bateaux = new List<Bateau>();
            PositionsBateaux = GenererPositionsAleatoires();
        }

        private Dictionary<string, List<string>> GenererPositionsAleatoires()
        {
            var positions = new Dictionary<string, List<string>>();
            var grilleTemp = new char[TAILLE_GRILLE, TAILLE_GRILLE];

            foreach (var bateau in bateaux)
            {
                bool bateauPlace = false;

                while (!bateauPlace)
                {
                    bool horizontal = Random.Shared.Next(2) == 0;
                    int ligne = Random.Shared.Next(TAILLE_GRILLE);
                    int colonne = Random.Shared.Next(TAILLE_GRILLE);

                    if (horizontal && colonne + bateau.taille <= TAILLE_GRILLE)
                    {
                        bool espaceLibre = true;
                        for (int i = 0; i < bateau.taille; i++)
                        {
                            if (grilleTemp[ligne, colonne + i] != '\0')
                            {
                                espaceLibre = false;
                                break;
                            }
                        }

                        if (espaceLibre)
                        {
                            List<string> positionsBateau = new List<string>();
                            for (int i = 0; i < bateau.taille; i++)
                            {
                                grilleTemp[ligne, colonne + i] = bateau.lettre;
                                char coordX = (char)('a' + ligne);
                                char coordY = (char)('1' + (colonne + i));
                                positionsBateau.Add($"{coordX}{coordY}");
                            }
                            positions.Add($"bateau-{bateau.lettre}", positionsBateau);
                            bateauPlace = true;
                        }
                    }
                    else if (!horizontal && ligne + bateau.taille <= TAILLE_GRILLE)
                    {
                        bool espaceLibre = true;
                        for (int i = 0; i < bateau.taille; i++)
                        {
                            if (grilleTemp[ligne + i, colonne] != '\0')
                            {
                                espaceLibre = false;
                                break;
                            }
                        }

                        if (espaceLibre)
                        {
                            List<string> positionsBateau = new List<string>();
                            for (int i = 0; i < bateau.taille; i++)
                            {
                                grilleTemp[ligne + i, colonne] = bateau.lettre;
                                char coordX = (char)('a' + (ligne + i));
                                char coordY = (char)('1' + colonne);
                                positionsBateau.Add($"{coordX}{coordY}");
                            }
                            positions.Add($"bateau-{bateau.lettre}", positionsBateau);
                            bateauPlace = true;
                        }
                    }
                }
            }

            return positions;
        }
    }
}
