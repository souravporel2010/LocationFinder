using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LocationFinder.Domain.Entities;
using LocationFinder.Domain.Interfaces;
using LocationFinder.Domain.Others;
using LocationFinder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LocationFinder.Infrastructure.Repositories
{
    public class LocationService : ILocation
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public LocationService(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext=applicationDBContext;
        }

        public async Task<IEnumerable<Location>> FindLocationWithinRadiusAsync(double originlatitude, double originlongitude, double searchRadius)
        {
            List<Location> finalLocationList= new List<Location>();
            double kmFactor = 0.009009; // 1 km = 0.009009 lat/ long convertion
            double approxSearchRadius = searchRadius + 2;

            double originMaxLat = originlatitude + (approxSearchRadius * kmFactor);
            double originMinLat= originlatitude - (approxSearchRadius * kmFactor);
            double originMaxLong= originlongitude + (approxSearchRadius * kmFactor);
            double originMinLong = originlongitude - (approxSearchRadius * kmFactor);        

            //get locations within approx square radius ( search radius +2 km) 
            List<Location> approxLocs =await _applicationDBContext.Locations.Where(x => x.Latitude <= originMaxLat
                            && x.Latitude >= originMinLat
                            && x.Longitude <= originMaxLong
                            && x.Longitude >= originMinLong
                            ).ToListAsync();

            finalLocationList =  approxLocs.Where(x => Math.Abs(DistanceBetweenCoOrdinate(originlatitude, originlongitude, x.Latitude, x.Longitude)) <= searchRadius).ToList();

            return finalLocationList;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            var locations = await  _applicationDBContext.Locations.ToListAsync();
            return locations;
        }

        public double DistanceBetweenCoOrdinate(double lat1,
            double lon1,
            double lat2,
            double lon2)
        {
            //6,357
            double earthRadius = 6357;

            double toRadians = 0.017453292519943295; // To Radians Conversion Factor

            double dlon = toRadians * (lon2 - lon1);
            double dlat = toRadians * (lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(toRadians * (lat1)) * Math.Cos(toRadians * (lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * earthRadius;
        }

        public async Task<LocationRepositoryResponse> AddLocationAsync(Location location)
        {
            LocationRepositoryResponse locationRepositoryResponse = new LocationRepositoryResponse();
            try { 
                await _applicationDBContext.Locations.AddAsync(location);
                await _applicationDBContext.SaveChangesAsync();
                locationRepositoryResponse.Status = true;
                locationRepositoryResponse.Message = "Succcess";
                locationRepositoryResponse.location = location;
                return locationRepositoryResponse;
            }
            catch (Exception)
            {
                locationRepositoryResponse.Status = false;
                locationRepositoryResponse.Message = "Add Location Failure";
                locationRepositoryResponse.location = location;
                return locationRepositoryResponse;
            }
            
        }

        public async Task<LocationRepositoryResponse> DeleteLocationAsync(Guid guid)
        {
            LocationRepositoryResponse locationRepositoryResponse = new LocationRepositoryResponse();
            try
            {
                var loc = await _applicationDBContext.Locations.FindAsync(guid);
                if (loc != null)
                {
                    _applicationDBContext.Locations.Remove(loc);
                    await _applicationDBContext.SaveChangesAsync();
                    locationRepositoryResponse.Status = true;
                    locationRepositoryResponse.Message = "Succcess";
                    return locationRepositoryResponse;
                }
                else
                {
                    locationRepositoryResponse.Status = false;
                    locationRepositoryResponse.Message = "Location Not Found";
                    return locationRepositoryResponse;
                }
            }
            catch (Exception)
            {
                locationRepositoryResponse.Status = false;
                locationRepositoryResponse.Message = "Location Delete Failure";
                return locationRepositoryResponse;
            }
            
        }

        public async Task<IEnumerable<Location>> FindAsync(Expression<Func<Location, bool>> expression)
        {
            return await _applicationDBContext.Locations.Where(expression).ToListAsync();
        }

        public async Task<LocationRepositoryResponse> UpdateLocationAsync(Guid guid, Location location)
        {
            LocationRepositoryResponse locationRepositoryResponse = new LocationRepositoryResponse();
            locationRepositoryResponse.location = location;
            try
            {
                if (guid != location.LocationId)
                {
                    locationRepositoryResponse.Message = "LocationId doesn't match with Payload";
                    locationRepositoryResponse.Status = false;
                    return locationRepositoryResponse;
                }
                var loc = await _applicationDBContext.Locations.Where(x => x.LocationId == guid).FirstOrDefaultAsync();
                if (loc == null)
                {
                    locationRepositoryResponse.Message = "Location Not Found";
                    locationRepositoryResponse.Status = false;
                    return locationRepositoryResponse;
                }
                else
                {
                    loc.LocationName = location.LocationName;
                    loc.Email = location.Email;
                    loc.City = location.City;
                    loc.Latitude = location.Latitude;
                    loc.Longitude = location.Longitude;
                    loc.CreatedUpdatedDate = DateTime.Now;
                    await _applicationDBContext.SaveChangesAsync();

                    locationRepositoryResponse.Message = "Success";
                    locationRepositoryResponse.Status = true;
                    return locationRepositoryResponse;
                }
            }
            catch (Exception)
            {
                locationRepositoryResponse.Message = "Location Update Failed";
                locationRepositoryResponse.Status = false;
                return locationRepositoryResponse;
            }
            
        }


    }
}
