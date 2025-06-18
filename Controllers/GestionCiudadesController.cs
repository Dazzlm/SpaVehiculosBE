using ServicesClass.Clases;
using SpaVehiculosBE;
using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace ServicesClass.Clases
{
    [RoutePrefix("api/Ciudades")]
    [AuthorizeSuperAdmin]

    public class GestionCiudadesController : ApiController
    {
        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsultarTodos()
        {
            GestionCiudades gestion = new GestionCiudades();
            return validation.FormatearRespuesta(this, gestion.ConsultarTodos());
        }

        [HttpGet]
        [Route("ConsultarXId")]
        public IHttpActionResult ConsultarXId(int IdCiudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            return validation.FormatearRespuesta(this, gestion.Consultar(IdCiudad));
        }

        [HttpPost]
        [Route("Insertar")]
        public IHttpActionResult Insertar([FromBody] Ciudad ciudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            gestion.ciudad = ciudad;
            return validation.FormatearRespuesta(this, gestion.Insertar());
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar([FromBody] Ciudad ciudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            gestion.ciudad = ciudad;
            return validation.FormatearRespuesta(this, gestion.Actualizar());
        }
        [HttpDelete]
        [Route("EliminarXId")]
        public IHttpActionResult EliminarXId(int IdCiudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            return validation.FormatearRespuesta(this, gestion.EliminarXId(IdCiudad));
        }
    }
}
