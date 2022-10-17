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
            Latitude= 22.664552,
            Longitude= 88.340285,
            CreatedUpdatedDate=DateTime.Now
        };

        Location location_Two = new Location()
        {
            LocationId = Guid.NewGuid(),
            LocationName = "Test_Two",
            City = "City2",
            Email = "email2@gmail.com",
            Latitude = 22.644112,
            Longitude = 88.324062,
            CreatedUpdatedDate = DateTime.Now
        };

        [Test]
        public async Task AddLocation_LocationOne_CheckValueFromInMemoryDB()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locationDB").Options;

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
            List<Location> expectedList = new List<Location>() { location_one, location_Two };
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                        .UseInMemoryDatabase(databaseName: "temp_locationDB").Options;

            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                await _repository.AddLocationAsync(location_one);
                await _repository.AddLocationAsync(location_Two);
            }

            //act
            List<Location> actualList;
            using (var context = new ApplicationDBContext(options))
            {
                var _repository = new LocationService(context);
                var test = await _repository.GetLocationsAsync();
                actualList = test.ToList();
            }


            //assert
            CollectionAssert.AreEqual(expectedList, actualList, new LocationCompare());
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
