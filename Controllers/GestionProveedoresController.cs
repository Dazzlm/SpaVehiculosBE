using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    public class GestionProveedoresController
    {
       

        [RoutePrefix("api/GestorProv")]
        [AuthorizeSuperAdmin]

        public class GestorProveedoresController : ApiController
        {
            private readonly RespuestaHelper validation = new RespuestaHelper();
            [HttpGet]
            [Route("ConsultarporID")]
            public IHttpActionResult ProvPorID(int idProveedor)
            {
                GestionProveedores servicioProveedor = new GestionProveedores();
                return validation.FormatearRespuesta(this, servicioProveedor.BuscarProveedorID(idProveedor));
            }
            [HttpGet]
            [Route("ConsultarTodos")]
            public IHttpActionResult TodosProv()
            {
                GestionProveedores servicioProveedores = new GestionProveedores();
                return validation.FormatearRespuesta(this, servicioProveedores.BuscarProveedorTodos());
            }
            [HttpDelete]
            [Route("EliminarProveedor")]
            public IHttpActionResult ElminarProv(int idProveedor) {
                GestionProveedores servicioProveedores = new GestionProveedores();
                return validation.FormatearRespuesta(this, servicioProveedores.EliminarProveedor(idProveedor));

            }
            [HttpPut]
            [Route("ActualizarProveedor")]
            public IHttpActionResult ActualizarProv(Proveedor proveedor) {
                GestionProveedores servicioProveedores = new GestionProveedores();
                return validation.FormatearRespuesta(this, servicioProveedores.ActualizarProveedor(proveedor));
            }
            [HttpPost]
            [Route("CrearProveedor")]
            public IHttpActionResult CrearProveedor([FromBody]Proveedor proveedor) {
                GestionProveedores servicioProveedores = new GestionProveedores();
                return validation.FormatearRespuesta(this, servicioProveedores.CrearProvedor(proveedor));

            }
        }
    }
}