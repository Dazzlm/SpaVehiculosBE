using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ServicesClass.Clases
{

    public class ClienteUsuario {

        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DocumentoUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        
    }

    public class GestionClientes
    {
        private SpaVehicularDBEntities dbSuper = new SpaVehicularDBEntities();
        public Cliente cliente { get; set; }

        public string Insertar()
        {
            try
            {
                dbSuper.Clientes.Add(cliente);
                dbSuper.SaveChanges();
                return "Cliente insertado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar el cliente: " + ex.Message;
            }
        }

        public string InsertarClienteUsuario(ClienteUsuario clienteUsuario )
        {
            try
            {
                Usuario usuario = new Usuario
                {
                    NombreUsuario = clienteUsuario.NombreUsuario,
                    Clave = clienteUsuario.Nombre + clienteUsuario.Apellidos, 
                    IdRol = 3,
                    Estado = true,
                    salt = null,
                    DocumentoUsuario = clienteUsuario.DocumentoUsuario,
                };
                dbSuper.Usuarios.Add(usuario);
                dbSuper.SaveChanges();
                Cliente nuevoCliente = new Cliente
                {
                    Nombre = clienteUsuario.Nombre,
                    Apellidos = clienteUsuario.Apellidos,
                    Email = clienteUsuario.Email,
                    Teléfono = clienteUsuario.Telefono,
                    Dirección = clienteUsuario.Direccion,
                    IdUsuario = usuario.IdUsuario,
                    Imagen = null 
                };
                dbSuper.Clientes.Add(nuevoCliente);
                dbSuper.SaveChanges();

                return "Cliente insertado correctamente";


            }
            catch (Exception ex)
            {
                return "Error al insertar el cliente: " + ex.Message;
            }
        }

        public string Actualizar()
        {
            try
            {
                Cliente cli = Consultar(cliente.IdCliente);
                if (cli == null)
                {
                    return "El cliente con el Id ingresado no existe, no se puede actualizar";
                }

                dbSuper.Clientes.AddOrUpdate(cliente);
                dbSuper.SaveChanges();
                return "Cliente actualizado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al actualizar el cliente: " + ex.Message;
            }
        }

        public string ActualizarClienteUsuario(int id) { 
            Cliente cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == id);
            if (cliente == null)
            {
                return "El cliente con el Id ingresado no existe, no se puede actualizar";
            }
            Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.IdUsuario == cliente.IdUsuario);
            if (usuario == null)
            {
                return "El usuario asociado al cliente no existe, no se puede actualizar";
            }

            try
            {
                cliente.Nombre = cliente.Nombre;
                cliente.Apellidos = cliente.Apellidos;
                cliente.Email = cliente.Email;
                cliente.Teléfono = cliente.Teléfono;
                cliente.Dirección = cliente.Dirección;
                usuario.DocumentoUsuario = usuario.DocumentoUsuario;
                usuario.NombreUsuario = usuario.NombreUsuario;
                dbSuper.SaveChanges();
                return "Cliente y usuario actualizados correctamente";
            }
            catch (Exception ex)
            {
                return "Error al actualizar el cliente y usuario: " + ex.Message;
            }


        }

        public List<Cliente> ConsultarTodos()
        {
            return dbSuper.Clientes
                .OrderBy(c => c.Apellidos)
                .ToList();
        }

        public Cliente Consultar(int idCliente)
        {
            return dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
        }

        public ClienteUsuario ConsultarClienteUsuario(int id) {
            Cliente cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == id);
            if (cliente == null)
            {
                return null;
            }
            Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.IdUsuario == cliente.IdUsuario);
            if (usuario == null)
            {
                return null;
            }
            return new ClienteUsuario
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                DocumentoUsuario = usuario.DocumentoUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Email = cliente.Email,
                Telefono = cliente.Teléfono,
                Direccion = cliente.Dirección
            };
        }

        public Cliente ConsultarXCC(string cedula)
        {
            Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.DocumentoUsuario == cedula);
            if (usuario == null)
            {
                return null;
            }

            return dbSuper.Clientes.FirstOrDefault(c => c.IdUsuario == usuario.IdUsuario);
        }

        public string EliminarXId(int idCliente)
        {
            try
            {
                Cliente cli = Consultar(idCliente);
                if (cli == null)
                {
                    return "El cliente con el Id ingresado no existe, no se puede eliminar";
                }

                dbSuper.Clientes.Remove(cli);
                dbSuper.SaveChanges();
                return "Cliente eliminado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al eliminar el cliente: " + ex.Message;
            }
        }
        public string GrabarImagenCliente(int idCliente, List<string> imagenes)
        {
            try
            {
                var cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
                if (cliente == null)
                {
                    return "No se encontró el cliente con Id " + idCliente;
                }
                cliente.Imagen = imagenes.FirstOrDefault();
                dbSuper.SaveChanges();

                return "Imagen guardada correctamente para el cliente.";
            }
            catch (Exception ex)
            {
                return "Error al guardar la imagen: " + ex.Message;
            }
        }
        public bool ClienteExiste(int idCliente)
        {
            return Consultar(idCliente) != null;
        }
        public string clsUploadCliente(int idCliente, List<string> imagenes)
        {
            GestionClientes gestionClientes = new GestionClientes();

            if (!gestionClientes.ClienteExiste(idCliente))
            {
                return "El cliente no existe";
            }

            string resultadoGuardarImagen = gestionClientes.GrabarImagenCliente(idCliente, imagenes);

            return resultadoGuardarImagen;
        }

        public string EliminarImagenCliente(string nombreArchivo)
        {
            try
            {
                var cliente = dbSuper.Clientes.FirstOrDefault(c => c.Imagen == nombreArchivo);
                if (cliente == null)
                {
                    return "No se encontró un cliente con esa imagen.";
                }

                cliente.Imagen = null;
                dbSuper.SaveChanges();

                return "Imagen eliminada correctamente de la base de datos.";
            }
            catch (Exception ex)
            {
                return "Error al eliminar la imagen: " + ex.Message;
            }
        }
        public string ObtenerImagenPorCliente(int idCliente)
        {
            var cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
            if (cliente == null)
                return null;
            return cliente.Imagen;
        }
        public int ContarClientes()
        {
            try
            {
                return dbSuper.Clientes.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
