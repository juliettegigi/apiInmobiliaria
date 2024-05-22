using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

using Models;
using Models.DAO;
using proyecto1.Clases;

namespace proyecto1.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]  

public class InmuebleTipoController : ControllerBase  
{

    private readonly DataContext contexto;
	private readonly IConfiguration config;
	private readonly IWebHostEnvironment environment;
	private Password password;
	private JWT jwt;

    public InmuebleTipoController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
			password=new Password(config["Salt"]);
			jwt=new JWT(config["TokenAuthentication:SecretKey"],config["TokenAuthentication:Issuer"],config["TokenAuthentication:Audience"]);
		}


/*----------------------------------------------------------------------------------------------------------------------- get  tipos de inmueble*/

 

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var tipos = await contexto.InmuebleTipos.Select(it=>new{Id=it.Id,Tipo=it.Tipo}).ToListAsync();
            if (tipos == null)
            {
                return NotFound($"no inmuebleTipos."); 
            }
            return Ok(tipos);
        }
        catch (Exception ex)
        {
            // En caso de error, devolver una respuesta 500 Internal Server Error con el mensaje de error
            return StatusCode(500, $"Error al obtener el inmueble: {ex.Message}");
        }
    }


 


}