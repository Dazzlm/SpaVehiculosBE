using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SpaVehiculosBE.Servicios
{
    public class AdminUsuario
    {
        public int IdAdmin { get; set; }
        public string Nombre { get; set; }

        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Cargo { get; set; }
        public bool Estado { get; set; }
    }
    public class GestionAdministradores
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();
        public string CrearAdmin(Administrador admin)

        {
            try
            {
                db.Administradors.Add(admin);
                db.SaveChanges();
                return "Administrador creado con exito";

            }
            catch (Exception ex)
            {
                return "Error al crear el administrador: " + ex;
            }
        }

        public string InsertarAdmin(Administrador admin)
        {
            try
            {
                db.Administradors.Add(admin);
                db.SaveChanges();
                return "Administrador insertado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar el administrador: " + ex.Message;
            }
        }

        public string InsertarAdminUsuario(AdminUsuario adminUsuario)
        {
            try
            {
                Usuario usuario = new Usuario
                {
                    NombreUsuario = adminUsuario.NombreUsuario,
                    Clave = adminUsuario.Nombre + adminUsuario.Apellidos,
                    IdRol = 1,
                    Estado = true,
                    salt = null,
                    DocumentoUsuario = adminUsuario.Cedula,
                };
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                Administrador nuevoAdmin = new Administrador
                {
                    Nombre = adminUsuario.Nombre,
                    Apellidos = adminUsuario.Apellidos,
                    Email = adminUsuario.Email,
                    Teléfono = adminUsuario.Telefono,
                    Cedula = adminUsuario.Cedula,
                    FechaNacimiento = adminUsuario.FechaNacimiento,
                    Cargo = adminUsuario.Cargo,
                    IdUsuario = usuario.IdUsuario
                };
                db.Administradors.Add(nuevoAdmin);
                db.SaveChanges();
                return "Administrador y usuario insertados correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar el administrador y usuario: " + ex.Message;
            }
        }

        public Administrador BuscarAdminID(int idAdmin)
        {
            Administrador admin = db.Administradors.FirstOrDefault(a => a.IdAdmin == idAdmin);
            return admin;
        }
        public Administrador BuscarAdminCedula(string cedula)
        {
            Administrador admin = db.Administradors.FirstOrDefault(a => a.Cedula== cedula);
            return admin;
        }
        public List<Administrador> BuscarAdminTodos()
        {
            List<Administrador> admin = db.Administradors.ToList();
            return admin;
        }
        public string EliminarAdmin(int idAdmin)
        {

            try
            {
                Administrador administrador = db.Administradors.FirstOrDefault(a => a.IdAdmin == idAdmin);
                if (administrador != null)
                {
                    db.Administradors.Remove(administrador);
                    db.SaveChanges();
                    return "El administrador ha sido eliminado correctamente";
                }
                else
                {
                    return "El administrador no se encuentra registrado";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el administrador: " + ex.Message;
            }

        }
        public string ActualizarAdmin(Administrador admin)
        {
            try
            {
                Administrador administrador = db.Administradors.FirstOrDefault(a => a.IdAdmin == admin.IdAdmin);
                if (administrador != null)
                {
                    administrador.IdAdmin = admin.IdAdmin;
                    administrador.Nombre = admin.Nombre;
                    administrador.Apellidos = admin.Apellidos;
                    administrador.Cedula = admin.Cedula;
                    administrador.Cargo = admin.Cargo;
                    administrador.Email = admin.Email;
                    administrador.FechaNacimiento = admin.FechaNacimiento;
                    administrador.Teléfono = admin.Teléfono;
                    administrador.IdUsuario = admin.IdUsuario;
                    db.SaveChanges();
                    return "El administrador ha sido actualizado exitosamente.";

                }
                else
                {
                    return "El administrador no existe.";
                }
            }
            catch (Exception ex)
            {
                return "Error al actualizar el administrador: " + ex.Message;

            }

        }
    }
}