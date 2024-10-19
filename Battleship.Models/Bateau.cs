using Battleship.Models;

public class BateauPositions
{
    public Dictionary<string, List<string>> PositionsBateaux { get; set; }
}

public class ApiResponse
{
    public List<BateauPositions> PositionsBateaux { get; set; }
}

public class BateauInfo
{
    public List<Dictionary<string, List<string>>> PositionsBateaux { get; set; }
}

public class PositionBateau
{
    public Dictionary<string, List<string>> Positions { get; set; }
}

public class Grille
{
    public List<Dictionary<string, List<string>>> PositionsBateaux { get; set; }
}

public class GrilleResponse
{
    public List<BatailleNavale> Grilles { get; set; }
    public string IdParty { get; set; }
}