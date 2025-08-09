using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose: Represents a flight and its main properties in the system.
namespace FlightBoard.Domain.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public string Gate { get; set; } = null!;

        // Status is calculated on the server, not stored in DB.
        [NotMapped]
        public FlightStatus Status { get; set; }
    }

    public enum FlightStatus
    {
        Scheduled,
        Boarding,
        Departed,
        Landed
    }
}
