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
    public class ClienteFunction
    {
        private readonly ILogger<ClienteFunction> _logger;
        private readonly IClienteLogic repos;

        public ClienteFunction(ILogger<ClienteFunction> logger, IClienteLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarCliente")]
        [OpenApiOperation("Listar", "Cliente", Description = "Lista a todas la clientes registradas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Cliente>), Description = "La lista se realizara de esta manera.")]


        public async Task<HttpResponseData> ListarCliente([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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

        [Function("ByIdCliente")]
        [OpenApiOperation("obtenerbyId", "Cliente", Description = "Lista a todas la clientes registradas por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "Id Cliente", Description = "Ingrese Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", bodyType: typeof(Cliente), Description = "Se mostra de esta manera")]

        public async Task<HttpResponseData> ByIdCliente([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerbyId/{id}")] HttpRequestData req, int id)
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

        [Function("CreateCliente")]
        [OpenApiOperation("InsertarCliente", "Cliente", Description = "Crear nueva Cliente con sus datos")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Cliente), Description = "Inserte los datos de Cliente")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Cliente), Description = "Insertará la Cliente")]

        public async Task<HttpResponseData> CreateCliente([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                var per = await req.ReadFromJsonAsync<Cliente>() ?? throw new Exception("Debe ingresar una cliente con todos sus datos.");
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

        [Function("UpdateCliente")]
        [OpenApiOperation("modificarCliente", "Cliente", Description = "Se modificara a la cliente con su respectivo id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Id Cliente", Description = "Ingrese Id de la cliente")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Cliente), Description = "Inserte los datos de Cliente a Modificar")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Cliente), Description = "Debe insertar a este modelo.")]
        public async Task<HttpResponseData> UpdateCliente([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarCliente/{id}")] HttpRequestData req, int id)
        {
            try
            {
                var pers = await req.ReadFromJsonAsync<Cliente>() ?? throw new Exception("Debe Ingresar los datos de cliente.");
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

        [Function("DeleteCliente")]
        [OpenApiOperation("eliminarCliente", "Cliente", Description = "Se eliminara cliente con su id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Ingrese Id", Description = "Debe ingresar el id de la cliente")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Se eliminara de esta forma")]
        public async Task<HttpResponseData> DeleteCliente([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarCliente/{id}")] HttpRequestData req, int id)
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
