using ServicesClass.Clases;
using SpaVehiculosBE;
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

        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsultarTodos()
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.ConsultarTodos());
        }

        [HttpGet]
        [Route("ConsultarXId")]
        public IHttpActionResult ConsultarXId(int IdCliente)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.Consultar(IdCliente));
        }

        [HttpGet]

        [Route("ConsultarXCC")]
        public IHttpActionResult ConsultarXCC(string CC)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.ConsultarXCC(CC));
        }

        [HttpGet]
        [Route("ConsultarClienteUsuario")]
        public IHttpActionResult ConsultarClienteUsuario(int id)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.ConsultarClienteUsuario(id));
        }


        [HttpPost]
        [Route("Insertar")]
        public IHttpActionResult Insertar([FromBody] Cliente cliente)
        {
            GestionClientes gestion = new GestionClientes();
            gestion.cliente = cliente;
            return validation.FormatearRespuesta(this, gestion.Insertar());
        }

        [HttpPost]
        [Route("InsertarClienteUsuario")]
        public IHttpActionResult InsertarClienteUsuario([FromBody] ClienteUsuario clienteUsuario)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.InsertarClienteUsuario(clienteUsuario));
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar([FromBody] Cliente cliente)
        {
            GestionClientes gestion = new GestionClientes();
            gestion.cliente = cliente;
            return validation.FormatearRespuesta(this, gestion.Actualizar());
        }

        [HttpPut]
        [Route("ActualizarClienteUsuario")]
        public IHttpActionResult ActualizarClienteUsuario([FromBody] ClienteUsuario cliente)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.ActualizarClienteUsuario(cliente));
        }

        [HttpDelete]
        [Route("EliminarXId")]
        public IHttpActionResult EliminarXId(int IdCliente)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.EliminarXId(IdCliente));
        }

        [HttpDelete]
        [Route("EliminarClienteUsuario")]
        public IHttpActionResult EliminarClienteUsuario(int idCliente)
        {
            GestionClientes gestion = new GestionClientes();
            return validation.FormatearRespuesta(this, gestion.EliminarClienteUsuario(idCliente));
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
