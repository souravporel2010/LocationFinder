using LocationFinder.Domain.Entities;
using LocationFinder.Infrastructure.Data;
using LocationFinder.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationFinder
{
    [TestFixture]
    public class LocationServiceTests
    {
        Location location_one= new Location()
        {
            LocationId= Guid.NewGuid(),
            LocationName="Test_One",
            City="City1",
            Email="email1@gmail.com",
            Latitude= 22.657704,
            Longitude= 88.334265,
            CreatedUpdatedDate=DateTime.Now
        };

        Location location_Two = new Location()
        {
            LocationId = Guid.NewGuid(),
            LocationName = "Test_Two",
            City = "City2",
            Email = "email2@gmail.com",
            Latitude = 22.592110,
            Longitude = 88.323413,
            CreatedUpdatedDate = DateTime.Now
        };

        Location location_Three = new Location()
        {
            LocationId = Guid.NewGuid(),
            LocationName = "Test_Three",
            City = "City3",
            Email = "email3@gmail.com",
            Latitude = 22.617822,
            Longitude = 88.324113,
            CreatedUpdatedDate = DateTime.Now
        };


        [Test]
        public async Task AddLocation_LocationOne_CheckValueFromInMemoryDB()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_addlocationDB").Options;

            //act
            using(var context= new ApplicationDBContext(options))
            {
                var _repository= new LocationService(context);
                await _repository.AddLocationAsync(location_one);
            }

            //assert
            using (var context = new ApplicationDBContext(options))
            {
                var location_one_fromDB = context.Locations.FirstOrDefault(x=>x.LocationId==location_one.LocationId);
                Assert.AreEqual(location_one.LocationId, location_one_fromDB?.LocationId);
                Assert.AreEqual(location_one.LocationName, location_one_fromDB?.LocationName);
                Assert.AreEqual(location_one.City, location_one_fromDB?.City);
                Assert.AreEqual(location_one.Email, location_one_fromDB?.Email);
                Assert.AreEqual(location_one.Latitude, location_one_fromDB?.Latitude);
                Assert.AreEqual(location_one.Longitude, location_one_fromDB?.Longitude);
                Assert.AreEqual(location_one.CreatedUpdatedDate, location_one_fromDB?.CreatedUpdatedDate);
            }
        }

        [Test]
        public async Task GetAllLocation_CheckBothBookingFromInMemoryDB()
        {
            //arrange
            List<Location> expectedList = new List<Location>() { location_one, location_Two, location_Three };
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locDB").Options;

            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                await _repository.AddLocationAsync(location_one);
                await _repository.AddLocationAsync(location_Two);
                await _repository.AddLocationAsync(location_Three);
            }

            //act
            List<Location> actualList;
            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                var test = await _repository.GetLocationsAsync();
                actualList = test.ToList();
                context.Dispose();
            }


            //assert
            CollectionAssert.AreEqual(expectedList, actualList, new LocationCompare());
        }

        [Test]
        public void DistanceBetweenTwoPoint_GetDistance()
        {
            double expectedDistance = 7.36;
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locationDB").Options;

            //Act
            double distance;
            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                distance = _repository.DistanceBetweenCoOrdinate(location_one.Latitude,
                    location_one.Longitude, location_Two.Latitude, location_Two.Longitude);
            }

            //assert
            Assert.AreEqual(expectedDistance, distance,.2);
        }

        [Test]
        public async Task FindLocationWithinRadiusAsync_GetProperList()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locationDB").Options;

            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                await _repository.AddLocationAsync(location_one);
                await _repository.AddLocationAsync(location_Two);
                await _repository.AddLocationAsync(location_Three);
            }

            //act
            int resultCount;
            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                var tempList = await _repository.FindLocationWithinRadiusAsync(location_one.Latitude,location_one.Longitude,5);
                resultCount = tempList.Count();
                context.Dispose();
            }

            //assert
            Assert.AreEqual(2,resultCount);
        }

        [Test]
        public async Task DeleteLocation_CheckValueFromInMemoryDB()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locDeleteDB").Options;

            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                await _repository.AddLocationAsync(location_one);
                await _repository.AddLocationAsync(location_Two);
                await _repository.AddLocationAsync(location_Three);
            }

            //act
            bool result;
            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                var test = await _repository.DeleteLocationAsync(location_one.LocationId);
                result= context.Locations.Where(x=>x.LocationId == location_one.LocationId).Any();
            }

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateLocation_CheckValueFromInMemoryDB()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locUpdateDB").Options;

            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                await _repository.AddLocationAsync(location_one);
                await _repository.AddLocationAsync(location_Two);
                await _repository.AddLocationAsync(location_Three);
            }

            //act
            Location updatedLocation;
            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                location_Three.Email = "location3Email@gmail.com";
                await _repository.UpdateLocationAsync(location_Three.LocationId,location_Three);
                updatedLocation = context.Locations.Where(x => x.LocationId == location_Three.LocationId).First();
            }

            //assert
            Assert.AreEqual("location3Email@gmail.com", updatedLocation.Email);
        }
    }

    public class LocationCompare : IComparer
    {
        public int Compare(object x, object y)
        {
            var locationOne = (Location)x;
            var locationTwo = (Location)y;
            if (locationOne.LocationId!= locationTwo.LocationId)
            {
                return 1;
            }
            else
            {
                return 0;
            }    
        }
    }
}
