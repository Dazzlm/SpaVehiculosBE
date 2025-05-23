using SpaVehiculosBE.Models;
using System;

namespace SpaVehiculosBE.Servicios
{
    public class Usuarios
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();

        public Usuario usuario { get; set; }

        public string CrearUsuario(int idRol)
        {
            try
            {
                Cypher cypher = new Cypher();
                //cyper.Password = usuario.Contrase�a;
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return "Usuario creado con exito";
            }
            catch (Exception ex)
            {
                return "Error al crear el usuario: " + ex.Message;
            }
        }
    }
}
