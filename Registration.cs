
namespace RaceDayAPI.Models
{
    public class Registration
    {
        public int RegistrationID { get; set; }

        public int UserID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public DateTime RegistrationDate { get; set; }

        public required string Status { get; set; }
    }
}
