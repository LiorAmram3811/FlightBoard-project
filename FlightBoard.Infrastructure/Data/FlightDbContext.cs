using FlightBoard.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose: Entity Framework Core database context for managing Flight entities with SQLite.
namespace FlightBoard.Infrastructure.Data
{
    public class FlightDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; } = null!;

        public FlightDbContext(DbContextOptions<FlightDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasIndex(f => f.FlightNumber)
                .IsUnique(); // Ensure flight number is unique in DB

            base.OnModelCreating(modelBuilder);
        }
    }
}
