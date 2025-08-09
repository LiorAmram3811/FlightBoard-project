using FlightBoard.Application.Validation;
using FlightBoard.Domain.Models;
using System;
using Xunit;

// Purpose: Unit tests for FlightValidator to ensure business rules are enforced.
namespace FlightBoard.Tests
{
    public class FlightValidatorTests
    {
        private readonly FlightValidator _validator = new FlightValidator();

        [Fact]
        public void Should_Pass_For_Valid_Flight()
        {
            var flight = new Flight
            {
                FlightNumber = "LY001",
                Destination = "London",
                DepartureTime = DateTime.UtcNow.AddHours(2),
                Gate = "A1"
            };
            var result = _validator.Validate(flight);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Fail_When_FlightNumber_Missing()
        {
            var flight = new Flight
            {
                FlightNumber = "",
                Destination = "London",
                DepartureTime = DateTime.UtcNow.AddHours(2),
                Gate = "A1"
            };
            var result = _validator.Validate(flight);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "FlightNumber");
        }

        [Fact]
        public void Should_Fail_When_DepartureTime_NotInFuture()
        {
            var flight = new Flight
            {
                FlightNumber = "LY002",
                Destination = "Paris",
                DepartureTime = DateTime.UtcNow.AddMinutes(-1),
                Gate = "B1"
            };
            var result = _validator.Validate(flight);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "DepartureTime");
        }
    }
}
