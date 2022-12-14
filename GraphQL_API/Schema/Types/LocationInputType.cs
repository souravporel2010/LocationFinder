namespace GraphQL_API.Schema.Types
{
    public class LocationInputType
    {
        public string LocationName { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
