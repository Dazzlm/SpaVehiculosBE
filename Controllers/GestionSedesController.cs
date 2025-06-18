using ServicesClass.Clases;
using SpaVehiculosBE;
using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace ServicesClass.Clases
{
    [RoutePrefix("api/Sedes")]
    [AuthorizeSuperAdmin]

    public class SedesController : ApiController
    {

        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsultarTodos()
        {
            GestionSedes gestion = new GestionSedes();
            return validation.FormatearRespuesta(this, gestion.ConsultarTodos());
        }

        [HttpGet]
        [Route("ConsultarXId")]
        public IHttpActionResult ConsultarXId(int IdSede)
        {
            GestionSedes gestion = new GestionSedes();
            return validation.FormatearRespuesta(this, gestion.Consultar(IdSede));
        }

        [HttpPost]
        [Route("Insertar")]
        public IHttpActionResult Insertar([FromBody] Sede sede)
        {
            GestionSedes gestion = new GestionSedes();
            gestion.sede = sede;
            return validation.FormatearRespuesta(this, gestion.Insertar());
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar([FromBody] Sede sede)
        {
            GestionSedes gestion = new GestionSedes();
            gestion.sede = sede;
            return validation.FormatearRespuesta(this, gestion.Actualizar());
        }
        [HttpDelete]
        [Route("EliminarXId")]
        public IHttpActionResult EliminarXId(int IdSede)
        {
            GestionSedes gestion = new GestionSedes();
            return validation.FormatearRespuesta(this, gestion.EliminarXId(IdSede));
        }
    }
}
