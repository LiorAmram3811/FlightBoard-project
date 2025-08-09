using AutoMapper;
using FlightBoard.API.Controllers;
using FlightBoard.API.Hubs;
using FlightBoard.Application.Services;
using FlightBoard.Application.Validation;
using FlightBoard.Domain.Models;
using FlightBoard.Domain.Models.DTOs;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

// Purpose: Unit test for FlightsController using Moq for dependencies.
namespace FlightBoard.Tests
{
    public class FlightsControllerTests
    {
        [Fact]
        public async Task GetAll_Returns_Flights_With_Calculated_Status()
        {
            // Arrange
            var mockRepo = new Mock<IFlightRepository>();
            var mockHub = new Mock<IHubContext<FlightHub>>();
            var mockMapper = new Mock<IMapper>();
            var statusCalc = new FlightStatusCalculator();
            var validator = new FlightValidator();

            var sampleFlights = new List<Flight>
            {
                new Flight
                {
                    Id = 1,
                    FlightNumber = "LY999",
                    Destination = "Tel Aviv",
                    // Use < 30 mins to assert "Boarding"; use > 30 mins to assert "Scheduled".
                    DepartureTime = DateTime.UtcNow.AddMinutes(10),
                    Gate = "C3"
                }
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sampleFlights);

            // IMPORTANT: tell the mapper what to return
            mockMapper
                .Setup(m => m.Map<IEnumerable<FlightDto>>(It.IsAny<IEnumerable<Flight>>()))
                .Returns((IEnumerable<Flight> flights) =>
                    flights.Select(f => new FlightDto
                    {
                        Id = f.Id,
                        FlightNumber = f.FlightNumber,
                        Destination = f.Destination,
                        DepartureTime = f.DepartureTime,
                        Gate = f.Gate,
                        Status = f.Status.ToString()
                    }).ToList()
                );

            var controller = new FlightsController(
                mockRepo.Object, statusCalc, validator, mockHub.Object, mockMapper.Object
            );

            // Act
            var result = await controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<FlightDto>>(ok.Value);
            var list = dtos.ToList();

            Assert.Single(list);
            Assert.Equal("Boarding", list[0].Status); // 10 min -> Boarding
        }
    }
}
