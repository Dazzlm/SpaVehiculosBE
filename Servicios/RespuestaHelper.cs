using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SpaVehiculosBE
{
    public class RespuestaHelper
    {
        public IHttpActionResult FormatearRespuesta(ApiController controller, string resultado)
        {
            if (resultado.Contains("Error404"))
            {
                return new ResponseMessageResult(controller.Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = resultado
                }));
            }

            if (resultado.Contains("Error"))
            {
                return new ResponseMessageResult(controller.Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = resultado
                }));
            }

            return new ResponseMessageResult(controller.Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                message = resultado
            }));
        }
    }
}