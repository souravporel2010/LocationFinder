using AutoMapper;
using GraphQL_API.Schema.Types;
using Location = LocationFinder.Domain.Entities.Location;

namespace GraphQL_API.Profiles
{
    public class MappingProfile : Profile   
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationType>().ReverseMap();
            CreateMap<Location, LocationInputType>().ReverseMap();
        }
    }
}
