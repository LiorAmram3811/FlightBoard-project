using FlightBoard.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose: Validates the Flight model for correct and required fields.
namespace FlightBoard.Application.Validation
{
    public class FlightValidator : AbstractValidator<Flight>
    {
        public FlightValidator()
        {
            RuleFor(x => x.FlightNumber)
                .NotEmpty().WithMessage("Flight number is required.");

            RuleFor(x => x.Destination)
                .NotEmpty().WithMessage("Destination is required.");

            RuleFor(x => x.Gate)
                .NotEmpty().WithMessage("Gate is required.");

            RuleFor(x => x.DepartureTime)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Departure time must be in the future.");
        }
    }
}
