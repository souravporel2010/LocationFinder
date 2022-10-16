using LocationFinder.Domain.Entities;
using LocationFinder.Domain.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LocationFinder.Domain.Interfaces
{
    public interface ILocation
    {
        public Task<IEnumerable<Location>> GetLocationsAsync();

        public Task<IEnumerable<Location>> FindLocationWithinRadiusAsync(double originlatitude, double originlongitude, double searchRadius);

        public Task<IEnumerable<Location>> FindAsync(Expression<Func<Location, bool>> expression);

        public Task<LocationRepositoryResponse> AddLocationAsync(Location location);

        public Task<LocationRepositoryResponse> UpdateLocationAsync(Guid guid, Location location);

        public Task<LocationRepositoryResponse> DeleteLocationAsync(Guid guid);

    }
}
