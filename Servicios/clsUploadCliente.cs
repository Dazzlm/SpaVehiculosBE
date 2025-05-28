using ServicesClass.Clases;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
    public class clsUploadCliente
    {
        public string SubirArchivo(HttpPostedFile archivo, int idCliente)
        {
            try
            {
                GestionClientes gestionClientes = new GestionClientes();

                bool clienteExiste = gestionClientes.ClienteExiste(idCliente);
                if (!clienteExiste)
                    return "Cliente no existe";

                string ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                string extension = Path.GetExtension(archivo.FileName);
                string nombreFinal = $"cliente_{idCliente}{extension}";
                string rutaCompleta = Path.Combine(ruta, nombreFinal);

                archivo.SaveAs(rutaCompleta);

                List<string> listaImagenes = new List<string> { nombreFinal };

                string resultadoBD = gestionClientes.GrabarImagenCliente(idCliente, listaImagenes);

                if (resultadoBD.StartsWith("Error") || resultadoBD.Contains("no"))
                {
                    if (File.Exists(rutaCompleta))
                        File.Delete(rutaCompleta);

                    return resultadoBD;
                }

                return resultadoBD;
            }
            catch (Exception ex)
            {
                return "Error al subir archivo: " + ex.Message;
            }
        }


        public string ActualizarArchivo(HttpPostedFile archivo, int idCliente)
        {
            try
            {
                GestionClientes gestionClientes = new GestionClientes();

                // Validar existencia del cliente
                if (!gestionClientes.ClienteExiste(idCliente))
                {
                    return "Error: El cliente no existe.";
                }

                string imagenActual = gestionClientes.ObtenerImagenPorCliente(idCliente);
                if (!string.IsNullOrEmpty(imagenActual))
                {
                    string rutaImagenActual = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/" + imagenActual);
                    if (File.Exists(rutaImagenActual))
                        File.Delete(rutaImagenActual);
                }

                string ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                string extension = Path.GetExtension(archivo.FileName);
                string nombreFinal = $"cliente_{idCliente}{extension}";
                string rutaCompleta = Path.Combine(ruta, nombreFinal);
                archivo.SaveAs(rutaCompleta);

                List<string> listaImagenes = new List<string> { nombreFinal };
                string resultadoBD = gestionClientes.GrabarImagenCliente(idCliente, listaImagenes);

                if (resultadoBD.StartsWith("Error") || resultadoBD.Contains("no"))
                {
                    if (File.Exists(rutaCompleta))
                        File.Delete(rutaCompleta);

                    return resultadoBD;
                }

                return resultadoBD;
            }
            catch (Exception ex)
            {
                return "Error al actualizar archivo: " + ex.Message;
            }
        }


        public string EliminarArchivo(string nombreArchivo)
        {
            try
            {
                string ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/" + nombreArchivo);
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);

                    GestionClientes gestionClientes = new GestionClientes();
                    return gestionClientes.EliminarImagenCliente(nombreArchivo);
                }
                else
                {
                    return "Archivo no encontrado.";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el archivo: " + ex.Message;
            }
        }

        public string ObtenerImagenPorCliente(int idCliente)
        {
            GestionClientes gestionClientes = new GestionClientes();
            return gestionClientes.ObtenerImagenPorCliente(idCliente);
        }
    }
}
