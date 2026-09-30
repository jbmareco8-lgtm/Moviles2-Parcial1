using System.Text.Json.Serialization;

namespace Parcial1.Models;

public class CarModel
{
    [JsonPropertyName("Make_ID")]
    public int MakeId { get; set; }

    [JsonPropertyName("Make_Name")]
    public string MakeName { get; set; } = string.Empty;

    [JsonPropertyName("Model_ID")]
    public int ModelId { get; set; }

    [JsonPropertyName("Model_Name")]
    public string ModelName { get; set; } = string.Empty;
}