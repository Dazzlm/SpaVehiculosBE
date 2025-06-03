using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using static SpaVehiculosBE.Servicios.GestorProductos;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/Productos")]
    [AuthorizeSuperAdmin]

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

        [HttpGet]
        [Route("ObtenerConStockPorSede")]
        public IHttpActionResult ObtenerConStockPorSede(int idSede)
        {
            try
            {
                List<ProductoConStockDTO> productosConStock = _gestor.ObtenerConStockPorSede(idSede);

                if (productosConStock == null )
                {
                    return Content(HttpStatusCode.NotFound, new {
                        success = false,
                        message = "No se encontraron productos con stock para la sede especificada."
                    } );
                }

                return Ok(new
                {
                    success = true,
                    data = productosConStock
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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
        [HttpGet]
        [Route("Contar")]
        public IHttpActionResult ContarProductos()
        {
            int cantidad = _gestor.ContarProductos();
            return Ok(new
            {
                success = true,
                cantidad = cantidad
            });
        }

    }
}