using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
    public class GestorImagen
    {

        private readonly string _directorioRaiz = HttpContext.Current.Server.MapPath("~/Uploads");

        public string GuardarImagen(HttpPostedFile archivo, string carpeta)
        {
            if (archivo == null || archivo.ContentLength == 0)
            {
                return "Error: Archivo no válido";
            }

            string nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivo.FileName);
            string carpetaDestino = Path.Combine(_directorioRaiz, carpeta);

            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);
            archivo.SaveAs(rutaCompleta);

            return $"~/Uploads/{carpeta}/{nombreArchivo}";
        }
    }
}