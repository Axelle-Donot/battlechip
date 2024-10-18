public class BateauPositions
{
    public Dictionary<string, List<string>> PositionsBateaux { get; set; }
}

public class ApiResponse
{
    public List<BateauPositions> PositionsBateaux { get; set; }
}