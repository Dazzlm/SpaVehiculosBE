using SpaVehiculosBE.Servicios;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosWEB.Controllers
{
    [RoutePrefix("api/UploadCliente")]
    public class UploadClienteController : ApiController
    {
        private readonly string[] extensionesPermitidas = { ".jpg", ".jpeg", ".png" };
        private const int TAMANIO_MAXIMO_BYTES = 2 * 1024 * 1024;

        private bool ValidarArchivo(HttpPostedFile archivo, out string mensaje)
        {
            mensaje = string.Empty;

            if (archivo == null || archivo.ContentLength == 0)
            {
                mensaje = "Archivo no proporcionado o vacío.";
                return false;
            }

            string extension = Path.GetExtension(archivo.FileName)?.ToLower();

            if (Array.IndexOf(extensionesPermitidas, extension) < 0)
            {
                mensaje = "Extensión de archivo no permitida. Solo se permiten: .jpg, .jpeg, .png.";
                return false;
            }

            if (archivo.ContentLength > TAMANIO_MAXIMO_BYTES)
            {
                mensaje = "El archivo excede el tamaño máximo permitido de 2 MB.";
                return false;
            }

            return true;
        }

        [HttpPost]
        [Route("Subir")]
        public HttpResponseMessage SubirArchivo()
        {
            HttpRequest request = HttpContext.Current.Request;

            if (request.Files.Count == 0 || string.IsNullOrEmpty(request["idCliente"]))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos: archivo o idCliente.");

            var archivo = request.Files[0];

            if (!int.TryParse(request["idCliente"], out int idCliente))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "idCliente inválido.");

            if (!ValidarArchivo(archivo, out string mensajeValidacion))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, mensajeValidacion);

            clsUploadCliente uploader = new clsUploadCliente();
            string resultado = uploader.SubirArchivo(archivo, idCliente);

            if (resultado.StartsWith("Error"))
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, resultado);

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
        }

        [HttpPut]
        [Route("Actualizar")]
        public HttpResponseMessage ActualizarArchivo()
        {
            HttpRequest request = HttpContext.Current.Request;

            if (string.IsNullOrEmpty(request["idCliente"]))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos: idCliente.");

            if (!int.TryParse(request["idCliente"], out int idCliente))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "idCliente inválido.");

            clsUploadCliente uploader = new clsUploadCliente();

            string imagenActual = uploader.ObtenerImagenPorCliente(idCliente);
            if (string.IsNullOrEmpty(imagenActual))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El cliente no tiene imagen asignada. Primero debe asignar una imagen.");

            if (request.Files.Count == 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No se proporcionó ningún archivo para actualizar.");

            var archivo = request.Files[0];

            if (!ValidarArchivo(archivo, out string mensajeValidacion))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, mensajeValidacion);

            string resultado = uploader.ActualizarArchivo(archivo, idCliente);

            if (resultado.StartsWith("Error"))
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, resultado);

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
        }


        [HttpDelete]
        [Route("EliminarPorCliente")]
        public HttpResponseMessage EliminarImagenPorCliente(int idCliente)
        {
            clsUploadCliente uploader = new clsUploadCliente();
            string nombreArchivo = uploader.ObtenerImagenPorCliente(idCliente);

            if (string.IsNullOrEmpty(nombreArchivo))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El cliente no tiene imagen o archivo no encontrado.");

            string resultado = uploader.EliminarArchivo(nombreArchivo);

            if (resultado.StartsWith("Error"))
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, resultado);

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
        }

        [HttpGet]
        [Route("Ver")]
        public HttpResponseMessage VerArchivo(string nombreArchivo)
        {
            var ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/" + nombreArchivo);

            if (!File.Exists(ruta))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

            var resultado = new HttpResponseMessage(HttpStatusCode.OK);
            var contenido = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            resultado.Content = new StreamContent(contenido);

            string tipoMime = MimeMapping.GetMimeMapping(nombreArchivo);
            resultado.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(tipoMime);

            return resultado;
        }

        [HttpGet]
        [Route("Descargar")]
        public HttpResponseMessage Descargar(string nombreArchivo)
        {
            try
            {
                string carpeta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/");

                var archivos = Directory.GetFiles(carpeta, nombreArchivo + ".*");

                if (archivos.Length == 0)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

                string ruta = archivos[0];

                byte[] archivoBytes = File.ReadAllBytes(ruta);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(archivoBytes)
                };

                string fileNameConExtension = Path.GetFileName(ruta);

                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileNameStar = fileNameConExtension
                };

                string extension = Path.GetExtension(ruta).ToLowerInvariant();
                string mimeType = "application/octet-stream"; 

                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        mimeType = "image/jpeg";
                        break;
                    case ".png":
                        mimeType = "image/png";
                        break;
                }

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("ImagenCliente")]
        public HttpResponseMessage MostrarImagenPorCliente(int idCliente)
        {
            clsUploadCliente uploader = new clsUploadCliente();
            string nombreImagen = uploader.ObtenerImagenPorCliente(idCliente);

            if (string.IsNullOrEmpty(nombreImagen))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El cliente no tiene imagen");

            var ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/" + nombreImagen);

            if (!File.Exists(ruta))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

            var resultado = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            resultado.Content = new StreamContent(stream);

            string tipoMime = MimeMapping.GetMimeMapping(nombreImagen);
            resultado.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(tipoMime);

            return resultado;
        }
    }
}
