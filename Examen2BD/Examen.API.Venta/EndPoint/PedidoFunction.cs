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
    public class PedidoFunction
    {
        private readonly ILogger<PedidoFunction> _logger;
        private readonly IPedidoLogic repos;

        public PedidoFunction(ILogger<PedidoFunction> logger, IPedidoLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarPedido")]
        [OpenApiOperation("Listar", "Pedido", Description = "Lista a todas la pedidos registradas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pedido>), Description = "La lista se realizara de esta manera.")]


        public async Task<HttpResponseData> ListarPedido([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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

        [Function("ByIdPedido")]
        [OpenApiOperation("obtenerbyId", "Pedido", Description = "Lista a todas la pedidos registradas por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "Id Pedido", Description = "Ingrese Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", bodyType: typeof(Pedido), Description = "Se mostra de esta manera")]

        public async Task<HttpResponseData> ByIdPedido([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerbyId/{id}")] HttpRequestData req, int id)
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

        [Function("CreatePedido")]
        [OpenApiOperation("InsertarPedido", "Pedido", Description = "Crear nueva Pedido con sus datos")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PedidoDetalle), Description = "Inserte los datos de Pedido")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pedido), Description = "Insertará la Pedido")]

        public async Task<HttpResponseData> CreatePedido([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                var per = await req.ReadFromJsonAsync<PedidoDetalle>() ?? throw new Exception("Debe ingresar una pedido con todos sus datos.");
                bool Guardando = await repos.Insertar(per.pedido, per.detalle);
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

        [Function("UpdatePedido")]
        [OpenApiOperation("modificarPedido", "Pedido", Description = "Se modificara a la pedido con su respectivo id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Id Pedido", Description = "Ingrese Id de la pedido")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PedidoDetalle), Description = "Inserte los datos de Pedido")]

        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pedido), Description = "Debe insertar a este modelo.")]
        public async Task<HttpResponseData> UpdatePedido([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarPedido/{id}")] HttpRequestData req, int id)
        {
            try
            {
                var pers = await req.ReadFromJsonAsync<PedidoDetalle>() ?? throw new Exception("Debe Ingresar los datos de pedido.");
                bool guardando = await repos.Actualizar(pers.pedido, pers.detalle, id);
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

        [Function("DeletePedido")]
        [OpenApiOperation("eliminarPedido", "Pedido", Description = "Se eliminara pedido con su id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Ingrese Id", Description = "Debe ingresar el id de la pedido")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Se eliminara de esta forma")]
        public async Task<HttpResponseData> DeletePedido([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarPedido/{id}")] HttpRequestData req, int id)
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
