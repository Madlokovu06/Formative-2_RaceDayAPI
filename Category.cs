
namespace RaceDayAPI.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public required string CategoryName { get; set; }

        public required string Description { get; set; }

        public int EventID { get; set; }
    }
}
