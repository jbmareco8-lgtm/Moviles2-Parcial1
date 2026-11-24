using Parcial1.Core.Models;

namespace Parcial1.Core.Validators;

public static class FavoriteMakeValidator
{
    public static void Validate(FavoriteMake favorite)
    {
        ArgumentNullException.ThrowIfNull(favorite);

        ValidateId(favorite.MakeId);

        if (string.IsNullOrWhiteSpace(favorite.MakeName))
        {
            throw new ArgumentException(
                "El nombre de la marca es obligatorio.");
        }
    }

    public static void ValidateId(int makeId)
    {
        if (makeId <= 0)
        {
            throw new ArgumentException(
                "El ID de la marca debe ser mayor a cero.");
        }
    }
}