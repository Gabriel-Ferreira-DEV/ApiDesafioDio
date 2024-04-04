using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Context
{
    public class OrganizadorContext : DbContext
    {
        public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>(ConfigureSuaEntidade);
        }

        private void ConfigureSuaEntidade(EntityTypeBuilder<Tarefa> builder)
        {
            // Mapeamento do enum para string
            builder.Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumStatusTarefa)Enum.Parse(typeof(EnumStatusTarefa), v));

           
        }
    }
}