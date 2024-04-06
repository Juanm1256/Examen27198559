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
    public class ClienteLogic : IClienteLogic
    {
        private readonly Contexto contexto;

        public ClienteLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> Actualizar(Cliente cliente, int id)
        {
            bool sw = false;
            Cliente modificar = await contexto.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (modificar != null)
            {
                modificar.Nombre = cliente.Nombre;
                modificar.Apellido = cliente.Apellido;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool sw = false;
            Cliente clienteAEliminar = await contexto.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (clienteAEliminar != null)
            {
                var pedidosDelCliente = await contexto.Pedidos.Where(p => p.IdCliente == id).ToListAsync();
                foreach (var pedido in pedidosDelCliente)
                {
                    var detallesDelPedido = await contexto.Detalles.Where(d => d.IdPedido == pedido.IdPedido).ToListAsync();
                    contexto.Detalles.RemoveRange(detallesDelPedido);
                }
                contexto.Pedidos.RemoveRange(pedidosDelCliente);
                contexto.Clientes.Remove(clienteAEliminar);
                await contexto.SaveChangesAsync();

                sw = true;
            }

            return sw;
        }

        public async Task<bool> Insertar(Cliente cliente)
        {
            bool sw = false;
            contexto.Clientes.Add(cliente);
            int test = await contexto.SaveChangesAsync();
            if (test == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Cliente>> Listar()
        {

            var lista = await contexto.Clientes.ToListAsync();
            return lista;
        }

        public async Task<Cliente> ObtenerbyId(int id)
        {

            var lista = await contexto.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            return lista;
        }
    }
}
