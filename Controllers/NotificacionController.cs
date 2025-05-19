using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/Notifiacion")]
    public class NotificacionController : ApiController
    {
        [HttpGet]
        [Route("EnviarNotificacion")]
        public IHttpActionResult EnviarNotificacion(int id)
        {
            Notificacion notificacion = new Notificacion();
            string result = notificacion.EnviarFactura(id);
            if (result.Contains("Error404"))
            {
                return Content(HttpStatusCode.NotFound, result);
            }
            if (result.Contains("Error"))
            {
                return Content(HttpStatusCode.InternalServerError, result);
            }
            return Ok(result);
        }


    }
}