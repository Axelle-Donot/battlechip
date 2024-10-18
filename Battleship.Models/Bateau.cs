using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BattleShip.Models
{
    public class Bateau
    {
        [JsonPropertyName("positionsBateaux")]
        public List<Dictionary<string, List<string>>> PositionsBateaux { get; set; }
    }
}
