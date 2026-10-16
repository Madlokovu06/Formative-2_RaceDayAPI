namespace RaceDayAPI.Models
{
    public class Location
    {
        public int LocationID { get; set; }

        public required string Address { get; set; }

        public required string City { get; set; }

        public required string VenueName { get; set; }
        public required string Province { get; set; }
    }
}