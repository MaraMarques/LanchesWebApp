using Azure;
using LanchesWebApp2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LanchesWebApp2.Data
{
    public class DbBitesContext : DbContext
    {
        public DbBitesContext(DbContextOptions<DbBitesContext> options) : base(options) { }

        public virtual DbSet<Ingrediente> Ingredientes { get; set;}
        public virtual DbSet<Lanche> Lanches { get; set;}  
        public virtual DbSet<LancheIngrediente> LancheIngredientes { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Lanche>()
                .HasMany(e => e.Ingredientes)
                .WithMany(e => e.Lanches)
                .UsingEntity<LancheIngrediente>(
                l => l.HasOne<Ingrediente>(e => e.Ingrediente).WithMany(e => e.LancheIngredientes).HasForeignKey(e => e.IngredienteId).OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne<Lanche>(e => e.Lanche).WithMany(e => e.LancheIngredientes).HasForeignKey(e => e.LancheId).OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Ingrediente>()
               .HasMany(e => e.Lanches)
               .WithMany(e => e.Ingredientes)
               .UsingEntity<LancheIngrediente>(
                l => l.HasOne<Lanche>(e => e.Lanche).WithMany(e => e.LancheIngredientes).HasForeignKey(e => e.LancheId).OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne<Ingrediente>(e => e.Ingrediente).WithMany(e => e.LancheIngredientes).HasForeignKey(e => e.IngredienteId).OnDelete(DeleteBehavior.Restrict));

        }
    }
}
