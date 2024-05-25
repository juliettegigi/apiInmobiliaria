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

public class ContratoController : ControllerBase  
{

    private readonly DataContext contexto;
	private readonly IConfiguration config;
	private readonly IWebHostEnvironment environment;
	private Password password;
	private JWT jwt;

    public ContratoController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
			password=new Password(config["Salt"]);
			jwt=new JWT(config["TokenAuthentication:SecretKey"],config["TokenAuthentication:Issuer"],config["TokenAuthentication:Audience"]);
		}


/*----------------------------------------------------------------------------------------------------------------------- get inmuebles con contraro*/
/* [HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    try
    {
       
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
 */


}