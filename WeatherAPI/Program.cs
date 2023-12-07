using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WeatherAPI.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
//seccion para autenticacion
builder.Services.AddAuthentication(Options =>
{
	Options.DefaultAuthenticateScheme =
	JwtBearerDefaults.AuthenticationScheme;
	Options.DefaultChallengeScheme =
	JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(Options =>
	{
		Options.Authority = builder.Configuration["AuthorizationServer:Authority"];
		Options.Audience = builder.Configuration["AuthorizationServer:Audience"];
	});



builder.Services.AddAuthorization(options =>
{
	
		options.AddPolicy("weatherapi.read", policy =>
			policy.RequireClaim("scope", "weatherapi.read"));

	options.AddPolicy("weatherapi.write", policy =>
		policy.RequireClaim("scope", "weatherapi.write"));
});





//builder.Services.AddAuthorization(Options =>
//{
//	Options.AddPolicy("weatherapi.read", policy =>
//	policy.Requirements.Add(new ScopeRequirement(
//		"weatherapi.read",
//		builder.Configuration["AuthorizationServer: Authority"])));
//	Options.AddPolicy("weatherapi.write", policy =>
//	policy.Requirements.Add(new ScopeRequirement(
//		"weatherapi.write",
//		builder.Configuration["AuthorizationServer:Authority"])));
//	Options.AddPolicy("weatherapi.read weatherapi.write", policy =>
//	policy.Requirements.Add(new ScopeRequirement(
//		"weatherapi.read weatherapi.write",
//		builder.Configuration["AuthorizationServer:Authority"])));

//});


//builder.Services.AddAuthentication(Options =>
//{
//	Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(Options =>
//{
//	Options.Authority = builder.Configuration["AuthorizationServer:Authority"];
//	Options.Audience = builder.Configuration["AuthorizationServer:Audience"];

//	// Aquí se agrega la política de autorización para el esquema JWT
//	Options.TokenValidationParameters = new TokenValidationParameters
//	{
//		ValidateIssuer = true,
//		ValidateAudience = true,
//		// ... otros parámetros de validación del token JWT
//	};

//	// Agregar la autenticación basada en políticas
//	//Options.Events = new JwtBearerEvents
//{
//	OnTokenValidated = async context =>
//	{
//		var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

//		var readPolicy = await authService.AuthorizeAsync(context.HttpContext.User, null, "weatherapi.read");
//		var writePolicy = await authService.AuthorizeAsync(context.HttpContext.User, null, "weatherapi.write");

//		if (!readPolicy.Succeeded || !writePolicy.Succeeded)
//		{
//			// Manejar el escenario en el que el usuario no cumple con las políticas requeridas
//			context.Fail("Acceso no autorizado");
//		}
//	}
//	//};
//});


//// Resto de tu código para registrar ScopeHandler y políticas de autorización
//builder.Services.AddAuthorization(options =>
//{
//	options.AddPolicy("weatherapi.read", policy =>
//	{
//		policy.Requirements.Add(new ScopeRequirement("api://weatherapi/weatherapiread", "your_issuer_here"));
//	});

//	options.AddPolicy("weatherapi.write", policy =>
//	{
//		policy.Requirements.Add(new ScopeRequirement("api://weatherapi/weatherapi.write", "your_issuer_here"));
//	});

//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
