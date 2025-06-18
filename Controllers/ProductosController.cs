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
            RespuestaServicio<List<Producto>> productos = _gestor.ObtenerTodos();
            return _validation.FormatearRespuesta(this, productos);
        }

        [HttpGet]
        [Route("ObtenerPorId")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            RespuestaServicio<Producto> producto = _gestor.ObtenerPorId(id);

            return _validation.FormatearRespuesta(this, producto);
        }

        [HttpGet]
        [Route("ObtenerPorSede")]
        public IHttpActionResult ObtenerPorSede(int idSede)
        {
            RespuestaServicio<List<Producto>> productos = _gestor.ObtenerPorSede(idSede);
            return _validation.FormatearRespuesta(this, productos);
        }

        [HttpGet]
        [Route("ObtenerConStockPorSede")]
        public IHttpActionResult ObtenerConStockPorSede(int idSede)
        {                
            RespuestaServicio<List<ProductoConStockDTO>> productosConStock = _gestor.ObtenerConStockPorSede(idSede);
             return _validation.FormatearRespuesta(this, productosConStock);
        }

        [HttpPost]
        [Route("Crear")]
        public IHttpActionResult CrearProducto(Producto producto)
        {

            RespuestaServicio<string> result = _gestor.Crear(producto);
            return _validation.FormatearRespuesta(this, result);
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult ActualizarProducto(Producto producto)
        {

            RespuestaServicio<string> result = _gestor.Actualizar(producto);

            return _validation.FormatearRespuesta(this, result);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult EliminarProducto(int id)
        {
            RespuestaServicio<string> result = _gestor.Eliminar(id);
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