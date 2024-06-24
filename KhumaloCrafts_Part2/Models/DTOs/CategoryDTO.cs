using System.ComponentModel.DataAnnotations;

namespace KhumaloCrafts_Part2.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }
    }
}
