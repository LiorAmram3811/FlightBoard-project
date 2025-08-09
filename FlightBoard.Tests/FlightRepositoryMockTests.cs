using FlightBoard.Domain.Models;
using FlightBoard.Infrastructure.Repositories;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

// Purpose: Shows how to mock IFlightRepository for use in higher-level tests.
namespace FlightBoard.Tests
{
    public class FlightRepositoryMockTests
    {
        [Fact]
        public async Task Should_Return_All_Flights()
        {
            var mockRepo = new Mock<IFlightRepository>();
            mockRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Flight>
                {
                    new Flight { Id = 1, FlightNumber = "LY123", Destination = "London", DepartureTime = System.DateTime.UtcNow.AddHours(1), Gate = "A1" }
                });

            var flights = await mockRepo.Object.GetAllAsync();

            Assert.Single(flights);
            Assert.Equal("LY123", ((List<Flight>)flights)[0].FlightNumber);
        }
    }
}
