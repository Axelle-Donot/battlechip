using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;


namespace BattleShip.App.Services;

public class GameState
{
    // Singleton Instance
    private static GameState? _instance = null;
    private static readonly object _lock = new object();
    private readonly HttpClient _httpClient;


    // Grille du joueur (char[,])
    public char[,] PlayerGrid { get; private set; }

    // Grille de l'adversaire (bool?[,])
    public bool?[,] OpponentGrid { get; private set; }

    // Taille des grilles (par exemple 10x10 pour un jeu classique)
    private const int gridSize = 10;

    // Constructeur privé pour empêcher l'instanciation
    public GameState()
    {
        // Initialisation des grilles
        PlayerGrid = new char[gridSize, gridSize];
        OpponentGrid = new bool?[gridSize, gridSize];

        // Optionnel: Initialiser la grille du joueur avec des bateaux (exemple)
        InitializePlayerGrid();
    }

    // Méthode pour récupérer l'instance unique
    public static GameState Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new GameState();
                }
                return _instance;
            }
        }
    }

    // Initialisation de la grille du joueur avec des bateaux
    private void InitializePlayerGrid()
    {
        // Exemple d'initialisation : placer des bateaux (indiqués par 'B')
        PlayerGrid[0, 0] = 'B'; // Placer un bateau sur la case (0,0)
        PlayerGrid[1, 1] = 'B'; // Placer un autre bateau sur la case (1,1)
                                // etc.
    }

    // Mettre à jour la grille de l'adversaire après un tir
    public void UpdateOpponentGrid(int x, int y, bool hit)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            OpponentGrid[x, y] = hit;
        }
    }

    // Vérifier l'état d'une case dans la grille de l'adversaire
    public string GetOpponentGridStatus(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
        {
            return "Invalid position";
        }

        bool? status = OpponentGrid[x, y];
        if (status == null)
        {
            return "Never fired";
        }
        else if (status == true)
        {
            return "Hit!";
        }
        else
        {
            return "Miss!";
        }
    }

    // Exemple de méthode pour vérifier si une case du joueur est touchée
    public bool IsPlayerHit(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return PlayerGrid[x, y] == 'B'; // 'B' représente un bateau
        }
        return false;
    }

    // Injection du HttpClient via constructeur
    public GameState(HttpClient httpClient)
    {
        _httpClient = httpClient;
        PlayerGrid = new char[gridSize, gridSize];
        OpponentGrid = new bool?[gridSize, gridSize];
        InitializePlayerGrid();
    }

    // Méthode pour appeler l'API
    public async Task CallApiAndLogResultAsync()
    {
        try
        {
            Console.WriteLine("Calling API...");

            var response = await _httpClient.GetAsync("http://localhost:5038/start");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GrilleResponse>();
                Console.WriteLine("API Response: " + result); 



                // Désérialisation du JSON
                //var bateauxData = JsonSerializer.Deserialize<List<BateauInfo>>(result);
                //ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                //Console.WriteLine("Bateau data : " + apiResponse);


                
                Console.WriteLine("Bateau data : ");
                //Console.WriteLine(bateauxData[0].PositionsBateaux[0][bateau-A][0]); 

                Console.WriteLine("---------------------------------------------------------------------");
                
                if (bateauxData != null && bateauxData.Count > 0)
                {
                    var positionsBateaux = bateauxData[0].PositionsBateaux;

                    // Placer les bateaux sur la grille du joueur
                    foreach (var bateau in positionsBateaux)
                    {
                        Console.WriteLine("Toto ");
                        Console.WriteLine(bateau);
                        foreach (var position in bateau.Values.SelectMany(x => x))
                        {
                            Console.WriteLine(position);
                            Console.WriteLine("Placing boat at: " + position); // Affiche chaque position de bateau
                            PlaceBoatOnPlayerGrid(position);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No boat data found in API response.");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
        }
    }
    private void PlaceBoatOnPlayerGrid(string position)
    {
        // Vérifie que la position est valide et contient au moins une lettre et un chiffre
        if (string.IsNullOrEmpty(position) || position.Length < 2)
        {
            Console.WriteLine("Invalid boat position: " + position);
            return;
        }

        // La première lettre représente la colonne (de 'a' à 'j')
        char colChar = char.ToLower(position[0]); // Convertit en minuscule
        int col = colChar - 'a'; // Convertit 'a'-'j' en 0-9

        // Le reste représente le numéro de ligne (de '1' à '10')
        string rowString = position.Substring(1);
        if (int.TryParse(rowString, out int row))
        {
            row--; // Conversion en 0-based index (1 devient 0)
        }
        else
        {
            Console.WriteLine("Invalid row in position: " + position);
            return;
        }

        // Vérifie que les indices sont valides
        if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
        {
            Console.WriteLine($"Placing boat at grid position [Row: {row}, Col: {col}]");
            PlayerGrid[row, col] = 'B'; // Marque la case avec un bateau
        }
        else
        {
            Console.WriteLine("Invalid grid position: " + position);
        }

        // Affiche le contenu de la grille après chaque placement
        Console.WriteLine("Current PlayerGrid:");
        for (int r = 0; r < gridSize; r++)
        {
            for (int c = 0; c < gridSize; c++)
            {
                Console.Write(PlayerGrid[r, c] + " ");
            }
            Console.WriteLine();
        }
    }


}