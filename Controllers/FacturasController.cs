using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using static SpaVehiculosBE.Servicios.Facturas;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/Facturas")]
    public class FacturasController : ApiController
    {
        [HttpGet]
        [Route("ConsultarFacturas")]
        public IHttpActionResult GetFacturas()
        {
            Facturas facturas = new Facturas();
            List<Factura> listaFacturas = facturas.GetFacturas();
            return Ok(listaFacturas);
        }

        [HttpGet]
        [Route("ConsultarFacturaXId")]
        public IHttpActionResult GetFactura(int id)
        {
            Facturas facturas = new Facturas();
            FacturaCompletedDTO factura = facturas.GetFactura(id);
            if (factura == null)
            {
              return Content(HttpStatusCode.NotFound, "Factura no encontrada");
            }
            return Ok(factura);
        }

        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult AddFactura([FromBody] FacturaCompletedDTO factura)
        {
            if (factura == null)
            {
                return BadRequest("Formato invalido");
            }
            Facturas facturas = new Facturas();
            string result = facturas.AddFactura(factura.Factura, factura.Productos, factura.Servicios);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult DeleteFactura(int id)
        {
            Facturas facturas = new Facturas();
            string result = facturas.DeleteFactura(id);

            if (result.Contains("Error404"))
            {
                return Content(HttpStatusCode.NotFound, "Factura no encontrada");
            }

            if (result.Contains("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result); ;

        }

    }
}