using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KhumaloCrafts_Part2.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ArtisanName { get; set; }


        [Required]
        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
