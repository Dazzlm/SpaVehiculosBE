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
    [RoutePrefix("api/Reservas")]
    public class ReservasController : ApiController
    {
        private readonly Reservas reservas = new Reservas();

        [HttpGet]
        [Route("ConsultarPorID")]
        public IHttpActionResult ConsultarPorID(int idReserva)
        {
            var reserva = reservas.ConsultarReservaID(idReserva);
            if (reserva == null)
            {
                return NotFound(); 
            }
            return Ok(reserva); 
        }

        [HttpGet]
        [Route("ConsultarPorCliente")]
        public IHttpActionResult ConsultarPorCliente(int idCliente)
        {
            var reservasCliente = reservas.ConsultarReservasPorCliente(idCliente);
            if (reservasCliente == null || !reservasCliente.Any())
            {
                return NotFound(); 
            }
            return Ok(reservasCliente); 
        }

        [HttpGet]
        [Route("ConsultarPorFecha")]
        public IHttpActionResult ConsultarPorFecha(DateTime fecha)
        {
            var reservasFecha = reservas.ConsultarReservasPorFecha(fecha);
            if (reservasFecha == null || !reservasFecha.Any())
            {
                return NotFound();
            }
            return Ok(reservasFecha);
        }

        [HttpGet]
        [Route("ConsultarTodos")]
        public IHttpActionResult ConsultarTodos()
        {
            var todasLasReservas = reservas.ConsultarReservasTodos();
            if (todasLasReservas == null || !todasLasReservas.Any())
            {
                return NotFound(); 
            }
            return Ok(todasLasReservas); 
        }

        [HttpPost]
        [Route("CrearReserva")]
        public IHttpActionResult CrearReserva([FromBody] Reserva reserva)
        {
            if (reserva == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            string result = reservas.CrearReserva(reserva);

            if (result.StartsWith("Error") || result.Contains("no válido") || result.Contains("solapa"))
            {
                return BadRequest(result);
            }
            return Ok(new { Message = result, IdReserva = reserva.IdReserva }); 
        }

        [HttpPut]
        [Route("ActualizarReserva")]
        public IHttpActionResult ActualizarReserva([FromBody] Reserva reserva)
        {
            if (reserva == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            string result = reservas.ActualizarReserva(reserva);

            if (result.StartsWith("Error") || result.Contains("no encontrada") || result.Contains("no válido") || result.Contains("solapa"))
            {
              
                if (result.Contains("no encontrada"))
                {
                    return NotFound();
                }
                return BadRequest(result);
            }
            return Ok(new { Message = result }); 
        }

        [HttpDelete]
        [Route("EliminarReserva")]
        public IHttpActionResult EliminarReserva(int idReserva)
        {
            string result = reservas.EliminarReserva(idReserva);

            if (result.StartsWith("Error") || result.Contains("no encontrada"))
            {
                if (result.Contains("no encontrada"))
                {
                    return NotFound(); 
                }
                return BadRequest(result); 
            }
            return Ok(new { Message = result }); 
        }




    }
}