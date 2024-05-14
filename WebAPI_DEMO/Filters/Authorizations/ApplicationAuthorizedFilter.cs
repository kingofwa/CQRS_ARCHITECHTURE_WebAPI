using Application.Enum;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI_Demo.Filters.Authorizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApplicationAuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Roles> _roles;

        public ApplicationAuthorizedAttribute(params Roles[] roles)
        {
            _roles = roles ?? Array.Empty<Roles>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var currentUser = context.HttpContext.Items["User"] as User;
                var listRole = context.HttpContext.Items["Roles"] as List<Role>;
                var listRoles = new List<string>();

                if (listRole != null)
                {
                    foreach (var item in listRole)
                    {
                        listRoles.Add(item.Name);
                    }
                }

                if (currentUser == null || listRoles == null)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }

                if (_roles.Any() && !_roles.Any(x => listRoles.Contains(x.ToString())))
                {
                    // role not authorized
                    context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

    }
}
