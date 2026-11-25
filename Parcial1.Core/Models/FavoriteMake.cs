using SQLite;

namespace Parcial1.Core.Models;

[Table("FavoriteMakes")]
public class FavoriteMake
{
    [PrimaryKey]
    public int MakeId { get; set; }

    [NotNull]
    public string MakeName { get; set; } = string.Empty;

    public string VehicleTypeName { get; set; } = string.Empty;

    public DateTime AddedAt { get; set; } = DateTime.Now;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [Ignore]
    public string LocationText =>
        Latitude.HasValue && Longitude.HasValue
            ? $"Ubicacion: {Latitude.Value:F5}, {Longitude.Value:F5}"
            : "Sin ubicacion guardada";
}