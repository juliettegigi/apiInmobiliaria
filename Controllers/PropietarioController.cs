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

   




/* --------------------------------------------------------------------------------------------------------------------------------- get Propietario, el q tiene el token */

	[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{   int userId =int.Parse(User.Identity.Name);
				return await contexto.Propietarios.FindAsync(userId);
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
				var entidad = await contexto.Propietarios.FindAsync(id);	
				entidad.NoPass();
				return entidad != null ? Ok(new JsonResult(entidad)) : NotFound();
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
				   Console.WriteLine("pass ",entidad.Pass);
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
					
					return Ok(new JwtSecurityTokenHandler().WriteToken( jwt.GenerarToken(p.Id)));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}




}



/*  const {nombre,correo,password,rol}=req.body;// nos van a pasar al usuario
  const usuario= new Usuario({nombre,correo,password,rol});
  console.log(usuario);
  const salt=bcryptjs.genSaltSync();
  usuario.password=bcryptjs.hashSync(password,salt);
  await usuario.save();
 
  res.json({msg:'post API-controlador',
            usuario});*/

/* 
			public class ApiClient {
    private static final String URL = "http://192.168.1.2:5000/";
    private static MisEndPoints mep;

    public static MisEndPoints getEndPoints(){
        Gson gson = new GsonBuilder().setLenient().create();
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(URL)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();
        mep = retrofit.create(MisEndPoints.class);
        return mep;
    }

    public interface MisEndPoints {
        @FormUrlEncoded
        @POST("Propietarios/login")
        Call<String> login(@Field("Usuario") String u, @Field("Clave") String c);
    }
}



public class LoginActivityViewModel extends AndroidViewModel {
    public LoginActivityViewModel(@NonNull Application application) {
        super(application);
    }

    public void logueo(String usuario, String clave){
        ApiClient.MisEndPoints api = ApiClient.getEndPoints();
        Call<String> call = api.login(usuario, clave);
        call.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String> call, Response<String> response) {
                if(response.isSuccessful()){
                    Log.d("salida", response.body());
                } else {
                    Log.d("salida", "Incorrecto");
                }
            }

            @Override
            public void onFailure(Call<String> call, Throwable throwable) {
                Log.d("salida", "Falla");
            }
        });
    }
} */