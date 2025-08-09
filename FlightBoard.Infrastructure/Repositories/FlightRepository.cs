using FlightBoard.Domain.Models;
using FlightBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose: Concrete repository that implements CRUD for flights using EF Core.
namespace FlightBoard.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightDbContext _context;

        public FlightRepository(FlightDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _context.Flights.AsNoTracking().ToListAsync();
        }

        public async Task<Flight?> GetByIdAsync(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task AddAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Flight>> SearchAsync(string? status, string? destination)
        {
            var query = _context.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(destination))
                query = query.Where(f => f.Destination.Contains(destination));

            // Status is calculated in app layer.
            // here, just fetch all and filter later
            return await query.AsNoTracking().ToListAsync();
        }
    }
}
