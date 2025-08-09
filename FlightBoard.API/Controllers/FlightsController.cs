using AutoMapper;
using FlightBoard.API.Hubs;
using FlightBoard.Application.Services;
using FlightBoard.Application.Validation;
using FlightBoard.Domain.Models;
using FlightBoard.Domain.Models.DTOs;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;

// Purpose: Exposes REST API endpoints for managing flights and triggering SignalR updates.
namespace FlightBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightRepository _repository;
        private readonly FlightStatusCalculator _statusCalculator;
        private readonly FlightValidator _validator;
        private readonly IHubContext<FlightHub> _hubContext;
        private readonly IMapper _mapper;

        public FlightsController(IFlightRepository repository, FlightStatusCalculator statusCalculator, FlightValidator validator, IHubContext<FlightHub> hubContext, IMapper mapper)
        {
            _repository = repository;
            _statusCalculator = statusCalculator;
            _validator = validator;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAll()
        {
            var flights = await _repository.GetAllAsync();
            var now = DateTime.UtcNow;

            foreach (var flight in flights)
                flight.Status = _statusCalculator.CalculateStatus(flight.DepartureTime, now);

            var flightDtos = _mapper.Map<IEnumerable<FlightDto>>(flights);
            return Ok(flightDtos);
        }

        [HttpPost]
        public async Task<ActionResult<FlightDto>> Create([FromBody] CreateFlightDto dto)
        {
            var flight = _mapper.Map<Flight>(dto);
            var validationResult = _validator.Validate(flight);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            // Unique check...
            var exists = (await _repository.GetAllAsync()).Any(f => f.FlightNumber == flight.FlightNumber);
            if (exists)
                return Conflict("Flight number must be unique.");

            await _repository.AddAsync(flight);
            flight.Status = _statusCalculator.CalculateStatus(flight.DepartureTime, DateTime.UtcNow);

            var flightDto = _mapper.Map<FlightDto>(flight);

            await _hubContext.Clients.All.SendAsync("FlightAdded", flightDto);

            return CreatedAtAction(nameof(GetAll), new { id = flightDto.Id }, flightDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _repository.GetByIdAsync(id);
            if (flight == null)
                return NotFound();

            await _repository.DeleteAsync(id);

            // Broadcast via SignalR
            await _hubContext.Clients.All.SendAsync("FlightDeleted", id);

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Flight>>> Search([FromQuery] string? status, [FromQuery] string? destination)
        {
            var flights = await _repository.SearchAsync(status, destination);
            var now = DateTime.UtcNow;

            var filtered = flights.Select(f =>
            {
                f.Status = _statusCalculator.CalculateStatus(f.DepartureTime, now);
                return f;
            });

            if (!string.IsNullOrEmpty(status) &&
                Enum.TryParse<FlightStatus>(status, true, out var statusEnum))
            {
                filtered = filtered.Where(f => f.Status == statusEnum);
            }

            return Ok(filtered);
        }
    }
}
