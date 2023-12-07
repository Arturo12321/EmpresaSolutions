using Microsoft.EntityFrameworkCore;

namespace EmpresaSolutions.Models
{
    public class ContextoSolutions: DbContext
    {
        public ContextoSolutions(DbContextOptions<ContextoSolutions> options)
        : base(options)
        {
        }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Personal> Personales { get; set; }

    }
}
