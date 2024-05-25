using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Models;

[Table("Inquilinos")]
public class Inquilino
{
    [Key]
    public int Id { get; set; }

    public string DNI { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Telefono { get; set; }

    public string Email { get; set; }

    public string Domicilio { get; set; }
}


