using System;
using System.Linq;
using System.Web;
using Vueling.Web.Library.Security;

namespace Vueling.XXX.WebUI.Helpers
{
    public static class SecurityHelper
    {
        public static bool IsAuthorizedUser(Type type, string actionName)
        {
            var result = false;
            var roles = "";
            var typeAttributes = type.GetCustomAttributes(false);
            typeAttributes = typeAttributes != null ? typeAttributes.Where(x => x is VuelingAuthorizeAttribute).ToArray() : null;

            var actionAttributes = type.GetMethod(actionName).GetCustomAttributes(false);
            actionAttributes = actionAttributes != null ? actionAttributes.Where(x => x is VuelingAuthorizeAttribute).ToArray() : null;

            var attributes = typeAttributes == null || typeAttributes.Count() == 0 ? actionAttributes : typeAttributes;

            foreach (VuelingAuthorizeAttribute item in attributes)
            {
                roles += item.Roles + ",";
            }
            foreach (var item in roles.Split(','))
            {
                if (HttpContext.Current.User.IsInRole(item))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}