using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;


 [Table("Contratos")]
public class Contrato
{
     [Key]
    public int Id { get; set; }

    public int InmuebleId { get; set; }
    [ForeignKey("InmuebleId")]
    public Inmueble Inmueble { get; set; }

    public int InquilinoId { get; set; }
    [ForeignKey("InquilinoId")]
    public Inquilino Inquilino { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaFinAnticipada { get; set; } 
    public decimal PrecioXmes { get; set; }
    public bool Estado { get; set; }
    public ICollection<Pago> Pagos { get; set; }
        
        
}