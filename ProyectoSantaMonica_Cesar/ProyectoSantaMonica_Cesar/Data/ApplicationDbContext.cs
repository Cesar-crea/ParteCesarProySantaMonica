using Microsoft.EntityFrameworkCore;
using ProyectoSantaMonica_Cesar.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProyectoSantaMonica_Cesar.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options)
        {
        }

        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<HorariosAtencion> HorariosAtencion { get; set; }
        public DbSet<Cita> Citas { get; set; }

        public DbSet<ComprobanteDePago> ComprobantePago { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ComprobanteDePago>()
          .Property(c => c.Metodo_Pago)
             .HasConversion<string>();

            modelBuilder.Entity<ComprobanteDePago>()
                .Property(c => c.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Cita>()
                .Property(c => c.Estado)
                .HasConversion<string>();
        }
    
}
}
