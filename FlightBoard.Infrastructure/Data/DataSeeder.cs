using FlightBoard.Domain.Models;
using FlightBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

// Purpose: Seeds initial flights at runtime if the DB is empty.
namespace FlightBoard.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(FlightDbContext db, CancellationToken ct = default)
        {
            if (await db.Flights.AnyAsync(ct)) return;

            var now = DateTime.UtcNow;
            var seed = new List<Flight>
            {
                new Flight { FlightNumber = "LY100", Destination = "New York",  DepartureTime = now.AddHours(2), Gate = "A1" },
                new Flight { FlightNumber = "AF202", Destination = "Paris",     DepartureTime = now.AddHours(3), Gate = "B2" },
                new Flight { FlightNumber = "BA300", Destination = "London",    DepartureTime = now.AddHours(4), Gate = "C3" },
                new Flight { FlightNumber = "EK400", Destination = "Dubai",     DepartureTime = now.AddHours(5), Gate = "D4" },
                new Flight { FlightNumber = "LH500", Destination = "Frankfurt", DepartureTime = now.AddHours(6), Gate = "E5" },
            };

            await db.Flights.AddRangeAsync(seed, ct);
            await db.SaveChangesAsync(ct);
        }
    }
}
