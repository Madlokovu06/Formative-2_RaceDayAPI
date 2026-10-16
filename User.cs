namespace RaceDayAPI.Models
{
    public class User
    {
        public int UserID { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }  
        public required string PhoneNumber { get; set; }

        public required string Role { get; set; }

        public string? PasswordHash { get; set; }
    }
}