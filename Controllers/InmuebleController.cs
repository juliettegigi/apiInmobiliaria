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


/*----------------------------------------------------------------------------------------------------------------------- post inmueble del prop logueado*/




[HttpPost]
public async Task<IActionResult> Post([FromForm] InmuebleViewModel model)
{
    try
    {
        if (!ModelState.IsValid)
				return BadRequest();
        int userId = int.Parse(User.Identity.Name);
        var propietario = await contexto.Propietarios.FindAsync(userId);
        var inmuebleTipo = await contexto.InmuebleTipos.FindAsync(model.InmuebleTipoId);

        if (propietario == null || inmuebleTipo == null)
        {
            return BadRequest("Propietario o Tipo de Inmueble no válido.");
        }


        
        var entidad = new Inmueble
        {
            PropietarioId = userId,
            InmuebleTipoId = model.InmuebleTipoId,
            Direccion = model.Direccion,
            CantidadAmbientes = model.CantidadAmbientes,
            Uso = model.Uso,
            PrecioBase = model.PrecioBase,
            CLatitud = model.CLatitud,
            CLongitud = model.CLongitud,
            Suspendido = model.Suspendido,
            Disponible = model.Disponible,
            Propietario = propietario,
            InmuebleTipo = inmuebleTipo,
            Imagenes = model.Imagenes==null?null:new List<ImagenInmueble>()
        };

       
         if (model.Imagenes != null && model.Imagenes.Count > 0)
            {
                //string path = environment.ContentRootPath;
                string path = environment.WebRootPath;
                if (string.IsNullOrEmpty(path))
                {
                    return StatusCode(500, "WebRootPath is not configured.");
                }

                string uploadPath = Path.Combine(path, "Uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var file in model.Imagenes)
                {
                    if (file.Length>0)
                    {   string nombreUnico= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath=Path.Combine(uploadPath,nombreUnico);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                          {
                              await file.CopyToAsync(stream);
                          }
                           var imagenInmueble = new ImagenInmueble {
                               Imagen = Path.Combine("Uploads", nombreUnico), // Ruta relativa
                               Inmueble = entidad
                    };

                    entidad.Imagenes.Add(imagenInmueble);
                    }
                
                   

                   
                }
            }

        

        // Guardar el inmueble y las imágenes en la base de datos
        await contexto.Inmuebles.AddAsync(entidad);
        await contexto.SaveChangesAsync();

        return Ok(model);
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
            var inmueble = await contexto.Inmuebles
            .Include(i => i.InmuebleTipo)
            .Include(i => i.Propietario)            
            .Include(i => i.Imagenes)
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
                    InmuebleId = img.InmuebleId,
                    Imagen = img.Imagen
                }).ToList()
            })            
           .FirstOrDefaultAsync(i => i.Id == id);


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





 
 /*----------------------------------------------------------------------------------------------------------------------- get inmuebles del propietario logueado*/
   [HttpGet("bypropietario")]
    public async Task<IActionResult> GetByPropietario()
    {
        try
        {  int userId =int.Parse(User.Identity.Name);








            List<Inmueble> inmuebles = await contexto.Inmuebles
            .Where(i => i.PropietarioId == userId)
            .Include(p => p.InmuebleTipo)
            .Include(e => e.Propietario)
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
            // En caso de error, devolver una respuesta 500 Internal Server Error con el mensaje de error
            return StatusCode(500, $"Error al obtener el inmueble: {ex.Message}");
        }
    }







 
 /*----------------------------------------------------------------------------------------------------------------------- get inmuebles con contrato del propietario logueado*/
   [HttpGet("inmconcontrato")]
    public async Task<IActionResult> GetByContrato()
    {

        
        try
        {  int userId =int.Parse(User.Identity.Name);

           var inmueblesQuery = await contexto.Inmuebles
                               .Where(inmueble => inmueble.PropietarioId == userId && inmueble.Contratos.Any(c => c.Estado))
                               .Include(inmueble => inmueble.Contratos)
                                   .ThenInclude(contrato => contrato.Inquilino)
                               .Include(inmueble => inmueble.Contratos)
                                   .ThenInclude(contrato => contrato.Pagos) 
                               .Include(inmueble => inmueble.InmuebleTipo)
                               .Include(inmueble => inmueble.Imagenes)
                               .ToListAsync();

var inmuebles = inmueblesQuery.Select(i => new Inmueble
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
    }).ToList(),
    Contratos = i.Contratos == null ? null : i.Contratos.Select(c => new Contrato
    {
        Id = c.Id,
        InquilinoId = c.InquilinoId,
        InmuebleId = c.InmuebleId,
       /*  FechaInicio = c.FechaInicio,
        FechaFin = c.FechaFin,
        FechaFinAnticipada = c.FechaFinAnticipada, */
        PrecioXmes = c.PrecioXmes,
        Estado = c.Estado,
        Inquilino = c.Inquilino == null ? null : new Inquilino
        {
            Id = c.Inquilino.Id,
            DNI = c.Inquilino.DNI,
            Nombre = c.Inquilino.Nombre,
            Apellido = c.Inquilino.Apellido,
            Telefono = c.Inquilino.Telefono,
            Email = c.Inquilino.Email,
            Domicilio = c.Inquilino.Domicilio
        },
         Pagos = c.Pagos == null ? null : c.Pagos.Select(p => new Pago
        {
            Id = p.Id,
            NumeroPago = p.NumeroPago,
            ContratoId = p.ContratoId,
            Fecha = p.Fecha,
            FechaPago = p.FechaPago,
            Importe = p.Importe
        }).ToList()
    }).ToList()
}).ToList();

        
       
           
            return Ok(inmuebles);
            }
    
        catch (Exception ex)
        {
            // En caso de error, devolver una respuesta 500 Internal Server Error con el mensaje de error
            return StatusCode(500, $"Error al obtener el inmueble: {ex.Message}");
        }
    }






