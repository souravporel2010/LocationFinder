using AutoMapper;
using GraphQL_API.Schema.Types;
using LocationFinder.Domain.Interfaces;
using LocationFinder.Domain.Others;
using Location = LocationFinder.Domain.Entities.Location;

namespace GraphQL_API.Schema.Mutation
{
    public class LocationMutation
    {
        private readonly ILocation _locationService;
        private readonly IMapper _mapper;
        public LocationMutation(ILocation location, IMapper mapper)
        {
            _locationService = location;
            _mapper = mapper;
        }
        public async Task<LocationType> AddLocationAsync(LocationInputType locationInputType)
        {
            Location location = _mapper.Map<Location>(locationInputType);
            location.LocationId = Guid.NewGuid();
            location.CreatedUpdatedDate = DateTime.Now;
            LocationRepositoryResponse response =await _locationService.AddLocationAsync(location);
            if (response.Status==false)
            {
                throw new GraphQLException(new Error(response.Message));
            }

            LocationType locationType = _mapper.Map<LocationType>(response.location);
            return locationType;
        }

        public async Task<bool> DeleteLocation(Guid guid)
        {
            LocationRepositoryResponse response = await _locationService.DeleteLocationAsync(guid);
            if (response.Status==false)
            {
                throw new GraphQLException(new Error(response.Message));
            }
            return response.Status;
        }

        public async Task<LocationType> UpdateLocation(Guid guid, LocationType locationInput)
        {
            Location location = _mapper.Map<Location>(locationInput);
            LocationRepositoryResponse response = await _locationService.UpdateLocationAsync(guid, location);
            if (response.Status==false)
            {
                throw new GraphQLException(new Error(response.Message));
            }
            return locationInput;

        }

    }
}
