using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        [HttpGet]
        [Route("ConsultarUsuarios")]
        public IHttpActionResult GetUsuarios()
        {
            using (var db = new SpaVehicularDBEntities())
            {
                var usuarios = db.Usuarios.ToList();
                return Ok(usuarios);
            }
        }

        [HttpGet]
        [Route("Usuario/{id}")]
        public IHttpActionResult GetUsuario(int id)
        {
            using (var db = new SpaVehicularDBEntities())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario == null)
                {
                    return NotFound();
                }
                return Ok(usuario);
            }
        }

        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult AddUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Invalid data.");
            }
            using (var db = new SpaVehicularDBEntities())
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = usuario.IdUsuario }, usuario);
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IHttpActionResult UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null || usuario.IdUsuario != id)
            {
                return BadRequest("Invalid data.");
            }
            using (var db = new SpaVehicularDBEntities())
            {
                var existingUsuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (existingUsuario == null)
                {
                    return NotFound();
                }
                existingUsuario.NombreUsuario = usuario.NombreUsuario;
                existingUsuario.ContraseñaHash = usuario.ContraseñaHash;
                existingUsuario.IdRol = usuario.IdRol;
                existingUsuario.Estado = usuario.Estado;
                db.SaveChanges();
                return Ok(existingUsuario);
            }
        }
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IHttpActionResult DeleteUsuario(int id)
        {
            using (var db = new SpaVehicularDBEntities())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario == null)
                {
                    return NotFound();
                }
                db.Usuarios.Remove(usuario);
                db.SaveChanges();
                return Ok(usuario);
            }

        }
    }
}