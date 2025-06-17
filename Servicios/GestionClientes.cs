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

        public RespuestaServicio<string> Insertar()
        {
            try
            {
                dbSuper.Clientes.Add(cliente);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Cliente insertado correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al insertar el cliente: " + ex.Message);
            }
        }

        public RespuestaServicio<string> InsertarClienteUsuario(ClienteUsuario clienteUsuario )
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

                return RespuestaServicio<string>.ConExito(default, "Cliente insertado correctamente");


            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al insertar el cliente: " + ex.Message);
            }
        }

        public RespuestaServicio<string> Actualizar()
        {
            try
            {
                RespuestaServicio<Cliente> cli = Consultar(cliente.IdCliente);
                if (cli.Data == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: El cliente con el Id ingresado no existe, no se puede actualizar");
                }

                dbSuper.Clientes.AddOrUpdate(cliente);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Cliente actualizado correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al actualizar el cliente: " + ex.Message);
            }
        }

        public RespuestaServicio<string> ActualizarClienteUsuario(ClienteUsuario clienteUsuario) { 
            Cliente cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == clienteUsuario.IdCliente);
            if (cliente == null)
            {
                return RespuestaServicio<string>.ConError("Error404: El cliente con el Id ingresado no existe, no se puede actualizar");
            }
            Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.IdUsuario == cliente.IdUsuario);
            if (usuario == null)
            {
                return RespuestaServicio<string>.ConError("Error404: El usuario asociado al cliente no existe, no se puede actualizar");
            }

            try
            {
                cliente.Nombre = clienteUsuario.Nombre;
                cliente.Apellidos = clienteUsuario.Apellidos;
                cliente.Email = clienteUsuario.Email;
                cliente.Teléfono = clienteUsuario.Telefono;
                cliente.Dirección = clienteUsuario.Direccion;
                usuario.DocumentoUsuario = clienteUsuario.DocumentoUsuario;
                usuario.NombreUsuario = clienteUsuario.NombreUsuario;
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Cliente y usuario actualizados correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al actualizar el cliente y usuario: " + ex.Message);
            }

        }

        public RespuestaServicio<List<Cliente>> ConsultarTodos()
        {
            try
            {
                List<Cliente> clientes = dbSuper.Clientes
                .OrderBy(c => c.Apellidos)
                .ToList();
                return RespuestaServicio<List<Cliente>>.ConExito(clientes, "Clientes consultados correctamente");
            }
            catch (Exception ex) { 
                return RespuestaServicio<List<Cliente>>.ConError("Error al consultar los clientes: " + ex.Message);
            }
             
        }

        public RespuestaServicio<Cliente> Consultar(int idCliente)
        {
            try {
                Cliente cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
                if (cliente == null)
                {
                    return RespuestaServicio<Cliente>.ConError("Error404: El cliente con el Id ingresado no existe");
                }
                return RespuestaServicio<Cliente>.ConExito(cliente, "Cliente consultado correctamente");

            }
            catch (Exception ex)
            {
                return RespuestaServicio<Cliente>.ConError("Error al consultar el cliente: " + ex.Message);
            }
            
        }

        public RespuestaServicio<ClienteUsuario> ConsultarClienteUsuario(int id) {

            try {

                Cliente cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdCliente == id);
                if (cliente == null)
                {
                    return RespuestaServicio<ClienteUsuario>.ConError("Error404: cliente no encontrado");
                }
                Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.IdUsuario == cliente.IdUsuario);
                if (usuario == null)
                {
                    return RespuestaServicio<ClienteUsuario>.ConError("Error404: Usuario no encontrado");
                }
                ClienteUsuario cli = new ClienteUsuario
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
                return RespuestaServicio<ClienteUsuario>.ConExito(cli, "Cliente y usuario consultados correctamente");

            }
            catch(Exception ex)
            {
                return RespuestaServicio<ClienteUsuario>.ConError("Error al consultar el cliente y usuario: " + ex.Message);
            }

        }

        public RespuestaServicio<Cliente> ConsultarXCC(string cedula)
        {
            try {
                Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.DocumentoUsuario == cedula);
                if (usuario == null)
                {
                    return RespuestaServicio<Cliente>.ConError("Error404: Cliente no encontrado");
                }

                Cliente cliente = dbSuper.Clientes.FirstOrDefault(c => c.IdUsuario == usuario.IdUsuario); ;
                return RespuestaServicio<Cliente>.ConExito(cliente, "Cliente consultado correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<Cliente>.ConError("Error al consultar el cliente por cédula: " + ex.Message);
            } 
        }

        public RespuestaServicio<string> EliminarXId(int idCliente)
        {
            try
            {
                RespuestaServicio<Cliente> cli = Consultar(idCliente);
                if (cli.Data == null)
                {
                    return RespuestaServicio<string>.ConError( "El cliente con el Id ingresado no existe, no se puede eliminar");
                }

                dbSuper.Clientes.Remove(cli.Data);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"Cliente eliminado correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al eliminar el cliente: " + ex.Message);
            }
        }

        public RespuestaServicio<string> EliminarClienteUsuario(int idCliente)
        {
            try
            {
                RespuestaServicio<Cliente> cli = Consultar(idCliente);
                if (cli.Data == null)
                {
                    return RespuestaServicio<string>.ConError("El cliente con el Id ingresado no existe, no se puede eliminar");
                }

                Usuario usuario = dbSuper.Usuarios.FirstOrDefault(u => u.IdUsuario == cli.Data.IdUsuario);
                if (usuario == null)
                {
                    return RespuestaServicio<string>.ConError("El usuario asociado al cliente no existe, no se puede eliminar");
                }
                dbSuper.Clientes.Remove(cli.Data);
                dbSuper.Usuarios.Remove(usuario);
                
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"Cliente eliminado correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al eliminar el cliente: " + ex.Message);
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
