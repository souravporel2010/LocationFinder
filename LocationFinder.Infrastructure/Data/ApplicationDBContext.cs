using LocationFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationFinder.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Location>(entity =>
            //    entity.HasKey(e => e.LocationId).IsClustered(true)
            //);
            //modelBuilder.Entity<Location>(entity =>
            //    entity.HasKey(e => e.Latitude).IsClustered(false)
            //);
            //modelBuilder.Entity<Location>(entity =>
            //    entity.HasKey(e => e.Longitude).IsClustered(false)
            //);

            modelBuilder.Entity<Location>().HasData(new Location[]
            {
                new Location() { LocationId=Guid.NewGuid(), 
                    LocationName= "Dunder Mifflin Ghent",
                    City= "Ghent",
                    Email="ghent@dundermifflin.com",
                    Latitude=51.043476,
                    Longitude=3.709138,
                    CreatedUpdatedDate= DateTime.Now
                },
                new Location() { LocationId=Guid.NewGuid(),
                    LocationName= "Dunder Mifflin Kortrijk",
                    City= "Kortrijk",
                    Email="kortrijk@dundermifflin.com",
                    Latitude=50.822956,
                    Longitude=3.250962,
                    CreatedUpdatedDate= DateTime.Now
                },
                new Location() { LocationId=Guid.NewGuid(),
                    LocationName= "Dunder Mifflin Harelbeke",
                    City= "Harelbeke",
                    Email="harelbeke@dundermifflin.com",
                    Latitude=50.855366,
                    Longitude=3.312553,
                    CreatedUpdatedDate= DateTime.Now
                }
            });
        }
    }
}
