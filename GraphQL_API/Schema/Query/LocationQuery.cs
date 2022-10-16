using AutoMapper;
using GraphQL_API.Schema.Types;
using LocationFinder.Domain.Interfaces;
using Location = LocationFinder.Domain.Entities.Location;

namespace GraphQL_API.Schema.Query
{
    public class LocationQuery
    {
        private readonly ILocation _locationService;
        private readonly IMapper _mapper;
        public LocationQuery([Service] ILocation location, IMapper mapper)
        {
            _locationService = location;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LocationType>> GetLocationsAsync()
        {
            IEnumerable<LocationType> locationTypeList= new List<LocationType>();
            IEnumerable<Location> locationList = await _locationService.GetLocationsAsync() ;

            if (locationList!=null && locationList.ToList().Count>0)
            {
                locationTypeList = _mapper.Map<IEnumerable<LocationType>>(locationList);
            }
            
            return locationTypeList;
        }
         
        public async Task<IEnumerable<LocationType>> GetLocationWithinRadiusAsync(double originlatitude, double originlongitude, double searchRadius)
        {
            IEnumerable<LocationType> locationTypeList = new List<LocationType>();
            IEnumerable<Location> locationList = await _locationService.FindLocationWithinRadiusAsync(originlatitude,originlongitude,searchRadius);

            if (locationList!=null && locationList.ToList().Count > 0)
            {
                locationTypeList = _mapper.Map<IEnumerable<LocationType>>(locationList);
            }
            return locationTypeList;
        }

    }
}
