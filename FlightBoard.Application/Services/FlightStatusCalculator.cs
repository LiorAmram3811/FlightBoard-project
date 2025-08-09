using FlightBoard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose: Calculates the current status of a flight based on departure time.
namespace FlightBoard.Application.Services
{
    public class FlightStatusCalculator
    {
        public FlightStatus CalculateStatus(DateTime departureTime, DateTime now)
        {
            var minutesToDeparture = (departureTime - now).TotalMinutes;
            var minutesSinceDeparture = (now - departureTime).TotalMinutes;

            if (minutesToDeparture > 30)
                return FlightStatus.Scheduled;
            else if (minutesToDeparture <= 30 && minutesToDeparture > 0)
                return FlightStatus.Boarding;
            else if (minutesSinceDeparture <= 60 && minutesSinceDeparture >= 0)
                return FlightStatus.Departed;
            else
                return FlightStatus.Landed;
        }
    }
}
