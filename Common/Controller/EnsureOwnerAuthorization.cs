using Common.DTOs;
using Common.Model;
using Common.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System.Security.Claims;


namespace Common.Controller
{
    public static class EnsureOwnerAuthorization
    {

        public static bool Authorize<TReadDto>(TReadDto entity, ClaimsPrincipal user)
        {

            if (entity is ReadOwnedResourceDTO ownedResource)
            {
                var ownerId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return (ownerId == ownedResource.Owner);
            }
            else
            {
                return user.Identity.IsAuthenticated;
            }
        }
    }

}