namespace RaceDayAPI.Models
{
    public class Result
    {
        public int ResultID { get; set; }

        public int RegistrationID { get; set; }

        public TimeSpan FinishTime { get; set; }

        public int Position { get; set; }

        public required string ResultStatus { get; set; }
    }
}