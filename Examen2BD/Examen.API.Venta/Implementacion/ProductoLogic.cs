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
    public class ProductoLogic : IProductoLogic
    {
        private readonly Contexto contexto;

        public ProductoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> Actualizar(Producto producto, int id)
        {
            bool sw = false;
            Producto modificar = await contexto.Productos.FirstOrDefaultAsync(x => x.IdProducto== id);
            if (modificar != null)
            {
                modificar.Nombre = producto.Nombre;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool sw = false;
            Producto eliminar = await contexto.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);
            if (eliminar != null)
            {
                contexto.Productos.Remove(eliminar);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> Insertar(Producto producto)
        {
            bool sw = false;
            contexto.Productos.Add(producto);
            int test = await contexto.SaveChangesAsync();
            if (test == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Producto>> Listar()
        {
            var lista = await contexto.Productos.ToListAsync();
            return lista;
        }

        public async Task<Producto> ObtenerbyId(int id)
        {
            var lista = await contexto.Productos.FirstOrDefaultAsync(x=>x.IdProducto==id);
            return lista;
        }
    }
}
