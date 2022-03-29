namespace Core.Domain
{
    public class Location : EntityBase
    {
        public string City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public ICollection<Player>? Players { get; set; }
        public List<PlayerLocation> PlayerLocations { get; set; }
    }
}
