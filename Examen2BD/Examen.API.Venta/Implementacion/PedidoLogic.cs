using Examen.API.Venta.ContextoBD;
using Examen.API.Venta.Contratos;
using Examen.API.Venta.Modelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Implementacion
{
    public class PedidoLogic : IPedidoLogic
    {
        private readonly Contexto contexto;

        public PedidoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> Actualizar(Pedido pedido, int id)
        {
            bool sw = false;
            Pedido modificar = await contexto.Pedidos.FirstOrDefaultAsync(x => x.IdPedido == id);
            if (modificar != null)
            {
                modificar.IdCliente = pedido.IdCliente;
                modificar.Fecha = pedido.Fecha;
                modificar.Total = pedido.Total;
                modificar.Estado = pedido.Estado;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool sw = false;
            Pedido eliminar = await contexto.Pedidos.FirstOrDefaultAsync(x => x.IdPedido == id);
            if (eliminar != null)
            {
                contexto.Pedidos.Remove(eliminar);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Insertar(Pedido pedido)
        {
            bool sw = false;
            contexto.Pedidos.Add(pedido);
            int test = await contexto.SaveChangesAsync();
            if (test == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Pedido>> Listar()
        {
            var lista = await contexto.Pedidos.ToListAsync();
            return lista;
        }

        public async Task<Pedido> ObtenerbyId(int id)
        {
            var lista = await contexto.Pedidos.FirstOrDefaultAsync(x=>x.IdPedido==id);
            return lista;
        }
    }
}
