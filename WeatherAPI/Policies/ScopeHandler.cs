using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;


namespace WeatherAPI.Policies
{
	public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
		{
			//verificamos si existe el Claim "scope" y que el emisor del token sea el requierido.
			if (context.User.HasClaim(c => c.Type ==
				"http://schemas.microsoft.com/identity/claims/scope" &&
				c.Issuer == requirement.Issuer))
			{
				var scopeClaim = context.User.FindFirst(c =>
				c.Type == "http://schemas.microsoft.com/identity/claims/scope" &&
				c.Issuer == requirement.Issuer);

				string[] Scopes = scopeClaim != null ? scopeClaim.Value.Split(' ') : Array.Empty<string>();


				//verificamos que se encuentre es scope requerido
				if (Scopes.Any(s => s == requirement.Scope))
					//indicamos que se cumple el requerimiento
					context.Succeed(requirement);

			}
			return Task.CompletedTask;

		}

		//protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
		//{
		//	var scopeClaim = context.User.FindFirst(c => c.Type == "http://schemas.microsoft.com/identity/claims/scope" &&
		//												 c.Issuer == requirement.Issuer);

		//	if (scopeClaim != null)
		//	{
		//		string[] scopes = scopeClaim.Value.Split(' ');

		//		if (scopes.Contains(requirement.Scope))
		//		{
		//			context.Succeed(requirement);
		//		}
		//	}

		//	// Log error or failure here if needed
		//	// ...

		//	return Task.CompletedTask;
		//}


	}
}
