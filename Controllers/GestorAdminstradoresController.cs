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
        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("ConsultarPorID")]
        public IHttpActionResult AdminPorID(int idAdmin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.BuscarAdminID(idAdmin));
        }
        [HttpGet]
        [Route("ConsultarPorCedula")]
        public IHttpActionResult AdminPorCedula(string Cedula)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.BuscarAdminCedula(Cedula));
        }
        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsutarTodos()
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.BuscarAdminTodos());
        }

        [HttpGet]
        [Route("ConsultarAdminUsuario")]
        public IHttpActionResult ConsultarAdminUsuario(int id)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.BuscarAdminUsuario(id));
        }

        [HttpDelete]
        [Route("EliminarAdmin")]
        public IHttpActionResult BorrarAdmin(int idAdmin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.EliminarAdmin(idAdmin));

        }

        [HttpDelete]
        [Route("EliminarAdminUsuario")]
        public IHttpActionResult BorrarAdminUsuario(int idAdmin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.EliminarAdminUsuario(idAdmin));
        }

        [HttpPut]
        [Route("ActualizarAdmin")]
        public IHttpActionResult ActualizarAdmin(Administrador admin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.ActualizarAdmin(admin));

        }
        [HttpPut]
        [Route("ActualizarAdminUsuario")]
        public IHttpActionResult ActualizarAdminUsuario(AdminUsuario adminUsuario)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.ActualizarAdminUsuario(adminUsuario));
        }

        [HttpPost]
        [Route("CrearAdmin")]
        public IHttpActionResult CrearAdmin([FromBody] Administrador admin)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.CrearAdmin(admin));

        }
        [HttpPost]
        [Route("InsertarAdminUsuario")]
        public IHttpActionResult InsertarAdminUsuario([FromBody] AdminUsuario adminUsuario)
        {
            GestionAdministradores servicioAdministrador = new GestionAdministradores();
            return validation.FormatearRespuesta(this, servicioAdministrador.InsertarAdminUsuario(adminUsuario));

        }
    }
}