using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApi.Dtos;
using TestWebApi.Models;

namespace TestWebApi.Dto
{
    public class NewBuyOrderDto
    {
        [Required]
        public ICollection<ProductDetailsDto> ProductsDetails { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public string State { get; set; }

        public int Ammount { get; set; }


    }
}