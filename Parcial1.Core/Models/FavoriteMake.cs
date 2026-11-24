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
}