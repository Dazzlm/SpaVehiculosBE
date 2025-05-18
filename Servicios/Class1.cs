using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
	public class Class1
	{
		private SpaVehicularDBEntities db = new SpaVehicularDBEntities();

		public List<Usuario> GetUsuarios() {

            List<Usuario> usuarios = db.Usuarios.ToList();
            return usuarios;
        }

        public Usuario GetUsuario(int id)
        {
            Usuario usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            return usuario;
        }

        public void AddUsuario(Usuario usuario)
        {
            db.Usuarios.Add(usuario);
            db.SaveChanges();
        }

        public void UpdateUsuario(Usuario usuario)
        {
            Usuario existingUsuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
            if (existingUsuario != null)
            {
                existingUsuario.NombreUsuario = usuario.NombreUsuario;
                existingUsuario.ContraseñaHash = usuario.ContraseñaHash;
                existingUsuario.IdRol = usuario.IdRol;
                existingUsuario.Estado = usuario.Estado;
                db.SaveChanges();
            }
        }

        public void DeleteUsuario(int id)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario != null)
            {
                db.Usuarios.Remove(usuario);
                db.SaveChanges();
            }
        }

    }
}