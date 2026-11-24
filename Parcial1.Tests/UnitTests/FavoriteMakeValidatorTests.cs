using Parcial1.Core.Models;
using Parcial1.Core.Validators;
using Xunit;

namespace Parcial1.Tests.UnitTests;

public class FavoriteMakeValidatorTests
{
    [Fact]
    public void Validate_MarcaValida_NoLanzaExcepcion()
    {
        // Arrange
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota",
            VehicleTypeName = "Passenger Car"
        };

        // Act
        var exception = Record.Exception(() =>
            FavoriteMakeValidator.Validate(favorite));

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ValidateId_IdInvalido_LanzaExcepcion(int makeId)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            FavoriteMakeValidator.ValidateId(makeId));

        // Assert
        Assert.Contains("mayor a cero", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_NombreVacio_LanzaExcepcion(string name)
    {
        // Arrange
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = name
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            FavoriteMakeValidator.Validate(favorite));

        // Assert
        Assert.Contains("obligatorio", exception.Message);
    }

    [Fact]
    public void Validate_MarcaNula_LanzaExcepcion()
    {
        Assert.Throws<ArgumentNullException>(() =>
            FavoriteMakeValidator.Validate(null!));
    }
}