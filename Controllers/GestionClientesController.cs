using ServicesClass.Clases;
using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace ServicesClass.Clases
{
    [RoutePrefix("api/Clientes")]
    [AuthorizeSuperAdmin]

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

        [HttpGet]
        [Route("ConsultarClienteUsuario")]
        public ClienteUsuario ConsultarClienteUsuario(int id)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.ConsultarClienteUsuario(id);
        }


        [HttpPost]
        [Route("Insertar")]
        public string Insertar([FromBody] Cliente cliente)
        {
            GestionClientes gestion = new GestionClientes();
            gestion.cliente = cliente;
            return gestion.Insertar();
        }

        [HttpPost]
        [Route("InsertarClienteUsuario")]
        public string InsertarClienteUsuario([FromBody] ClienteUsuario clienteUsuario)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.InsertarClienteUsuario(clienteUsuario);
        }

        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Cliente cliente)
        {
            GestionClientes gestion = new GestionClientes();
            gestion.cliente = cliente;
            return gestion.Actualizar();
        }

        [HttpPut]
        [Route("ActualizarClienteUsuario")]
        public string ActualizarClienteUsuario([FromBody] int id)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.ActualizarClienteUsuario(id);
        }

        [HttpDelete]
        [Route("EliminarXId")]
        public string EliminarXId(int IdCliente)
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.EliminarXId(IdCliente);
        }
        [HttpGet]
        [Route("Contar")]
        public int ContarClientes()
        {
            GestionClientes gestion = new GestionClientes();
            return gestion.ContarClientes();
        }

    }
}
