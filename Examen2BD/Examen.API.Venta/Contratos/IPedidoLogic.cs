using Examen.API.Venta.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Contratos
{
    public interface IPedidoLogic
    {

        public Task<List<Pedido>> Listar();
        public Task<Pedido> ObtenerbyId(int id);
        public Task<bool> Insertar(Pedido pedido);
        public Task<bool> Actualizar(Pedido pedido, int id);
        public Task<bool> Eliminar(int id);
    }
}
