using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Models;

[Table("Propietarios")]
public class Propietario
{
  [Key]
        public int Id { get; set; }
        public string? DNI { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Pass { get; set; }
        public string? Domicilio { get; set; }

       
        public ICollection<Inmueble>? Inmuebles { get; set; }



public string NoPass(){
		 this.Pass=null;
		 var jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore // Ignorar propiedades nulas
        };

        // Serializa la entidad usando Newtonsoft.Json
        var json = JsonConvert.SerializeObject(this, jsonSettings);
		return json;
	}

}









/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Models;

[Table("Propietarios")]
public class Propietario
{
    public int Id { get; set; }
    public string Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Pass { get; set; }
    public string Domicilio { get; set; }

    public ICollection<Inmueble> Inmuebles { get; set; }
}



*/


/*public class Propietario
{   [Key]
	public int Id { get; set; }
	public string? Nombre { get; set; }
	public string? Apellido { get; set; }
	public String? DNI {get;set;}

	//[Display(Name = "Teléfono")]
	public String? Telefono  { get; set; }
	 
    public String? Email { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public String? Pass { get; set; }

	public override string ToString()
		{
			//return $"{Apellido} \n {Nombre} \n {Email},";
			return $"{Apellido} {Nombre}  {Email}  {Pass}";
		}

	public string NoPass(){
		 this.Pass=null;
		 var jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore // Ignorar propiedades nulas
        };

        // Serializa la entidad usando Newtonsoft.Json
        var json = JsonConvert.SerializeObject(this, jsonSettings);
		return json;
	}

	
}*/