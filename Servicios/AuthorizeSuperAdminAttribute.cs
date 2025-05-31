using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SpaVehiculosBE.Servicios
{
	public class AuthorizeSuperAdminAttribute : AuthorizeAttribute
    {

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var principal = HttpContext.Current?.User as ClaimsPrincipal;
            if (principal == null || !principal.Identity.IsAuthenticated)
                return false;

            var rolClaim = principal.Claims.FirstOrDefault(c => c.Type == "rol_id");
            if (rolClaim == null)
                return false;

            return rolClaim.Value == "1";
        }

    }
}