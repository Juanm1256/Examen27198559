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
    public class DetalleLogic : IDetalleLogic
    {
        private readonly Contexto contexto;

        public DetalleLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> Actualizar(Detalle detalle, int id)
        {
            bool sw = false;
            Detalle modificar = await contexto.Detalles.FirstOrDefaultAsync(x => x.IdPedido == id);
            if (modificar != null)
            {
                modificar.IdProducto = detalle.IdProducto;
                modificar.Cantidad = detalle.Cantidad;
                modificar.Precio = detalle.Precio;
                modificar.Subtotal = detalle.Subtotal;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool sw = false;
            Detalle eliminar = await contexto.Detalles.FirstOrDefaultAsync(x => x.IdPedido == id);
            if (eliminar != null)
            {
                contexto.Detalles.Remove(eliminar);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Insertar(Detalle detalle)
        {
            bool sw = false;
            contexto.Detalles.Add(detalle);
            int test = await contexto.SaveChangesAsync();
            if (test == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Detalle>> Listar()
        {
            var lista = await contexto.Detalles.ToListAsync();
            return lista;
        }

        public async Task<Detalle> ObtenerbyId(int id)
        {
            var lista = await contexto.Detalles.FirstOrDefaultAsync(x=>x.IdPedido == id);
            return lista;
        }
    }
}
