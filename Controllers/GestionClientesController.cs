using ServicesClass.Clases;
using SpaVehiculosBE.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace ServicesClass.Clases
{
    [RoutePrefix("api/Clientes")]
    public class ClientesController : ApiController
    {
        [HttpGet]
        [Route("ConsultarTodos")]
        public List<Cliente> ConsultarTodos()
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.ConsultarTodos();
        }

        [HttpGet]
        [Route("ConsultarXId")]
        public Cliente ConsultarXId(int IdCliente)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.Consultar(IdCliente);
        }

        [HttpGet]

        [Route("ConsultarXCC")]
        public Cliente ConsultarXCC(string CC)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.ConsultarXCC(CC);
        }


        [HttpPost]
        [Route("Insertar")]
        public string Insertar([FromBody] Cliente cliente)
        {
            GestionClientes gestion = new GestionClientes();
            gestion.cliente = cliente;
            return gestion.Insertar();
        }

        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Cliente cliente)
        {
            GestionClientes gestion = new GestionClientes();
            gestion.cliente = cliente;
            return gestion.Actualizar();
        }

        [HttpDelete]
        [Route("EliminarXId")]
        public string EliminarXId(int IdCliente)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.EliminarXId(IdCliente);
        }
    }
}
