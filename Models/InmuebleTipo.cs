using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("InmuebleTipos")]
public class InmuebleTipo
{
    [Key]
    public int Id { get; set; }
    public string Tipo { get; set; }
   public ICollection<Inmueble>? Inmuebles { get; set; }
}

/*
namespace Models;
public class InmuebleTipo
{
    public int Id { get; set; }
    public string Tipo { get; set; }

    public ICollection<Inmueble> Inmuebles { get; set; }
} */