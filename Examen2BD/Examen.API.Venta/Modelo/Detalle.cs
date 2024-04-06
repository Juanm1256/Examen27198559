using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Modelo
{
    public class Detalle
    {
        [Key]
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }
        public int Subtotal { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto IdProductoNav { get; set; }

        [ForeignKey("IdPedido")]
        public virtual Pedido IdPedidoNav { get; set; }
    }
}
