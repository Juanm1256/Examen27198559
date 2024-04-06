using Examen.API.Venta.Contratos;
using Examen.API.Venta.DTOS;
using Examen.API.Venta.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Examen.API.Venta.EndPoint
{
    public class ReportesFunction
    {
        private readonly ILogger<ReportesFunction> _logger;
        private readonly IReportesLogic repos;

        public ReportesFunction(ILogger<ReportesFunction> logger, IReportesLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarConNombre")]
        [OpenApiOperation("ListarConNombre", "Reportes", Description = "Lista a todas la detalles registradas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Reporte2>), Description = "La lista se realizara de esta manera.")]

        public async Task<HttpResponseData> ListarConNombre([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
                var listarper = repos.ListarConNombre();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listarper.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("Listar3masVendido")]
        [OpenApiOperation("Listar3masvendido", "Reportes", Description = "Lista a todas la detalles registradas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Reporte3>), Description = "La lista se realizara de esta manera.")]


        public async Task<HttpResponseData> Listar3masVendido([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
                var listarper = repos.Listar3MasVendido();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listarper.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("Listar3masVendidoRango")]
        [OpenApiOperation("Listar3masvendidorango", "Reportes", Description = "Lista a todas la detalles registradas")]
        [OpenApiParameter("FechaInicio", In = ParameterLocation.Path, Type = typeof(DateTime))]
        [OpenApiParameter("FechaFinal", In = ParameterLocation.Path, Type = typeof(DateTime))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Reporte3>), Description = "La lista se realizara de esta manera.")]


        public async Task<HttpResponseData> Listar3masVendidoRango([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Listar3masVendidoRango/{FechaInicio},{FechaFinal}")] HttpRequestData req, DateTime FechaInicio, DateTime FechaFinal)
        {
            try
            {
                var listarper = repos.ListarMasVendidoConFechas(FechaInicio, FechaFinal);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listarper.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
    }
}
