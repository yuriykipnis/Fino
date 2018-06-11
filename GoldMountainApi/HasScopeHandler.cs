using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GoldMountainApi
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            HasScopeRequirement requirement)
        {
            //var client = new RestClient("https://__AUTH0_NAMESPACE__/api/v2/users");
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("authorization", "Bearer YOUR_MGMT_API_ACCESS_TOKEN");
            //IRestResponse response = client.Execute(request);
            //var client =
            //    new RestClient(
            //        "https://__AUTH0_NAMESPACE__/api/v2/users/user_id?fields=user_metadata&include_fields=true");
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("content-type", "application/json");
            //request.AddHeader("authorization", "Bearer ABCD");
            //IRestResponse response = client.Execute(request);


            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value
                .Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
