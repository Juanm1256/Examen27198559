using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.DTOS
{
    public class Reporte2
    {
        public string NombreCliente { get; set; }
        public DateTime Fecha {  get; set; }
        public string NombreProducto { get; set; }
        public int Subtotal { get; set; }
    }
}
