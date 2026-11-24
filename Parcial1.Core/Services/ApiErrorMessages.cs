using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Parcial1.Core.Services;

public static class ApiErrorMessages
{
    public static string GetMessage(
        Exception exception,
        string unexpectedMessage)
    {
        if (exception is HttpRequestException httpException)
        {
            if (httpException.StatusCode is null)
            {
                return "No se pudo conectar con el servidor. Verifica tu conexion a Internet.";
            }

            return httpException.StatusCode switch
            {
                HttpStatusCode.BadRequest =>
                    "Error 400: la solicitud enviada no es valida.",

                HttpStatusCode.NotFound =>
                    "Error 404: no se encontro el recurso solicitado.",

                HttpStatusCode.InternalServerError =>
                    "Error 500: ocurrio un problema interno en el servidor.",

                _ =>
                    $"Error HTTP {(int?)httpException.StatusCode}: {httpException.StatusCode}."
            };
        }

        if (exception is TaskCanceledException)
        {
            return "La solicitud tardo demasiado tiempo. Intenta nuevamente.";
        }

        if (exception is JsonException)
        {
            return "No se pudo interpretar la respuesta recibida de la API.";
        }

        return unexpectedMessage;
    }
}