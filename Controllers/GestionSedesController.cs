using ServicesClass.Clases;
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
        [HttpGet]
        [Route("ConsultarTodos")]
        public List<Sede> ConsultarTodos()
        {
            GestionSedes gestion = new GestionSedes();
            return gestion.ConsultarTodos();
        }

        [HttpGet]
        [Route("ConsultarXId")]
        public Sede ConsultarXId(int IdSede)
        {
            GestionSedes gestion = new GestionSedes();
            return gestion.Consultar(IdSede);
        }

        [HttpPost]
        [Route("Insertar")]
        public string Insertar([FromBody] Sede sede)
        {
            GestionSedes gestion = new GestionSedes();
            gestion.sede = sede;
            return gestion.Insertar();
        }

        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Sede sede)
        {
            GestionSedes gestion = new GestionSedes();
            gestion.sede = sede;
            return gestion.Actualizar();
        }
        [HttpDelete]
        [Route("EliminarXId")]
        public string EliminarXId(int IdSede)
        {
            GestionSedes gestion = new GestionSedes();
            return gestion.EliminarXId(IdSede);
        }
    }
}
