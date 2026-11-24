using System.Net;
using System.Net.Http;
using System.Text.Json;
using Parcial1.Core.Services;
using Xunit;

namespace Parcial1.Tests.UnitTests;

public class ApiErrorMessagesTests
{
    [Fact]
    public void GetMessage_ErrorDeConexion_RetornaMensajeDeConexion()
    {
        // Arrange
        var exception = new HttpRequestException();

        // Act
        var message = ApiErrorMessages.GetMessage(
            exception, "Error inesperado");

        // Assert
        Assert.Contains("No se pudo conectar", message);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "Error 400")]
    [InlineData(HttpStatusCode.NotFound, "Error 404")]
    [InlineData(HttpStatusCode.InternalServerError, "Error 500")]
    public void GetMessage_ErrorHttp_RetornaMensajeCorrespondiente(
        HttpStatusCode statusCode,
        string expected)
    {
        // Arrange
        var exception = new HttpRequestException(
            "Error HTTP", null, statusCode);

        // Act
        var message = ApiErrorMessages.GetMessage(
            exception, "Error inesperado");

        // Assert
        Assert.Contains(expected, message);
    }

    [Fact]
    public void GetMessage_OtroCodigoHttp_RetornaCodigo()
    {
        var exception = new HttpRequestException(
            "Error HTTP", null, HttpStatusCode.Forbidden);

        var message = ApiErrorMessages.GetMessage(
            exception, "Error inesperado");

        Assert.Contains("403", message);
    }

    [Fact]
    public void GetMessage_Timeout_RetornaMensajeDeTiempo()
    {
        var exception = new TaskCanceledException();

        var message = ApiErrorMessages.GetMessage(
            exception, "Error inesperado");

        Assert.Contains("demasiado tiempo", message);
    }

    [Fact]
    public void GetMessage_JsonInvalido_RetornaMensajeJson()
    {
        var exception = new JsonException();

        var message = ApiErrorMessages.GetMessage(
            exception, "Error inesperado");

        Assert.Equal(
            "No se pudo interpretar la respuesta recibida de la API.",
            message);
    }

    [Fact]
    public void GetMessage_ErrorDesconocido_RetornaMensajeGeneral()
    {
        var exception = new Exception();

        var message = ApiErrorMessages.GetMessage(
            exception, "Error inesperado");

        Assert.Equal("Error inesperado", message);
    }
}