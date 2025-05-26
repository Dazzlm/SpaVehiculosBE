using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ServicesClass.Clases
{
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

                // Solo guardamos una imagen (la primera), puedes adaptarlo si deseas múltiples.
                cliente.Imagen = imagenes.FirstOrDefault();
                dbSuper.SaveChanges();

                return "Imagen guardada correctamente para el cliente.";
            }
            catch (Exception ex)
            {
                return "Error al guardar la imagen: " + ex.Message;
            }
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
    }
}
