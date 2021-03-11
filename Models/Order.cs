using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models
{
    public class Order : BaseEntity
    {

        public User User { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public string State { get; set; }

        public float TotalAmmount { get; set; }

        public ICollection<ProductDetailOnOrder> ProductDetailOnOrder { get; set; }



    }
}