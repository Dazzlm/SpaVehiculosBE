using SpaVehiculosBE.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SpaVehiculosBE
{
    public class RespuestaHelper
    {
        public IHttpActionResult FormatearRespuesta<T>(ApiController controller, RespuestaServicio<T> respuestaServicio)
        {
            HttpStatusCode status;
            string mensaje;

            if (!respuestaServicio.Success)
            {
                mensaje = string.Join(" | ", respuestaServicio.Message);

                if (respuestaServicio.Message.Contains("404"))
                    status = HttpStatusCode.NotFound;
                else
                    status = HttpStatusCode.BadRequest;
            }
            else
            {
                mensaje = "Operación exitosa";
                status = HttpStatusCode.OK;
            }

           
            return new ResponseMessageResult(controller.Request.CreateResponse(status, respuestaServicio));
        }
    }
}
