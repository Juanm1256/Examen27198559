using Examen.API.Venta.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Contratos
{
    public interface IProductoLogic
    {
        public Task<List<Producto>> Listar();
        public Task<Producto> ObtenerbyId(int id);
        public Task<bool> Insertar(Producto producto);
        public Task<bool> Actualizar(Producto producto, int id);
        public Task<bool> Eliminar(int id);
    }
}