[HttpPut("{id}")]
public async Task<IActionResult> cambiarSuspendido(int id){

    int userId =int.Parse(User.Identity.Name);
    var inmueble = await contexto.Inmuebles
        .Where(i => i.Id == id && i.PropietarioId == userId)
        .Include(inmueble => inmueble.Contratos)
                                   .ThenInclude(contrato => contrato.Inquilino)
                               .Include(inmueble => inmueble.Contratos)
                                   .ThenInclude(contrato => contrato.Pagos) 
                               .Include(inmueble => inmueble.InmuebleTipo)
                               .Include(inmueble => inmueble.Imagenes)
        .FirstOrDefaultAsync();

         if (inmueble == null)
    {
        return BadRequest("Inmueble incorrecto");
    }        
   
    
    inmueble.Suspendido=!inmueble.Suspendido;
    contexto.Entry(inmueble).Property(i => i.Suspendido).IsModified = true;
    await contexto.SaveChangesAsync();

    // Transforma el inmueble en el objeto deseado para devolverlo
    var inmueble2 = new Inmueble
    {
        Id = inmueble.Id,
        PropietarioId = inmueble.PropietarioId,
        InmuebleTipoId = inmueble.InmuebleTipoId,
        Direccion = inmueble.Direccion,
        CantidadAmbientes = inmueble.CantidadAmbientes,
        Uso = inmueble.Uso,
        PrecioBase = inmueble.PrecioBase,
        CLatitud = inmueble.CLatitud,
        CLongitud = inmueble.CLongitud,
        Suspendido = inmueble.Suspendido,
        Disponible = inmueble.Disponible,
        InmuebleTipo = inmueble.InmuebleTipo == null ? null : new InmuebleTipo
        {
            Id = inmueble.InmuebleTipo.Id,
            Tipo = inmueble.InmuebleTipo.Tipo
        },
        Imagenes = inmueble.Imagenes == null ? null : inmueble.Imagenes.Select(img => new ImagenInmueble
        {
            Id = img.Id,
            Imagen = img.Imagen
        }).ToList(),
        Contratos = inmueble.Contratos == null ? null : inmueble.Contratos.Select(c => new Contrato
        {
            Id = c.Id,
            InquilinoId = c.InquilinoId,
            InmuebleId = c.InmuebleId,
            PrecioXmes = c.PrecioXmes,
            Estado = c.Estado,
            Inquilino = c.Inquilino == null ? null : new Inquilino
            {
                Id = c.Inquilino.Id,
                DNI = c.Inquilino.DNI,
                Nombre = c.Inquilino.Nombre,
                Apellido = c.Inquilino.Apellido,
                Telefono = c.Inquilino.Telefono,
                Email = c.Inquilino.Email,
                Domicilio = c.Inquilino.Domicilio
            },
            Pagos = c.Pagos == null ? null : c.Pagos.Select(p => new Pago
            {
                Id = p.Id,
                NumeroPago = p.NumeroPago,
                ContratoId = p.ContratoId,
                Fecha = p.Fecha,
                FechaPago = p.FechaPago,
                Importe = p.Importe
            }).ToList()
        }).ToList()
    };

    
    return Ok(inmueble2);



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