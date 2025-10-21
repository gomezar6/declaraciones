using EIGO.PDLA.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
// TAKEN from https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/blob/master/5-WebApp-AuthZ/5-2-Groups/Infrastructure/CustomAuthorization.cs
namespace EIGO.PDLA.Common.Policies
{
    /// <summary>
    /// GroupPolicyHandler deals with custom Policy-based authorization.
    /// GroupPolicyHandler evaluates the GroupPolicyRequirement against AuthorizationHandlerContext 
    /// by calling CheckUsersGroupMembership method to determine if authorization is allowed.
    /// </summary>
    public class GroupPolicyHandler : AuthorizationHandler<GroupPolicyRequirement>
    {
        /// <summary>
        /// Makes a decision if authorization is allowed based on GroupPolicyRequirement.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   GroupPolicyRequirement requirement)
        {
            var pendingRequirements = context.PendingRequirements.ToList();
            foreach (var item in pendingRequirements)
            {
                if (item is GroupPolicyRequirement && GraphHelper.CheckUsersGroupMembership(context.User, requirement.GroupName))
                {
                    context.Succeed(requirement);
                }
            }
            // Calls method to check if requirement exists in user claims or session.

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// GroupPolicyRequirement contains data parameter that 
    /// GroupPolicyHandler uses to evaluate against the current user principal or session data.
    /// </summary>
    public class GroupPolicyRequirement : IAuthorizationRequirement
    {
        public string GroupName { get; }
        public GroupPolicyRequirement(string GroupName)
        {
            this.GroupName = GroupName;
        }
    }
}
