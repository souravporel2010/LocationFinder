using System.ComponentModel.DataAnnotations;
namespace GraphQL_API.Schema.Types
{
    public class LocationType
    {
        public Guid LocationId { get; set; }
        [Required]
        [StringLength(100)]
        public string LocationName { get; set; }=String.Empty;
        [StringLength(50)]
        public string City { get; set; } = String.Empty;
        [EmailAddress]
        public string Email { get; set; }= String.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
