using Common.DTOs;
using Common.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Common.Authorizations
{
    public class OwnedResourceHandler : AuthorizationHandler<OwnedResourceRequirement> //TODO: Remove
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnedResourceRequirement requirement)
        {
            // Retrieve the current user's ID, assuming you store it as a claim
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the current user's roles
            var roles = ContextRolesHelper.GetRoles(context);

            if (context.Resource is CreateOwnedResourceDTO || context.Resource is ReadOwnedResourceDTO || context.Resource is EditOwnedResourceDTO)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    // User is authenticated, populate the owner field if applicable
                    if (context.Resource is CreateOwnedResourceDTO createDto) createDto.SetOwner(userId);
                    if (context.Resource is EditOwnedResourceDTO editDto) editDto.SetOwner(userId);

                    context.Succeed(requirement);
                }
            }
            else
            {
                // Only allow admin role if the DTO is not one of the above
                if (roles.Contains("admin"))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
