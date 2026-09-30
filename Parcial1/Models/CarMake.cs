namespace Parcial1.Models;

public class CarMake
{
    public int MakeId { get; set; }

    public string MakeName { get; set; } = string.Empty;

    public int VehicleTypeId { get; set; }

    public string VehicleTypeName { get; set; } = string.Empty;
}