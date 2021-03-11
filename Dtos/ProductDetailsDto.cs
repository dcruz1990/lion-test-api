
using System.ComponentModel.DataAnnotations;


namespace TestWebApi.Dtos
{

    public class ProductDetailsDto
    {

        [Required]
        public int Ammount { get; set; }

        [Required]
        public int ProductId { get; set; }

    }
}