using FlightBoard.Application.Services;
using FlightBoard.Domain.Models;
using FluentAssertions;
using System;
using Xunit;

// Purpose: Unit tests for FlightStatusCalculator to verify status calculation business logic.
namespace FlightBoard.Tests
{
    public class FlightStatusCalculatorTests
    {
        private readonly FlightStatusCalculator _calculator = new FlightStatusCalculator();

        [Fact]
        public void Should_Return_Scheduled_When_DepartureMoreThan30Minutes()
        {
            var now = DateTime.UtcNow;
            var result = _calculator.CalculateStatus(now.AddMinutes(60), now);
            result.Should().Be(FlightStatus.Scheduled);
        }

        [Fact]
        public void Should_Return_Boarding_When_Within30MinutesToDeparture()
        {
            var now = DateTime.UtcNow;
            var result = _calculator.CalculateStatus(now.AddMinutes(10), now);
            result.Should().Be(FlightStatus.Boarding);
        }

        [Fact]
        public void Should_Return_Departed_When_Within60MinutesAfterDeparture()
        {
            var now = DateTime.UtcNow;
            var result = _calculator.CalculateStatus(now.AddMinutes(-10), now);
            result.Should().Be(FlightStatus.Departed);
        }

        [Fact]
        public void Should_Return_Landed_When_MoreThan60MinutesAfterDeparture()
        {
            var now = DateTime.UtcNow;
            var result = _calculator.CalculateStatus(now.AddMinutes(-70), now);
            result.Should().Be(FlightStatus.Landed);
        }
    }
}
