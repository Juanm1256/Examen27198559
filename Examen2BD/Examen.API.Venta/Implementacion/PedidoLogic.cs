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

        public async Task<bool> Actualizar(Pedido pedido, Detalle detalle, int id)
        {
            bool sw = false;
            Pedido modificarPedido = await contexto.Pedidos.FirstOrDefaultAsync(x => x.IdPedido == id);

            if (modificarPedido != null)
            {
                modificarPedido.IdCliente = pedido.IdCliente;
                modificarPedido.Fecha = pedido.Fecha;
                modificarPedido.Total = pedido.Total;
                modificarPedido.Estado = pedido.Estado;
                var detallesPedido = await contexto.Detalles.Where(x => x.IdPedido == id).ToListAsync();
                foreach (var detallePedido in detallesPedido)
                {
                    detallePedido.IdProducto = detalle.IdProducto;
                    detallePedido.Cantidad = detalle.Cantidad;
                    detallePedido.Subtotal = detalle.Subtotal;
                }

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

        public async Task<bool> Insertar(Pedido pedido, Detalle detalle)
        {
            bool sw = false;
            contexto.Pedidos.Add(pedido);
            detalle.IdPedido = pedido.IdPedido;
            contexto.Detalles.Add(detalle);
            int registrosAfectados = await contexto.SaveChangesAsync();
            if (registrosAfectados > 0)
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
