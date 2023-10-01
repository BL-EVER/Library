using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Common.Controller
{
    public class AuthorizeAndPopulateOwnedResourceAttribute : AuthorizeAttribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, return an Unauthorized result
                context.Result = new UnauthorizedResult();
                return;
            }
            // Loop through each argument in the action method
            foreach (var arg in context.ActionArguments)
            {
                // Get the current user's id from the claims and set on the DTO
                var ownerId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (arg.Value is CreateOwnedResourceDTO ownedResource) ownedResource.SetOwner(ownerId);
                if (arg.Value is EditOwnedResourceDTO editOwnedResource) editOwnedResource.SetOwner(ownerId);

            }

            await next();
        }
    }
}
