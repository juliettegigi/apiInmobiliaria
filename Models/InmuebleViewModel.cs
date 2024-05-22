namespace Models;
public class InmuebleViewModel
{
    public int InmuebleTipoId { get; set; }
    public string? Direccion { get; set; }
    public int CantidadAmbientes { get; set; }
    public TipoUso Uso { get; set; }
    public decimal PrecioBase { get; set; }
    public decimal CLatitud { get; set; }
    public decimal CLongitud { get; set; }
    public bool Suspendido { get; set; }
    public bool Disponible { get; set; }
    public List<IFormFile> Imagenes { get; set; }
}