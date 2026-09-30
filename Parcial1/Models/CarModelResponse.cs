namespace Parcial1.Models;

public class CarModelResponse
{
    public int Count { get; set; }

    public string Message { get; set; } = string.Empty;

    public string SearchCriteria { get; set; } = string.Empty;

    public List<CarModel> Results { get; set; } = new();
}