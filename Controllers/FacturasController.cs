using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using static SpaVehiculosBE.Servicios.Facturas;

namespace SpaVehiculosBE.Controllers
{
    
    [RoutePrefix("api/Facturas")]
    public class FacturasController : ApiController
    {

        private readonly ValidationController validation = new ValidationController();

        [HttpGet]
        [Route("ConsultarFacturas")]
        public IHttpActionResult GetFacturas()
        {
            Facturas facturas = new Facturas();
            List<Factura> listaFacturas = facturas.GetFacturas();
            return Ok(new
            {
                success = true,
                data = listaFacturas
            });
        }

        [HttpGet]
        [Route("ConsultarFacturaXId")]
        public IHttpActionResult GetFactura(int id)
        {
            Facturas facturas = new Facturas();
            FacturaCompletedDTO factura = facturas.GetFactura(id);
            if (factura == null)
            {
              return Content(HttpStatusCode.NotFound, new {
                  success = false,
                  message = "Factura no encontrada"
              });
            }
            return Ok(new
            {
                success = true,
                data = factura
            });
        }

        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult AddFactura([FromBody] FacturaCompletedDTO factura)
        {
            if (factura == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = "formato invalido"
                });
            }
            Facturas facturas = new Facturas();
            string result = facturas.AddFactura(factura.Factura, factura.Productos, factura.Servicios);
            return validation.ValidationResult(result);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult DeleteFactura(int id)
        {
            Facturas facturas = new Facturas();
            string result = facturas.DeleteFactura(id);
            return validation.ValidationResult(result);
        }
    }
}