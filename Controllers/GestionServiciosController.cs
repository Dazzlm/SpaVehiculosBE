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
    [RoutePrefix("api/Servicios")]
    public class ServiciosController : ApiController
    {
        private readonly GestionServicios gestionServicios = new GestionServicios();

        [HttpGet]
        [Route("ConsultarPorID")]
        public IHttpActionResult ConsultarPorID(int idServicio) 
        {
            var servicio = gestionServicios.BuscarServicioID(idServicio);
            if (servicio == null)
                return NotFound();

            return Ok(servicio);
        }

        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsultarTodos()
        {
            return Ok(gestionServicios.BuscarServicioTodos());
        }

        [HttpDelete]
        [Route("EliminarServicio")]
        public IHttpActionResult EliminarServicio(int idServicio)
        {
            return Ok(gestionServicios.EliminarServicio(idServicio));
        }

        [HttpPut]
        [Route("ActualizarServicio")]
        public IHttpActionResult ActualizarServicio([FromBody] Servicio servicio)
        {
            return Ok(gestionServicios.ActualizarServicio(servicio));
        }

        [HttpPost]
        [Route("CrearServicio")]
        public IHttpActionResult CrearServicio([FromBody] Servicio servicio)
        {
            return Ok(gestionServicios.CrearServicio(servicio));
        }
    }
}