using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Modelo
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
        public string? Estado { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente IdClienteNav { get; set; }
    }
}
