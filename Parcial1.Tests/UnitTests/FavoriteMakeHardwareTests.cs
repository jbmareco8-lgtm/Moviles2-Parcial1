using Moq;
using Parcial1.Core.Models;
using Parcial1.Core.Repositories;
using Parcial1.Core.Services;
using Parcial1.Core.Validators;
using Xunit;

namespace Parcial1.Tests.UnitTests;

public class FavoriteMakeHardwareTests
{
    [Theory]
    [InlineData(-34.42, -58.57)]
    [InlineData(90, 180)]
    public void ValidateLocation_CoordenadasValidas(
        double latitude,
        double longitude)
    {
        var exception = Record.Exception(() =>
            FavoriteMakeValidator.ValidateLocation(
                latitude, longitude));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData(-91, 0)]
    [InlineData(91, 0)]
    [InlineData(0, -181)]
    [InlineData(0, 181)]
    [InlineData(double.NaN, 0)]
    [InlineData(0, double.PositiveInfinity)]
    public void ValidateLocation_CoordenadasInvalidas(
        double latitude,
        double longitude)
    {
        Assert.Throws<ArgumentException>(() =>
            FavoriteMakeValidator.ValidateLocation(
                latitude, longitude));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidatePhotoPath_RutaInvalida(
        string? photoPath)
    {
        Assert.Throws<ArgumentException>(() =>
            FavoriteMakeValidator.ValidatePhotoPath(
                photoPath!));
    }

    [Fact]
    public void ValidatePhotoPath_RutaValida()
    {
        var exception = Record.Exception(() =>
            FavoriteMakeValidator.ValidatePhotoPath(
                "fotos/toyota.jpg"));

        Assert.Null(exception);
    }

    [Fact]
    public async Task UpdateLocationAsync_GuardaCoordenadas()
    {
        // Arrange
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(favorite);

        repository
            .Setup(r => r.UpdateAsync(favorite))
            .ReturnsAsync(1);

        var service = new FavoriteMakeService(repository.Object);

        // Act
        var result = await service.UpdateLocationAsync(
            1, -34.42, -58.57);

        // Assert
        Assert.NotNull(result);
        Assert.Equal<double?>(-34.42, result.Latitude);
        Assert.Equal<double?>(-58.57, result.Longitude);

        repository.Verify(
            r => r.UpdateAsync(favorite),
            Times.Once);
    }

    [Fact]
    public async Task UpdateLocationAsync_MarcaInexistente()
    {
        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((FavoriteMake?)null);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.UpdateLocationAsync(
            1, -34.42, -58.57);

        Assert.Null(result);

        repository.Verify(
            r => r.UpdateAsync(It.IsAny<FavoriteMake>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdatePhotoAsync_GuardaRutaDeFoto()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(favorite);

        repository
            .Setup(r => r.UpdateAsync(favorite))
            .ReturnsAsync(1);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.UpdatePhotoAsync(
            1, "fotos/toyota.jpg");

        Assert.NotNull(result);
        Assert.Equal("fotos/toyota.jpg", result.PhotoPath);
        Assert.True(result.HasPhoto);

        repository.Verify(
            r => r.UpdateAsync(favorite),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePhotoAsync_MarcaInexistente()
    {
        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((FavoriteMake?)null);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.UpdatePhotoAsync(
            1, "fotos/toyota.jpg");

        Assert.Null(result);

        repository.Verify(
            r => r.UpdateAsync(It.IsAny<FavoriteMake>()),
            Times.Never);
    }
}