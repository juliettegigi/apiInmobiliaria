using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using MailKit.Net.Smtp;
using MimeKit;

using Models;
using Models.DAO;
using proyecto1.Clases;


namespace proyecto1.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]  /* este decorador es para establecer la ruta, indica que la ruta es "/elControlador" . lo q está entre corchetes es una variable
cómo llego acá?  "la url propia del dominio"/ "nombre del controlador" es decir "whetherforestcast"
y cómo sabe que método tiene q ejecutar?  cuando sea httpGet ==> este anotador  [HttpGet(Name="getWeatherForecast")]  solo dice el name, no dice parte de la ruta ==> ejecuta ese método, el Get, el método puede tener cualquier nombre, esto es una convennción, el que define quien ejecuta es la ruta
*/
public class PropietarioController : ControllerBase  // acá los controladores heredan de ControllerBase
{

    private readonly DataContext contexto;
	private readonly IConfiguration config;
	private readonly IWebHostEnvironment environment;
	private Password password;
	private JWT jwt;

    public PropietarioController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
			password=new Password(config["Salt"]);
			jwt=new JWT(config["TokenAuthentication:SecretKey"],config["TokenAuthentication:Issuer"],config["TokenAuthentication:Audience"]);
		}

   





/* ------------------------------------------------------------------------------------------------------------------------------- inmuebles del usuario logueado */

[HttpGet("misInmuebles")]
    public async Task<IActionResult> GetMisInmuebles()
    {
        try
        {
            int userId =int.Parse(User.Identity.Name);
			List<Inmueble> inmuebles = await contexto.Propietarios
            .Where(p => p.Id == userId)
            .Include(p => p.Inmuebles)
                .ThenInclude(i => i.InmuebleTipo)
            .Include(p => p.Inmuebles)
                .ThenInclude(i => i.Imagenes)
            .SelectMany(p => p.Inmuebles)
            .Select(i => new Inmueble
            {
                Id = i.Id,
                PropietarioId = i.PropietarioId,
                InmuebleTipoId = i.InmuebleTipoId,
                Direccion = i.Direccion,
                CantidadAmbientes = i.CantidadAmbientes,
                Uso = i.Uso,
                PrecioBase = i.PrecioBase,
                CLatitud = i.CLatitud,
                CLongitud = i.CLongitud,
                Suspendido = i.Suspendido,
                Disponible = i.Disponible,
                InmuebleTipo = i.InmuebleTipo == null ? null : new InmuebleTipo
                {
                    Id = i.InmuebleTipo.Id,
                    Tipo = i.InmuebleTipo.Tipo
                },
                Imagenes = i.Imagenes == null ? null : i.Imagenes.Select(img => new ImagenInmueble
                {
                    Id = img.Id,
                    Imagen = img.Imagen
                }).ToList()
            })
            .ToListAsync();

        return Ok(inmuebles);
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, $"Error al obtener el inmueble: {ex.Message}");
        }
    }







