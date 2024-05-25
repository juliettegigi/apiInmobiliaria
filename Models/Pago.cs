using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Models;

[Table("Pagos")]
public class Pago
{
    [Key]
    public int Id { get; set; }

    public int NumeroPago { get; set; }

    public int ContratoId { get; set; }
    [ForeignKey("ContratoId")]
    public Contrato Contrato { get; set; }

    public string Fecha { get; set; }

    public string FechaPago { get; set; }

    public string Importe { get; set; }
}
