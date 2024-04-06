using Examen.API.Venta.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.API.Venta.Contratos
{
    public interface IClienteLogic
    {

        public Task<List<Cliente>> Listar();
        public Task<Cliente> ObtenerbyId(int id);
        public Task<bool> Insertar(Cliente cliente);
        public Task<bool> Actualizar(Cliente cliente, int id);
        public Task<bool> Eliminar(int id);
    }
}
