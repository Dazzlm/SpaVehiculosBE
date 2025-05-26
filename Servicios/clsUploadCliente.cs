using ServicesClass.Clases;
using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
    public class clsUploadCliente
    {
        public string SubirArchivo(HttpPostedFile archivo, string idCliente)
        {
            try
            {
                string ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                string nombreFinal = Path.GetFileName(archivo.FileName);
                string rutaCompleta = Path.Combine(ruta, nombreFinal);
                archivo.SaveAs(rutaCompleta);

                List<string> listaImagenes = new List<string> { nombreFinal };

                // Usar la clase correcta: GestionClientes
                GestionClientes gestionClientes = new GestionClientes();
                return gestionClientes.GrabarImagenCliente(Convert.ToInt32(idCliente), listaImagenes);
            }
            catch (Exception ex)
            {
                return "Error al subir archivo: " + ex.Message;
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

                    // Usar la clase correcta: GestionClientes
                    GestionClientes gestionClientes = new GestionClientes();
                    string resultadoBD = gestionClientes.EliminarImagenCliente(nombreArchivo);
                    return resultadoBD;
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
        public string ActualizarArchivo(HttpPostedFile archivo, int idCliente)
        {
            try
            {
                GestionClientes gestionClientes = new GestionClientes();

                // Obtener la imagen actual para eliminarla primero
                string imagenActual = gestionClientes.ObtenerImagenPorCliente(idCliente);
                if (!string.IsNullOrEmpty(imagenActual))
                {
                    string rutaImagenActual = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/" + imagenActual);
                    if (File.Exists(rutaImagenActual))
                    {
                        File.Delete(rutaImagenActual);
                    }
                }

                // Guardar la nueva imagen
                string ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                string nombreFinal = Path.GetFileName(archivo.FileName);
                string rutaCompleta = Path.Combine(ruta, nombreFinal);
                archivo.SaveAs(rutaCompleta);

                // Actualizar la base de datos con la nueva imagen
                List<string> listaImagenes = new List<string> { nombreFinal };
                return gestionClientes.GrabarImagenCliente(idCliente, listaImagenes);
            }
            catch (Exception ex)
            {
                return "Error al actualizar archivo: " + ex.Message;
            }
        }

    }
}