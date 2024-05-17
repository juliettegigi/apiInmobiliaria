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

public class InmuebleController : ControllerBase  
{

    private readonly DataContext contexto;
	private readonly IConfiguration config;
	private readonly IWebHostEnvironment environment;
	private Password password;
	private JWT jwt;

    public InmuebleController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
			password=new Password(config["Salt"]);
			jwt=new JWT(config["TokenAuthentication:SecretKey"],config["TokenAuthentication:Issuer"],config["TokenAuthentication:Audience"]);
		}


/*--------------------------------------------------------------------------------------------------------------------------------------------- post inmueble del prop logueado*/

         [HttpPost]
public async Task<IActionResult> Post([FromBody] Inmueble entidad)
{
    try
    {
        // propietario y el tipo de inmueble  tiene q existir
       // var propietario = await contexto.Propietarios.FindAsync(entidad.PropietarioId);
       int userId =int.Parse(User.Identity.Name);
       var propietario = await contexto.Propietarios.FindAsync(userId);
        var inmuebleTipo = await contexto.InmuebleTipos.FindAsync(entidad.InmuebleTipoId);

        if (propietario == null || inmuebleTipo == null)
        {
            return BadRequest("Propietario o Tipo de Inmueble no válido.");
        }

        
        entidad.Propietario = propietario;
        entidad.InmuebleTipo = inmuebleTipo;

        await contexto.Inmuebles.AddAsync(entidad);
        await contexto.SaveChangesAsync();

        //return Ok(entidad);
        return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
    
 /*---------http://localhost:5000/Inmueble/7------------------------------------------------------------------------------------------ get by id inmueble*/
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var inmueble = await contexto.Inmuebles.FindAsync(id);
            if (inmueble == null)
            {
                return NotFound($"Inmueble con Id {id} no encontrado.");
            }
            return Ok(inmueble);
        }
        catch (Exception ex)
        {
            // En caso de error, devolver una respuesta 500 Internal Server Error con el mensaje de error
            return StatusCode(500, $"Error al obtener el inmueble: {ex.Message}");
        }
    }


 /*--------------------------------------------------------------------------------------------------------------------------------------------- get inmuebles del propietario logueado*/
    [HttpGet("bypropietario")]
    public async Task<IActionResult> GetByPropietarioId()
    {
        try
        {
            
            int userId =int.Parse(User.Identity.Name);
          //  List<Inmueble> inmuebles = await contexto.Inmuebles.Where(a => a.PropietarioId == userId).ToListAsync();
            //contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Id == userId)
            return Ok(contexto.Inmuebles.Include(e => e.Imagenes).Include(e => e.Propietario).Where(e => e.Propietario.Id == userId));
        }
        catch (Exception ex)
        {
            // En caso de error, devolver una respuesta 500 Internal Server Error con el mensaje de error
            return StatusCode(500, $"Error al obtener el inmueble: {ex.Message}");
        }
    }





}