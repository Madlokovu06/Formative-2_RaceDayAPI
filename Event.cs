
namespace RaceDayAPI.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public required string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public required string Description { get; set; }

        public decimal Distance { get; set; }

        public required string EventType { get; set; }

        public decimal Entry { get; set; }

        public int LocationID { get; set; }

        public int UserID { get; set; }
    }
}
