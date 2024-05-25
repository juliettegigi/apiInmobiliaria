using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models{


 [Table("Inmuebles")]
public class Inmueble
{
    [Key]
    public int Id { get; set; }
    public int PropietarioId { get; set; }
    public int InmuebleTipoId { get; set; }
    public string? Direccion { get; set; }
    public int CantidadAmbientes { get; set; }
   [Column(TypeName = "string")]
    public TipoUso Uso { get; set; }
    public decimal PrecioBase { get; set; }
    public decimal CLatitud { get; set; }
    public decimal CLongitud { get; set; }
    public bool Suspendido { get; set; }
    public bool Disponible { get; set; }

    [ForeignKey("PropietarioId")]

    public Propietario? Propietario { get; set; }

    [ForeignKey("InmuebleTipoId")]
   
    public InmuebleTipo? InmuebleTipo { get; set; }    
public ICollection<ImagenInmueble>? Imagenes { get; set; }
public ICollection<Contrato> Contratos { get; set; }

}


public enum TipoUso
{
    Comercial=1,
    Residencial=2
}
}

/*




using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models




  [Table("Inmuebles")]
public class Inmueble
{
    public int Id { get; set; }
    public int PropietarioId { get; set; }
    public int InmuebleTipoId { get; set; }
    public string Direccion { get; set; }
    public int CantidadAmbientes { get; set; }
    public TipoUso Uso { get; set; }
    public decimal PrecioBase { get; set; }
    public decimal CLatitud { get; set; }
    public decimal CLongitud { get; set; }
    public bool Suspendido { get; set; }
    public bool Disponible { get; set; }

    public Propietario Propietario { get; set; }
    public InmuebleTipo InmuebleTipo { get; set; }
    public ICollection<ImagenInmueble> Imagenes { get; set; }
}

public enum TipoUso
{
    Comercial,
    Residencial
}



*/


/*


{
    [Table("Inmuebles")]
    public class Inmueble
    {
        [Key]
        public int Id { get; set; }

        // Esta propiedad es la clave foránea para la relación con Propietarios
      public int PropietarioId { get; set; }
		[ForeignKey(nameof(PropietarioId))]
		public Propietario? Duenio { get; set; }
        

        public string? Direccion { get; set; }

        // Clave foránea para la relación con InmuebleTipo
        public int InmuebleTipoId { get; set; }


        public int CantidadAmbientes { get; set; }
        public bool Suspendido { get; set; }
        public bool Disponible { get; set; }
        public decimal PrecioBase { get; set; }
		
		[Column("uso")]
        public TipoUso Uso { get; set; }

        [NotMapped]
        public Coordenada? Coordenadas { get; set; }

        public decimal CLatitud { get; set; }
        public decimal CLongitud { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, PropietarioId: {PropietarioId}, Dirección: {Direccion}, Tipo: {InmuebleTipoId}, Cantidad de Ambientes: {CantidadAmbientes}, Uso: {Uso}, Coordenadas: {Coordenadas}, Precio: {PrecioBase}";
        }
    }

    public enum TipoUso
    {
        Comercial = 0,
        Residencial = 1
    }
}






*/