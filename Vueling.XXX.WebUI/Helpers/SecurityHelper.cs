using System;
using System.Collections.Generic;
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
            
            var typeAttributes = type.GetCustomAttributes(false);
            typeAttributes = typeAttributes != null ? typeAttributes.Where(x => x is VuelingAuthorizeAttribute).ToArray() : null;

            var actionAttributes = type.GetMethod(actionName).GetCustomAttributes(false);
            actionAttributes = actionAttributes != null ? actionAttributes.Where(x => x is VuelingAuthorizeAttribute).ToArray() : null;

            var attributes = typeAttributes == null || !typeAttributes.Any() ? actionAttributes : typeAttributes;

            if (attributes != null)
            {
                List<string> roles = new List<string>();
                foreach (VuelingAuthorizeAttribute item in attributes)
                {
                    roles.AddRange(item.Roles.Split('\''));
                }

                result = roles.Any(c => HttpContext.Current.User.IsInRole(c));
            }
            return result;
        }
    }
}