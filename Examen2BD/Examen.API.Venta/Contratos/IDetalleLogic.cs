using Examen.API.Venta.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Contratos
{
    public interface IDetalleLogic
    {

        public Task<List<Detalle>> Listar();
        public Task<Detalle> ObtenerbyId(int id);
        public Task<bool> Insertar(Detalle detalle);
        public Task<bool> Actualizar(Detalle detalle, int id);
        public Task<bool> Eliminar(int id);
    }
}
