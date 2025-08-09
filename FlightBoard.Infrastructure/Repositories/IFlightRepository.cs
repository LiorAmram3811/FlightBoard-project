using FlightBoard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose: Interface for flight CRUD operations in the data layer.
namespace FlightBoard.Infrastructure.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllAsync();
        Task<Flight?> GetByIdAsync(int id);
        Task AddAsync(Flight flight);
        Task DeleteAsync(int id);
        Task<IEnumerable<Flight>> SearchAsync(string? status, string? destination);
    }
}
