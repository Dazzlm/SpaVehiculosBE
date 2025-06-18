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
            RespuestaServicio< List<SedeProducto>> sedeProducto = gestorProductoSede.BuscarProductosSedeID(idSedeProducto);
            return validation.FormatearRespuesta(this, sedeProducto);
        }

        [HttpGet]
        [Route("ConsultarPorIdSedeProducto")]
        public IHttpActionResult ConsultarPorIdSedeProducto(int idSedeProducto)
        {
            RespuestaServicio<SedeProducto> sedeProducto = gestorProductoSede.BuscarPorID(idSedeProducto);
            return validation.FormatearRespuesta(this, sedeProducto);
        }

        [HttpGet]
        [Route("ConsultarPorSede")]

        public IHttpActionResult ConsultarPorSede()
        {
            RespuestaServicio<List<SedeProducto>> sedeProductos = gestorProductoSede.BuscarProductoSedeTodos();
            return validation.FormatearRespuesta(this, sedeProductos);
        }
        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult Crear([FromBody] SedeProducto sedeProducto)
        {
            RespuestaServicio<string> result = gestorProductoSede.CrearProductoSede(sedeProducto);

            return validation.FormatearRespuesta(this, result);

        }
        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar([FromBody] SedeProducto sedeProducto)
        {
            RespuestaServicio<string> result = gestorProductoSede.ActualizarStock(sedeProducto);
            return validation.FormatearRespuesta(this, result);
        }
        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult Eliminar(int idSedeProducto)
        {
            RespuestaServicio<string> result = gestorProductoSede.EliminarProductoSede(idSedeProducto);
            return validation.FormatearRespuesta(this, result);
        }

        [HttpDelete]
        [Route("EliminarPorId")]
        public IHttpActionResult EliminarPorId(int idSedeProducto)
        {
            RespuestaServicio<string> result = gestorProductoSede.EliminarProductoSedeId(idSedeProducto);
            return validation.FormatearRespuesta(this, result);
        }
    }
}