using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static SpaVehiculosBE.Servicios.GestorFacturaPDF;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/GestorPDF")]
    public class GestorPDFController : ApiController
    {

        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("descargar")]
        public HttpResponseMessage DescargarFactura(int id)
        {
            GestorFacturaPDF pdfService = new GestorFacturaPDF();
            PDFResult resultado = pdfService.ObtenerPDF(id);

            if (!resultado.Success)
            {
                return Request.CreateResponse(
                    resultado.Mensaje == "Factura no encontrada" ? HttpStatusCode.NotFound : HttpStatusCode.InternalServerError,
                    new
                    {
                        success = false,
                        message = resultado.Mensaje
                    }

                );
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(resultado.Archivo);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = resultado.NombreArchivo
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            return response;
        }

        [HttpDelete]
        [Route("EliminarPDF")]
        public IHttpActionResult EliminarPDF(int id)
        {
            GestorFacturaPDF pdfService = new GestorFacturaPDF();
            string result = pdfService.EliminarPDF(id);
            return validation.FormatearRespuesta(this, result);

        }
    }
}