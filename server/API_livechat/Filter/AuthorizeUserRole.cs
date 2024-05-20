using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace API_livechat.Filter
{
    public class AuthorizeUserRole : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredUserRole;
        public AuthorizeUserRole(string requiredUserRole)
        {
            _requiredUserRole = requiredUserRole;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userClaims = context.HttpContext.User.Claims;
            var userRole = userClaims.FirstOrDefault(u => u.Type == "UserRole")?.Value;

            if (userRole == null || userRole != _requiredUserRole)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}

