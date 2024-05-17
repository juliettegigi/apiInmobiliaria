using Microsoft.EntityFrameworkCore;

namespace Models.DAO;

/*	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<Propietario> Propietarios { get; set; }
		public DbSet<Inquilino> Inquilinos { get; set; }
		public DbSet<Inmueble> Inmuebles { get; set; }

		public DbSet<Persona> Personas { get; set; }
		public DbSet<Pasatiempo> Pasatiempos { get; set; }
		public DbSet<PersonaPasatiempo> PersonaPasatiempos { get; set; }

		public DbSet<Conectado> Conectados { get; set; } 
		public DbSet<Propietario> Propietarios { get; set; }
		public DbSet<Inmueble> Inmuebles { get; set; }
		public DbSet<InmuebleTipo> InmuebleTipos { get; set; }
	}

*/

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<InmuebleTipo> InmuebleTipos { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; }
    public DbSet<ImagenInmueble> ImagenesInmuebles { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración del enum TipoUso
        modelBuilder.Entity<Inmueble>()
            .Property(i => i.Uso)
            .HasConversion<string>();
    }

    /* protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inmueble>()
            .HasOne(i => i.Propietario)
            .WithMany(p => p.Inmuebles)
            .HasForeignKey(i => i.PropietarioId);

        modelBuilder.Entity<Inmueble>()
            .HasOne(i => i.InmuebleTipo)
            .WithMany()
            .HasForeignKey(i => i.InmuebleTipoId);

        modelBuilder.Entity<ImagenInmueble>()
            .HasOne(ii => ii.Inmueble)
            .WithMany(i => i.Imagenes)
            .HasForeignKey(ii => ii.InmuebleId);
    } */
}
 