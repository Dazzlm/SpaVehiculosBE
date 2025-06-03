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
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
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

                Cypher cypher = new Cypher();
                cypher.Password = adminUsuario.Contrasena;

                if (!cypher.CifrarClave())
                {
                    return "Error al cifrar la clave del usuario.";
                }


                Usuario usuario = new Usuario
                {
                    NombreUsuario = adminUsuario.NombreUsuario,
                    Clave = cypher.PasswordCifrado,
                    IdRol = 1,
                    Estado = true,
                    salt = cypher.Salt,
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

        public AdminUsuario BuscarAdminUsuario(int id)
        {
            Administrador administrador = db.Administradors.FirstOrDefault(a => a.IdAdmin == id);
            if (administrador == null)
            {
                return null;
            }
            Usuario usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == administrador.IdUsuario);
            if (usuario == null)
            {
                return null; 
            }
            AdminUsuario adminUsuario = new AdminUsuario
            {
                IdAdmin = administrador.IdAdmin,
                Nombre = administrador.Nombre,
                Apellidos = administrador.Apellidos,
                Email = administrador.Email,
                Telefono = administrador.Teléfono,
                Cedula = administrador.Cedula,
                NombreUsuario = usuario.NombreUsuario,
                FechaNacimiento = administrador.FechaNacimiento,
                Cargo = administrador.Cargo,
            };
            return adminUsuario;
        }

        public Administrador BuscarAdminID(int idAdmin)
        {
            Administrador admin = db.Administradors.FirstOrDefault(a => a.IdAdmin == idAdmin);
            return admin;
        }
        public Administrador BuscarAdminCedula(string cedula)
        {
            Administrador admin = db.Administradors.FirstOrDefault(a => a.Cedula == cedula);
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

        public string ActualizarAdminUsuario(AdminUsuario adminUsuario)
        {
            try
            {
                Administrador administrador = db.Administradors.FirstOrDefault(a => a.IdAdmin == adminUsuario.IdAdmin);
                if (administrador == null)
                {
                    return "El administrador no existe.";
                }
                Usuario usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == administrador.IdUsuario);
                if (usuario == null)
                {
                    return "El usuario asociado al administrador no existe.";
                }
                // Actualizar los datos del administrador
                administrador.Nombre = adminUsuario.Nombre;
                administrador.Apellidos = adminUsuario.Apellidos;
                administrador.Email = adminUsuario.Email;
                administrador.Teléfono = adminUsuario.Telefono;
                administrador.Cedula = adminUsuario.Cedula;
                administrador.FechaNacimiento = adminUsuario.FechaNacimiento;
                administrador.Cargo = adminUsuario.Cargo;
                usuario.NombreUsuario = adminUsuario.NombreUsuario; 
                usuario.DocumentoUsuario = adminUsuario.Cedula;
                usuario.Estado = adminUsuario.Estado;
                db.SaveChanges();
                return "Administrador y usuario actualizados correctamente.";
            }
            catch (Exception ex)
            {
                return "Error al actualizar el administrador y usuario: " + ex.Message;
            }
        }

        public string EliminarAdminUsuario(int id) {
            try
            {
                Administrador administrador = db.Administradors.FirstOrDefault(a => a.IdAdmin == id);
                if (administrador != null)
                {
                    Usuario usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == administrador.IdUsuario);
                    if (usuario == null)
                    {
                        return "El usuario asociado al administrador no existe.";
                    }
                    
                    db.Administradors.Remove(administrador);
                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                    return "El administrador y su usuario han sido eliminados correctamente.";
                }
                else
                {
                    return "El administrador no se encuentra registrado.";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el administrador y usuario: " + ex.Message;
            }

        }
    }
}