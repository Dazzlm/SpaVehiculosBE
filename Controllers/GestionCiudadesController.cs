using ServicesClass.Clases;
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
        [HttpGet]
        [Route("ConsultarTodos")]
        public List<Ciudad> ConsultarTodos()
        {
            GestionCiudades gestion = new GestionCiudades();
            return gestion.ConsultarTodos();
        }

        [HttpGet]
        [Route("ConsultarXId")]
        public Ciudad ConsultarXId(int IdCiudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            return gestion.Consultar(IdCiudad);
        }

        [HttpPost]
        [Route("Insertar")]
        public string Insertar([FromBody] Ciudad ciudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            gestion.ciudad = ciudad;
            return gestion.Insertar();
        }

        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Ciudad ciudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            gestion.ciudad = ciudad;
            return gestion.Actualizar();
        }
        [HttpDelete]
        [Route("EliminarXId")]
        public string EliminarXId(int IdCiudad)
        {
            GestionCiudades gestion = new GestionCiudades();
            return gestion.EliminarXId(IdCiudad);
        }
    }
}
