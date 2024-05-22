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

public class ImagenInmuebleController : ControllerBase  
{

    private readonly DataContext contexto;
	private readonly IConfiguration config;
	private readonly IWebHostEnvironment environment;
	private Password password;
	private JWT jwt;

    public ImagenInmuebleController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
			password=new Password(config["Salt"]);
			jwt=new JWT(config["TokenAuthentication:SecretKey"],config["TokenAuthentication:Issuer"],config["TokenAuthentication:Audience"]);
		}


/*----------------------------------------------------------------------------------------------------------------------- get imagen by id*/
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    try
    {
        var imagen = await contexto.ImagenInmuebles.FirstOrDefaultAsync(i => i.Id == id);
         
        if (imagen == null)
        {
            return NotFound($"Imagen con Id {id} no encontrado.");
        }

        // Ruta completa del archivo de imagen
        string wwwPath = environment.WebRootPath;
	    string rutaCompleta = Path.Combine(wwwPath,imagen.Imagen);
        // Verifica si el archivo existe
        if (!System.IO.File.Exists(rutaCompleta))
        {
            return NotFound("Archivo de imagen no encontrado.");
        }
       string mimeType = GetMimeType(rutaCompleta);
        return PhysicalFile(rutaCompleta, mimeType);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}


private string GetMimeType(string path)
{
    var types = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
    {
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".bmp", "image/bmp" },
        { ".tiff", "image/tiff" },
        { ".svg", "image/svg+xml" }
    };

    var ext = Path.GetExtension(path);
    return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
}

}