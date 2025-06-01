using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/StockSedes")]
    [AuthorizeSuperAdmin]

    public class SedeProductoController : ApiController
    {

        private readonly GestorProductoSede gestorProductoSede = new GestorProductoSede();
        private readonly RespuestaHelper validation = new RespuestaHelper();
        [HttpGet]
        [Route("ConsultarPorID")]
        public IHttpActionResult ConsultarPorID(int idSedeProducto)
        {
            List<SedeProducto> sedeProducto = gestorProductoSede.BuscarProductosSedeID(idSedeProducto);
            return Ok(new
            {
                success = true,
                data = sedeProducto
            });
        }

        [HttpGet]
        [Route("ConsultarPorIdSedeProducto")]
        public IHttpActionResult ConsultarPorIdSedeProducto(int idSedeProducto)
        {
            SedeProducto sedeProducto = gestorProductoSede.BuscarPorID(idSedeProducto);
            if (sedeProducto == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                success = true,
                data = sedeProducto
            });
        }

        [HttpGet]
        [Route("ConsultarPorSede")]

        public IHttpActionResult ConsultarPorSede()
        {
            List<SedeProducto> sedeProductos = gestorProductoSede.BuscarProductoSedeTodos();
            if (sedeProductos == null || !sedeProductos.Any())
            {
                return NotFound();
            }
            return Ok(new
            {
                success = true,
                data = sedeProductos
            });
        }
        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult Crear([FromBody] SedeProducto sedeProducto)
        {
            string result = gestorProductoSede.CrearProductoSede(sedeProducto);

            return validation.FormatearRespuesta(this, result);

        }
        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar([FromBody] SedeProducto sedeProducto)
        {
            string result = gestorProductoSede.ActualizarStock(sedeProducto);
            return validation.FormatearRespuesta(this, result);
        }
        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult Eliminar(int idSedeProducto)
        {
            string result = gestorProductoSede.EliminarProductoSede(idSedeProducto);
            return validation.FormatearRespuesta(this, result);
        }
    }
}