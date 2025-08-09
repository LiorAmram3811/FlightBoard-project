using AutoMapper;
using FlightBoard.Domain.Models;
using FlightBoard.Domain.Models.DTOs;

namespace FlightBoard.API.Mapping
{
    // using AutoMapper
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<Flight, FlightDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateFlightDto, Flight>();
        }
    }
}