/* --------------------------------------------------------------------------------------------------------------------------------- get Propietario, el q tiene el token */

	[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{   int userId =int.Parse(User.Identity.Name);
			    Propietario p=await contexto.Propietarios.FindAsync(userId);
				p.Pass=null;
				return Ok(p);

				//return await contexto.Propietarios.SingleOrDefaultAsync(x => x.Id == userId);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}




/*--------------------------------------------------------------------------------------------------------------------- getPropietarioById*/

//http://localhost:5000/Usuario?id=2
[HttpGet("{id}")]  
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				Propietario entidad = await contexto.Propietarios.FindAsync(id);	
				entidad.NoPass();
				return Ok(entidad);
				//return entidad != null ? Ok(new JsonResult(entidad)) : NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



	/******************************************************************************************************************************post propietario*/

	[HttpPost]
	[AllowAnonymous]
		public async Task<IActionResult> Post([FromForm] Propietario entidad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					entidad.Pass=password.HashPassword(entidad.Pass);
					await contexto.Propietarios.AddAsync(entidad);
					contexto.SaveChanges();
					return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

    /************************************************************************************************ PUT  el Propietario loggueado*/

		[HttpPut]
	public async Task<IActionResult> Put([FromBody] Propietario entidad)
	{  
	   
	   
		try
		{ 
		    int userId =int.Parse(User.Identity.Name);
		       	//busco el propietario que quiero editar
			    Propietario propietario=await contexto.Propietarios.AsNoTracking().FirstOrDefaultAsync(p=> p.Id ==userId);
			    entidad.Id=propietario.Id;
				
			    if(!string.IsNullOrEmpty(entidad.Pass)){
				   entidad.Pass=password.HashPassword(entidad.Pass);
			    }
				else entidad.Pass=propietario.Pass;
				if(string.IsNullOrEmpty(entidad.Nombre))entidad.Nombre=propietario.Nombre;
				if(string.IsNullOrEmpty(entidad.Apellido))entidad.Apellido=propietario.Apellido;
				if(string.IsNullOrEmpty(entidad.DNI))entidad.DNI=propietario.DNI;
				if(string.IsNullOrEmpty(entidad.Telefono))entidad.Telefono=propietario.Telefono;
				if(string.IsNullOrEmpty(entidad.Email))
					 entidad.Email=propietario.Email;
			    else {
                   // si quieren actualizar el email==> tengo que checkear que el email no exista
                   Propietario p=await contexto.Propietarios.AsNoTracking().FirstOrDefaultAsync(p=>p.Email==entidad.Email && p.Id !=propietario.Id);
                   if(p!=null)return BadRequest("El email ingresado pertenece a otro usuario.");
				}
				  
				
			
				
				contexto.Propietarios.Update(entidad);
				await contexto.SaveChangesAsync();
				return Ok(entidad);
              

		}
		catch (Exception ex)
		{
			
			return BadRequest(ex.Message);
		}
	}


    /************************************************************************************************ logeo, JWT*/

    	[HttpPost("login")]
		[AllowAnonymous]
		//[FromBody] Propietario entidad, [Required, FromBody] string pass

		public async Task<IActionResult> Login([FromForm] LoginView loginView)// objeto con usuario y Pass
		{  

			  if (!ModelState.IsValid){
            return BadRequest(ModelState);
        }

			try
			{ 
				/*hasheo la pass igresada para poder compararla con la pass de la DB*/                

				var p = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (p == null || !password.EsIgual(loginView.Pass,p.Pass))
				{
					return BadRequest("Nombre de usuario o Pass incorrecta");
				}
				else
				{
					
					return Ok(new JwtSecurityTokenHandler().WriteToken( jwt.GenerarToken(p.Id,180)));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

/*---------------------------------------------------------------------------------------------------------        enviar el enlace al correo    */

[HttpPost("email")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByEmail([FromForm] string email)
		{
			try
			{ 
				var entidad = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == email);
				if(entidad==null)
				   return BadRequest("El email ingresado no existe."); 
				 

				   
				//var dominio = environment.IsDevelopment() ? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() : "www.misitio.com";
				//var dominio = environment.IsDevelopment() ? config["AppSettings:DevelopmentDomain"] : config["AppSettings:ProductionDomain"];
				var dominio= GetLocalIpAddress()+":5000";

				if(await Mensaje.EnviarEnlace(entidad,"Restablecer contraseña. ",config,environment,jwt,dominio)){
					
					return Ok("Se ha enviado un enlace de restablecimiento de contraseña a su correo electrónico.");
				}

				return StatusCode(500, "Hubo un problema al enviar el correo de restablecimiento. Inténtelo nuevamente más tarde.");
			}
			catch (Exception ex)
			{   
				return BadRequest(ex.Message);
			}
		}


 [HttpGet("resetearpass")]
        public IActionResult ResetearPass([FromQuery] string access_token)
        {
            var redirectUrl = $"myapp://nueva_pass?access_token={access_token}";
            return Redirect(redirectUrl);
        }

/*--------------------------------------------------------------------------------------------------------------------------  enviar contraseña*/		
	[HttpGet("token")]
		public async Task<IActionResult> Token()
		{
			
			try
			{ //este método si tiene autenticación, al entrar, generar clave aleatorio y enviarla por correo

			     int userId =int.Parse(User.Identity.Name);
			    Propietario propietario=await contexto.Propietarios.FirstOrDefaultAsync(p=> p.Id ==userId);

				
				string nuevaPass=await Mensaje.EnviarCodigo(propietario,"Codigo. ",config,password);
				if(String.IsNullOrEmpty(nuevaPass)){
                    return StatusCode(500, "Hubo un problema al enviar código.");
				} 
				propietario.Pass=nuevaPass;
				await contexto.SaveChangesAsync();
				return Ok("Contraseña actualizada exitosamente.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("pass")]
		public async Task<IActionResult> ActualizarPass([FromForm] string nuevaPass)
		{
			
			try
			{ 

			     int userId =int.Parse(User.Identity.Name);
			    Propietario propietario=await contexto.Propietarios.FirstOrDefaultAsync(p=> p.Id ==userId);

				
				propietario.Pass=password.HashPassword(nuevaPass);
				await contexto.SaveChangesAsync();
				return Ok(new JwtSecurityTokenHandler().WriteToken( jwt.GenerarToken(propietario.Id,180)));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



private string GetLocalIpAddress()
        {
            string localIp = null;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            return localIp;
        }

}

