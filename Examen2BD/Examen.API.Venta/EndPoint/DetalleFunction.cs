using Examen.API.Venta.Contratos;
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
    public class DetalleFunction
    {
        private readonly ILogger<DetalleFunction> _logger;
        private readonly IDetalleLogic repos;

        public DetalleFunction(ILogger<DetalleFunction> logger, IDetalleLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarDetalle")]
        [OpenApiOperation("Listar", "Detalle", Description = "Lista a todas la detalles registradas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Detalle>), Description = "La lista se realizara de esta manera.")]


        public async Task<HttpResponseData> ListarDetalle([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
                var listarper = repos.Listar();
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

        [Function("ByIdDetalle")]
        [OpenApiOperation("obtenerbyId", "Detalle", Description = "Lista a todas la detalles registradas por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "Id Detalle", Description = "Ingrese Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", bodyType: typeof(Detalle), Description = "Se mostra de esta manera")]

        public async Task<HttpResponseData> ByIdDetalle([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerbyId/{id}")] HttpRequestData req, int id)
        {
            try
            {
                var res = repos.ObtenerbyId(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(res.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var resul = req.CreateResponse(HttpStatusCode.InternalServerError);
                await resul.WriteAsJsonAsync(e.Message);
                return resul;
            }
        }

        [Function("CreateDetalle")]
        [OpenApiOperation("InsertarDetalle", "Detalle", Description = "Crear nueva Detalle con sus datos")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Detalle), Description = "Inserte los datos de Detalle")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Detalle), Description = "Insertará la Detalle")]

        public async Task<HttpResponseData> CreateDetalle([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                var per = await req.ReadFromJsonAsync<Detalle>() ?? throw new Exception("Debe ingresar una detalle con todos sus datos.");
                bool Guardando = await repos.Insertar(per);
                if (Guardando)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("UpdateDetalle")]
        [OpenApiOperation("modificarDetalle", "Detalle", Description = "Se modificara a la detalle con su respectivo id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Id Detalle", Description = "Ingrese Id de la detalle")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Detalle), Description = "Inserte los datos de Detalle a Modificar")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Detalle), Description = "Debe insertar a este modelo.")]
        public async Task<HttpResponseData> UpdateDetalle([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarDetalle/{id}")] HttpRequestData req, int id)
        {
            try
            {
                var pers = await req.ReadFromJsonAsync<Detalle>() ?? throw new Exception("Debe Ingresar los datos de detalle.");
                bool guardando = await repos.Actualizar(pers, id);
                if (guardando)
                {
                    var resultado = req.CreateResponse(HttpStatusCode.OK);
                    return resultado;
                }
                else
                {
                    var resultado = req.CreateResponse(HttpStatusCode.BadRequest);
                    return resultado;
                }

            }
            catch (Exception e)
            {
                var res = req.CreateResponse(HttpStatusCode.InternalServerError);
                await res.WriteAsJsonAsync(e.Message);
                return res;
            }
        }

        [Function("DeleteDetalle")]
        [OpenApiOperation("eliminarDetalle", "Detalle", Description = "Se eliminara detalle con su id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Ingrese Id", Description = "Debe ingresar el id de la detalle")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Se eliminara de esta forma")]
        public async Task<HttpResponseData> DeleteDetalle([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarDetalle/{id}")] HttpRequestData req, int id)
        {
            try
            {
                bool guardo = await repos.Eliminar(id);
                if (guardo)
                {
                    var re = req.CreateResponse(HttpStatusCode.OK);
                    return re;
                }
                else
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                var re = req.CreateResponse(HttpStatusCode.InternalServerError);
                await re.WriteAsJsonAsync(e.Message);
                return re;
            }
        }
    }
}
