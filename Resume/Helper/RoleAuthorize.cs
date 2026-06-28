using Tastehub.Web.Helper;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tastehub.Web.Helper
{
    public class RoleAuthorize : AuthorizeAttribute
    {
        private string[] RoleNames { get; set; }
        public RoleAuthorize(params string[] roles) : base()
        {
            RoleNames = roles;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (RoleNames.Contains(UserAuthenticate.Role))
            {
                return;
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary(
                     new
                     {
                         controller = "Account",
                         action = "Unauthorized"
                     })
                 );
        }
    }
}