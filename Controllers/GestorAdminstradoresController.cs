using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/GestorAdmin")]
    [AuthorizeSuperAdmin]

    public class GestorAdminstradoresController : ApiController
    {
        [HttpGet]
        [Route("ConsultarPorID")]
        public IHttpActionResult AdminPorID(int idAdmin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.BuscarAdminID(idAdmin));
        }
        [HttpGet]
        [Route("ConsultarPorCedula")]
        public IHttpActionResult AdminPorCedula(string Cedula)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.BuscarAdminCedula(Cedula));
        }
        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsutarTodos()
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.BuscarAdminTodos());
        }
        [HttpDelete]
        [Route("EliminarAdmin")]
        public IHttpActionResult BorrarAdmin(int idAdmin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.EliminarAdmin(idAdmin));

        }
        [HttpPut]
        [Route("ActualizarAdmin")]
        public IHttpActionResult ActualizarAdmin(Administrador admin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.ActualizarAdmin(admin));

        }
        [HttpPost]
        [Route("CrearAdmin")]
        public IHttpActionResult CrearAdmin([FromBody] Administrador admin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.CrearAdmin(admin));

        }
        [HttpPost]
        [Route("InsertarAdminUsuario")]
        public IHttpActionResult InsertarAdminUsuario([FromBody] AdminUsuario adminUsuario)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return Ok(servicioAdministrador.InsertarAdminUsuario(adminUsuario));

        }
    }
}