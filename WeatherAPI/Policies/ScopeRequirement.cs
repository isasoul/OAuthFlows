using Microsoft.AspNetCore.Authorization;

public class ScopeRequirement : IAuthorizationRequirement
{
	public string Issuer { get; }
	public string Scope { get; }

	public ScopeRequirement(string scope, string issuer )
	{
		Issuer = issuer ?? "valorPorDefecto";
		Scope = scope ?? throw new ArgumentNullException(nameof(scope), "El argumento 'scope' no puede ser nulo.");
	}
}
