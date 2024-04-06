using Examen.API.Venta.ContextoBD;
using Examen.API.Venta.Contratos;
using Examen.API.Venta.DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Implementacion
{
    public class ReportLogic : IReportesLogic
    {
        private readonly Contexto contexto;

        public ReportLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<List<Reporte2>> ListarConNombre()
        {
            var lista = await contexto.Detalles.Select(x => new Reporte2
            {
                NombreCliente = x.IdPedidoNav.IdClienteNav.Nombre,
                Fecha = x.IdPedidoNav.Fecha,
                NombreProducto = x.IdProductoNav.Nombre,
                Subtotal = x.Subtotal
            }).ToListAsync();
            return lista;
        }

        public async Task<List<Reporte3>> Listar3MasVendido()
        {
            var resultado = await contexto.Detalles
                .GroupBy(d => new { d.IdProducto, d.IdProductoNav.Nombre })
                .Select(g => new Reporte3
                {
                    IdProducto = g.Key.IdProducto,
                    NombreProducto = g.Key.Nombre,
                    Cantidad = g.Sum(d => d.Cantidad)
                })
                .OrderByDescending(p => p.Cantidad)
                .Take(3)
                .ToListAsync();

            return resultado;
        }

        public async Task<List<Reporte3>> ListarMasVendidoConFechas(DateTime f1, DateTime f2)
        {
            var resultado = contexto.Detalles
                        .Where(x => x.IdPedidoNav.Fecha >= f1 && x.IdPedidoNav.Fecha <= f2)
                        .GroupBy(d => new { d.IdProducto, d.IdProductoNav.Nombre })
                        .Select(g => new Reporte3
                        {
                            IdProducto = g.Key.IdProducto,
                            NombreProducto = g.Key.Nombre,
                            Cantidad = g.Sum(d => d.Cantidad)
                        })
                        .OrderByDescending(p => p.Cantidad)
                        .ToListAsync();

            return resultado.Result;
        }
    }
}
