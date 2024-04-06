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
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProductoLogic repos;

        public ProductoFunction(ILogger<ProductoFunction> logger, IProductoLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarProducto")]
        [OpenApiOperation("Listar", "Producto", Description = "Lista a todas la productos registradas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Producto>), Description = "La lista se realizara de esta manera.")]


        public async Task<HttpResponseData> ListarProducto([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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

        [Function("ByIdProducto")]
        [OpenApiOperation("obtenerbyId", "Producto", Description = "Lista a todas la productos registradas por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "Id Producto", Description = "Ingrese Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", bodyType: typeof(Producto), Description = "Se mostra de esta manera")]

        public async Task<HttpResponseData> ByIdProducto([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerbyId/{id}")] HttpRequestData req, int id)
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

        [Function("CreateProducto")]
        [OpenApiOperation("InsertarProducto", "Producto", Description = "Crear nueva Producto con sus datos")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Producto), Description = "Inserte los datos de Producto")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Insertará la Producto")]

        public async Task<HttpResponseData> CreateProducto([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                var per = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar una producto con todos sus datos.");
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

        [Function("UpdateProducto")]
        [OpenApiOperation("modificarProducto", "Producto", Description = "Se modificara a la producto con su respectivo id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Id Producto", Description = "Ingrese Id de la producto")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Producto), Description = "Inserte los datos de Producto a Modificar")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Debe insertar a este modelo.")]
        public async Task<HttpResponseData> UpdateProducto([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarProducto/{id}")] HttpRequestData req, int id)
        {
            try
            {
                var pers = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe Ingresar los datos de producto.");
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

        [Function("DeleteProducto")]
        [OpenApiOperation("eliminarProducto", "Producto", Description = "Se eliminara producto con su id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Type = typeof(int), Summary = "Ingrese Id", Description = "Debe ingresar el id de la producto")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Se eliminara de esta forma")]
        public async Task<HttpResponseData> DeleteProducto([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarProducto/{id}")] HttpRequestData req, int id)
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
