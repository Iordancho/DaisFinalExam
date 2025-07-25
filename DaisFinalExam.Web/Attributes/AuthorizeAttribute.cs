﻿// Attributes/AuthorizeAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DaisFinalExam.Web.Attributes
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                var returnUrl = context.HttpContext.Request.Path.ToString();
                context.Result = new RedirectToActionResult("Login", "Authorization", new { returnUrl });
            }
        }
    }
}
