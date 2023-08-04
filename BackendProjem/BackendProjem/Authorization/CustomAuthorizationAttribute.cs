using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BackendProjem.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private List<string> _roles = new List<string>();



        public CustomAuthorizationAttribute(params object[] roles)
        {
            foreach (var item in roles)
            {
                _roles.Add(item.ToString());
            }
        }



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_roles.Any())
            {
                if (context.HttpContext.User.Claims == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }



                var claim = context.HttpContext.User.Claims.FirstOrDefault(s => s.Type == "ClientType");



                if (claim == null || !_roles.Any(s => claim.Value == s))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }
    }
}
