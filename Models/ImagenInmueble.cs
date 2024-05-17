
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

 [Table("ImagenInmuebles")]
public class ImagenInmueble
{
    [Key]
    public int Id { get; set; }
    public int InmuebleId { get; set; }
    public string Imagen { get; set; }

    [ForeignKey("InmuebleId")]
    public Inmueble Inmueble { get; set; }
}