﻿namespace WebApi.Filters
{
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using static WebApi.Extensions.Constants;

    public class GenerateAntiforgeryTokenCookieForAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();

            // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
            var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);
            context.HttpContext.Response.Cookies.Append(
                AntiforgeryHeaderName,
                tokens.RequestToken,
                new CookieOptions() { HttpOnly = false });
        }
    }
}
