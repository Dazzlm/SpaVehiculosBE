using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/Notificacion")]
    public class NotificacionController : ApiController
    {

        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("EnviarNotificacion")]
        public IHttpActionResult EnviarNotificacion(int id)
        {
            Notificacion notificacion = new Notificacion();
            string result = notificacion.EnviarFactura(id);
            return validation.FormatearRespuesta(this, result);
        }

    }
}