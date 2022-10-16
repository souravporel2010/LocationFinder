using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationFinder.Services.DTO
{
    public class LocationDTO
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
