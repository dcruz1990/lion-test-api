using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models
{
    public class ProductForCreateDto
    {
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public int Ammount { get; set; }
        public string Slug { get; set; }
        [Required]
        public float Price { get; set; }

    }
}