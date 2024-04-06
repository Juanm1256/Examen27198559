using Examen.API.Venta.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Contratos
{
    public interface IReportesLogic
    {
        public Task<List<Reporte2>> ListarConNombre();
        public Task<List<Reporte3>> Listar3MasVendido();
        public Task<List<Reporte3>> ListarMasVendidoConFechas(DateTime f1, DateTime f2);
    }
}
