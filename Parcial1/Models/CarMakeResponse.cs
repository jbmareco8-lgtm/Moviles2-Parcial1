namespace Parcial1.Models;

public class CarMakeResponse
{
    public int Count { get; set; }

    public string Message { get; set; } = string.Empty;

    public string SearchCriteria { get; set; } = string.Empty;

    public List<CarMake> Results { get; set; } = new();
}