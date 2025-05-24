using SpaVehiculosBE.Servicios;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    [RoutePrefix("api/Login")]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("Ingresar")]
        public IHttpActionResult Ingresar([FromBody] Login login)
        {
            clsLogin servicioLogin = new clsLogin();
            if (login == null)
            {
                return BadRequest("Datos de login no recibidos.");
            }

            servicioLogin.login = login;
            var respuesta = servicioLogin.Ingresar(login);

            if (respuesta.Autenticado)
            {
                return Ok(respuesta);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}