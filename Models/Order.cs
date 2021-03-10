using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models
{
    public class Order : BaseEntity
    {
        [Required]
        public ICollection<Product> Products { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        public string State { get; set; }

        public int Ammount { get; set; }


    }
}