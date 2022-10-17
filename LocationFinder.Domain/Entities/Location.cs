using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationFinder.Domain.Entities
{
    public class Location
    {
        [Key]
        public Guid LocationId { get; set; }
        [Required]
        public string LocationName { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        public DateTime CreatedUpdatedDate { get; set; }= DateTime.Now;
    }
}
