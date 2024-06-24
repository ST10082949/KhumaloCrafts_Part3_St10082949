using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhumaloCrafts_Part2.Models
{
    
        [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

            [Required]
            [MaxLength(40)]
            public string CategoryName { get; set; }
            public List<Product> Products { get; set; }
        }
    }
