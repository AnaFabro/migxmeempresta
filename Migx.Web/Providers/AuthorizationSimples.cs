using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Migx.Web.Providers
{
    public class AuthorizeSimples : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Ocorre quando o método AuthorizeCore retorna false. Redireciona para a página de login.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Login", action = "index" })
            );
        }

        /// <summary>
        /// Método chamado antes de qualquer action
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>True se existe 'oUser' na sesão, falso caso contrário.</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["oUser"] != null; 
        }
    }
}