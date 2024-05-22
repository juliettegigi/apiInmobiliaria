using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // para usar CookieAuthenticationDefaults
using Models.DAO;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000","https://localhost:5001", "http://*:5000", "https://*:5001");
var configuration = builder.Configuration;
// Add services to the container.

// añadimos auth a los servicios. indicamos que las cookies es el método de auth de la app.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>//la api web valida con token
	{
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = configuration["TokenAuthentication:Issuer"],
			ValidAudience = configuration["TokenAuthentication:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
				configuration["TokenAuthentication:SecretKey"])),
		};
		// opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
//hay peticiones q no tienen encabezado como una url, enlaces, sources img

		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				// Leer el token desde el query string
				var accessToken = context.Request.Query["access_token"];
				// Si el request es para el Hub u otra ruta seleccionada...
				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) &&
					(path.StartsWithSegments("/chatsegurohub") ||
					path.StartsWithSegments("/api/propietarios/reset") ||
					path.StartsWithSegments("/Propietario/token")))
				{//reemplazar las urls por las necesarias ruta ⬆
					context.Token = accessToken;
				}
				return Task.CompletedTask;
			}
		};
	});



builder.Services.AddAuthentication();

builder.Services.AddAuthorization(options =>
{
	
	options.AddPolicy("Administrador", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador"));
});


builder.Services.AddControllers().AddJsonOptions(options =>{
      // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
	 //  options.JsonSerializerOptions.DictionaryKeyPolicy = null;
		 /* options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
        options.JsonSerializerOptions.IgnoreNullValues = true; */
		}
		); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(
	options => options.UseMySql(
		configuration["ConnectionStrings:DefaultConnection"],
		ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"]),
		options => options.UseMicrosoftJson()
	)
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseStaticFiles();
app.UseRouting();	

app.UseAuthorization();

app.MapControllers();

app.Run();
