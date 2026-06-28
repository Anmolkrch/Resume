using System.Web.Mvc;
using System.Web.Routing;
using TMS.Model.Master;

namespace TMS.Web.Helper
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public string ParamName { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey(ParamName))
            {
                var data = filterContext.ActionParameters[ParamName] as string;
                Qparams qparams = null;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    qparams = Helper.DecryptParamData<Qparams>(data);
                    if (UserAuthenticate.LogId != qparams.LogId?.ToString())
                        HandleUnauthorizedRequest(filterContext);
                }
            }
            return;
            //base.OnActionExecuting(filterContext);
        }
        protected void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
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