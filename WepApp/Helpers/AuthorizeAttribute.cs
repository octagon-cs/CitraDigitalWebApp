using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using WebApp.Models;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string Roles { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else
        {
            if (Roles != null && Roles.Split(",").Length > 0 && !InRole(user))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }

    private bool InRole(User user)
    {
        bool isFound = false;
        for (int i = 0; i < Roles.Split(",").Length; i++)
        {
            var roleName = Roles.Split(",")[i].Trim();
            var data = user.UserRoles.FirstOrDefault(x => x.Role.Name.ToLower() == roleName.ToLower());
            if (data != null)
            {
                isFound = true;
                break;
            }
        }
        return isFound;
    }
}