using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    public class GestionProveedoresController
    {
        [RoutePrefix("api/GestorProv")]
        public class GestorProveedoresController : ApiController
        {
            [HttpGet]
            [Route("api/ConsultarporID")]
            public IHttpActionResult ProvPorID(int idProveedor)
            {
                GestionProvevdores servicioAdministrador = new GestionAdministradores();
                return Ok(servicioAdministrador.BuscarAdminID(idAdmin));
            }
        }
    }
}