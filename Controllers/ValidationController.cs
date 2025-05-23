using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Controllers
{
    public class ValidationController : ApiController
    {
        // GET: Validation
        [NonAction]
        public IHttpActionResult ValidationResult(string result)
        {
            if (result.Contains("Error404"))
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = result
                });
            }

            if (result.Contains("Error"))
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = result
                });
            }

            return Ok(new
            {
                success = true,
                message = result
            });
        }
    }
}