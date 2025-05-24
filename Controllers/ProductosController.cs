using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/Productos")]
    public class ProductosController : ApiController
    {
        private readonly GestorProductos _gestor = new GestorProductos();
        private readonly RespuestaHelper _validation = new RespuestaHelper();

        [HttpGet]
        [Route("ObtenerTodos")]
        public IHttpActionResult ObtenerTodos()
        {
            List<Producto> productos = _gestor.ObtenerTodos();
            return Ok(new
            {
                success = true,
                data = productos
            });
        }

        [HttpGet]
        [Route("ObtenerPorId")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            Producto producto = _gestor.ObtenerPorId(id);

            if (producto == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = "Producto no encontrado"
                });
            }

            return Ok(new
            {
                success = true,
                data = producto
            });
        }

        [HttpGet]
        [Route("ObtenerPorSede")]
        public IHttpActionResult ObtenerPorSede(int idSede)
        {
            List<Producto> productos = _gestor.ObtenerPorSede(idSede);
            return Ok(new
            {
                success = true,
                data = productos
            });
        }

        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult CrearProducto(Producto producto)
        {

            string result = _gestor.Crear(producto);
            return _validation.FormatearRespuesta(this, result);
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult ActualizarProducto(Producto producto)
        {

            string result = _gestor.Actualizar(producto);

            return _validation.FormatearRespuesta(this, result);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult EliminarProducto(int id)
        {
            string result = _gestor.Eliminar(id);
            return _validation.FormatearRespuesta(this, result);
        }

    }
}