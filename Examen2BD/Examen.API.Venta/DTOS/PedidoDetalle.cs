using Examen.API.Venta.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.DTOS
{
    public class PedidoDetalle
    {
        public Pedido pedido { get; set; }
        public Detalle detalle { get; set; }
    }
}
