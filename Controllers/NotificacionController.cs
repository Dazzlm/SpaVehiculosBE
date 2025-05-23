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

        private readonly ValidationController validation = new ValidationController();

        [HttpGet]
        [Route("EnviarNotificacion")]
        public IHttpActionResult EnviarNotificacion(int id)
        {
            Notificacion notificacion = new Notificacion();
            string result = notificacion.EnviarFactura(id);
            return validation.ValidationResult(result);
        }

    }
}