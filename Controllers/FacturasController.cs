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
                return Content(HttpStatusCode.NotFound, new
                {
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
            Response response = facturas.AddFactura(factura.Factura, factura.Productos, factura.Servicios);
            string result = response.Message;

            if (result.Contains("Error404"))
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = result,
                    

                });
            }

            if (result.Contains("Error"))
            {
                return  Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = result,
                });
            }

            return  Content(HttpStatusCode.OK, new
            {
                success = true,
                message = result,
                IdFactura = response.IdFactura
            });

        }
    }
}