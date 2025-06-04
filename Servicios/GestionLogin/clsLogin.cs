
using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;




namespace SpaVehiculosBE.Servicios
{ 
    public class clsLogin
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();
        public Login login { get; set; }
        public LoginRespuesta loginRespuesta { get; set; }

        public bool ValidarUsuario()
        {
            try
            {
                Cypher cypher = new Cypher();
                Usuario usuario = db.Usuarios.FirstOrDefault(u => u.NombreUsuario == login.NombreUsuario);
                if (usuario == null)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Usuario no existe";
                    return false;
                }
                byte[] arrBytesSalt = Convert.FromBase64String(usuario.salt);
                string ClaveCifrada = cypher.HashPassword(login.Clave, arrBytesSalt);
                login.Clave = ClaveCifrada;
                return true;
            }
            catch (Exception ex)
            {
                loginRespuesta.Autenticado = false;
                loginRespuesta.Mensaje = ex.Message;
                return false;
            }

        }

        private bool ValidarClave()
        {
            try
            {
                Usuario usuario = db.Usuarios.FirstOrDefault(u => u.NombreUsuario == login.NombreUsuario && u.Clave == login.Clave);
                if (usuario == null)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "La clave no coincide";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                loginRespuesta.Autenticado = false;
                loginRespuesta.Mensaje = ex.Message;
                return false;
            }
        }

        public LoginRespuesta Ingresar(Login login)
        {
            
            LoginRespuesta respuesta = new LoginRespuesta();

            try
            {
               
                Usuario usuario = db.Usuarios.Include(u => u.Rol)
                                             .FirstOrDefault(u => u.NombreUsuario == login.NombreUsuario);
                Console.WriteLine(usuario);
                if (usuario == null)
                {
                    return new LoginRespuesta
                    {
                        Autenticado = false,
                        Mensaje = "Usuario no existe"
                    };
                }

                if (usuario.IdRol != 1 || usuario.Estado == true)
                {
                    return new LoginRespuesta
                    {
                        Autenticado = false,
                        Mensaje = "Solo el SuperAdmin puede iniciar sesión"
                    };
                }

                
                Cypher cypher = new Cypher();
                byte[] arrBytesSalt = Convert.FromBase64String(usuario.salt);
                string claveHasheada = cypher.HashPassword(login.Clave, arrBytesSalt);

               
                if (usuario.Clave != claveHasheada)
                {
                    return new LoginRespuesta
                    {
                        Autenticado = false,
                        Mensaje = "Contraseña incorrecta"
                    };
                }

                string token = TokenGenerator.GenerateTokenJwt(usuario.NombreUsuario,usuario.IdRol);

                string paginaInicio;
                switch (usuario.Rol.NombreRol)
                {
                    case "SuperAdmin":
                        paginaInicio = "/superadmin/inicio";
                        break;
                    case "Administrador":
                        paginaInicio = "/admin/inicio";
                        break;
                    case "Empleado":
                        paginaInicio = "/empleado/inicio";
                        break;
                    case "Cliente":
                        paginaInicio = "/cliente/inicio";
                        break;
                    default:
                        paginaInicio = "/inicio";
                        break;
                }

                return new LoginRespuesta
                {
                    Usuario = usuario.NombreUsuario,
                    Autenticado = true,
                    Perfil = usuario.Rol.NombreRol,
                    PaginaInicio = paginaInicio,
                    Token = token,
                    Mensaje = ""
                };
            }
            catch (Exception ex)
            {
                return new LoginRespuesta
                {
                    Autenticado = false,
                    Mensaje = $"Error interno: {ex.Message}"
                };
            }
        }

    }

}
