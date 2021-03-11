using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models
{
    public class ProductDetailOnOrder : BaseEntity
    {
        [Required]
        public int Ammount { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Order Order { get; set; }





    }
}