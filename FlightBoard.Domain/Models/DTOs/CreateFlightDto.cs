using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBoard.Domain.Models.DTOs
{
    public class CreateFlightDto
    {
        public string FlightNumber { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public string Gate { get; set; } = null!;
    }
}
