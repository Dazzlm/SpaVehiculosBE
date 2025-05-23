using SpaVehiculosBE.Models;
using System;
using System.Linq;

namespace SpaVehiculosBE.Servicios
{
    public class Usuarios
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();

        public Usuario usuario { get; set; }

        public string CrearUsuario(Usuario usuario, int idRol)
        {
            try
            {
                var rolExiste = db.Rols.Any(r => r.IdRol == idRol);
                if (!rolExiste)
                {
                    return "El rol especificado no existe.";
                }
                Cypher cypher = new Cypher();
                cypher.Password = usuario.Clave;

                if (cypher.CifrarClave())
                {
                    usuario.Clave = cypher.PasswordCifrado;
                    usuario.salt = cypher.Salt;
                    usuario.Estado = true;
                    usuario.IdRol = idRol;

                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    return "Usuario creado con éxito";
                }
                else
                {
                    return "Error al cifrar la clave del usuario.";
                }
            }
            catch (Exception ex)
            {
                return "Error al crear el usuario: " + ex.Message;
            }
        }
    }
}
