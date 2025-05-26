using SpaVehiculosBE.Servicios;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosWEB.Controllers
{
    [RoutePrefix("api/UploadCliente")]
    public class UploadClienteController : ApiController
    {
        [HttpPost]
        [Route("Subir")]
        public HttpResponseMessage SubirArchivo()
        {
            HttpRequest request = HttpContext.Current.Request;

            if (request.Files.Count == 0 || string.IsNullOrEmpty(request["idCliente"]))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos: archivo o idCliente.");

            var archivo = request.Files[0];
            string idCliente = request["idCliente"];

            clsUploadCliente uploader = new clsUploadCliente();
            string resultado = uploader.SubirArchivo(archivo, idCliente);

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
        }

        [HttpDelete]
        [Route("EliminarPorCliente")]
        public HttpResponseMessage EliminarImagenPorCliente(int idCliente)
        {
            clsUploadCliente uploader = new clsUploadCliente();
            // Obtiene el nombre del archivo asociado al cliente
            string nombreArchivo = uploader.ObtenerImagenPorCliente(idCliente);

            if (string.IsNullOrEmpty(nombreArchivo))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El cliente no tiene imagen o archivo no encontrado.");

            // Elimina archivo y actualiza BD
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

            if (!System.IO.File.Exists(ruta))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

            var resultado = new HttpResponseMessage(HttpStatusCode.OK);
            var contenido = new System.IO.FileStream(ruta, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            resultado.Content = new StreamContent(contenido);
            resultado.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png"); // cambia si es jpg o pdf

            return resultado;
        }
        [HttpGet]
        [Route("Descargar")]
        public HttpResponseMessage Descargar(string nombreArchivo)
        {
            try
            {
                string ruta = HttpContext.Current.Server.MapPath("~/Imagenes/Clientes/" + nombreArchivo);

                if (!System.IO.File.Exists(ruta))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Archivo no encontrado");
                }

                byte[] archivoBytes = System.IO.File.ReadAllBytes(ruta);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(archivoBytes);

                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                // Usar FileNameStar para evitar problemas con codificación y extensión
                response.Content.Headers.ContentDisposition.FileNameStar = nombreArchivo;

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

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

            if (!System.IO.File.Exists(ruta))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

            var resultado = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new System.IO.FileStream(ruta, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            resultado.Content = new StreamContent(stream);

            string tipoMime = MimeMapping.GetMimeMapping(nombreImagen);
            resultado.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(tipoMime);

            return resultado;
        }
        [HttpPut]
        [Route("Actualizar")]
        public HttpResponseMessage ActualizarArchivo()
        {
            HttpRequest request = HttpContext.Current.Request;

            if (request.Files.Count == 0 || string.IsNullOrEmpty(request["idCliente"]))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos: archivo o idCliente.");

            var archivo = request.Files[0];
            int idCliente;
            if (!int.TryParse(request["idCliente"], out idCliente))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "idCliente inválido.");

            clsUploadCliente uploader = new clsUploadCliente();
            string resultado = uploader.ActualizarArchivo(archivo, idCliente);

            if (resultado.StartsWith("Error"))
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, resultado);

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
        }


    }
}
