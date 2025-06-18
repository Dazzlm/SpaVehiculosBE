using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.UI.WebControls;
using static SpaVehiculosBE.Servicios.Facturas;

namespace SpaVehiculosBE.Controllers
{

    [RoutePrefix("api/Facturas")]
    [AuthorizeSuperAdmin]
    public class FacturasController : ApiController
    {

        private readonly RespuestaHelper validation = new RespuestaHelper();

        [HttpGet]
        [Route("ConsultarFacturas")]
        public IHttpActionResult GetFacturas()
        {
            Facturas facturas = new Facturas();
            RespuestaServicio< List<Factura>> listaFacturas = facturas.GetFacturas();
            return validation.FormatearRespuesta(this, listaFacturas);
        }

        [HttpGet]
        [Route("ConsultarFacturasHoy")]
        public IHttpActionResult GetCantidadFacturasHoy()
        {
            Facturas facturas = new Facturas();
            int cantidad = facturas.ContarFacturasDeHoy();
            return Ok(new{cantidad = cantidad});
        }

        [HttpGet]
        [Route("ConsultarFacturaXId")]
        public IHttpActionResult GetFactura(int id)
        {
            Facturas facturas = new Facturas();
            RespuestaServicio<FacturaCompletedDTO> factura = facturas.GetFactura(id);
            return validation.FormatearRespuesta(this, factura);
        }

        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult AddFactura([FromBody] FacturaCompletedDTO factura)
        {
            if (factura == null)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = "formato invalido"
                });
            }
            Facturas facturas = new Facturas();
            RespuestaServicio<int> response = facturas.AddFactura(factura.Factura, factura.Productos, factura.Servicios);
            return validation.FormatearRespuesta(this, response);
        }
    }
}