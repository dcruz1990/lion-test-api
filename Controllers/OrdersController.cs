using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;


using TestWebApi.Models;
using TestWebApi.Services;
using TestWebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using TestWebApi.Dto;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace coding.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRepository<Order> _OrderDal;

        private readonly IRepository<Product> _ProductDal;

        private readonly IOrderRepository<Order> _repo;



        public OrdersController(IConfiguration config, IRepository<Order> orderDal, IRepository<Product> productDal, IOrderRepository<Order> repo)
        {
            _config = config;
            _OrderDal = orderDal;
            _ProductDal = productDal;
            _repo = repo;

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders(int UserId)
        {
            if (UserId == 1 || UserId == 2)
            {

                var allOrders = await _repo.ListAllOrders();

                return Ok(allOrders);
            }

            return Unauthorized();


        }

        [HttpPost("new")]
        public async Task<IActionResult> BuySome(NewBuyOrderDto orderDetails)
        {
            var allAmount = new float();
            // Dummy product detail validation, if there is no stack then you cant buy!
            foreach (var product in orderDetails.ProductsDetails)
            {
                var prodFromDB = await _ProductDal.GetById(product.ProductId);

                allAmount += product.Ammount * prodFromDB.Price;

                if (prodFromDB.Ammount < product.Ammount)
                {
                    return BadRequest("There is no stack available for " + prodFromDB.Name);
                }
            }

            ICollection<ProductDetailOnOrder> allProductsDetails = new Collection<ProductDetailOnOrder>();
            //Clean Dtos
            foreach (var product in orderDetails.ProductsDetails)
            {
                var Detail = new ProductDetailOnOrder()
                {
                    ProductId = product.ProductId,
                    Ammount = product.Ammount
                };

                allProductsDetails.Add(Detail);
            }

            var OrderToCreate = new Order()
            {
                UserId = orderDetails.UserId,
                ProductDetailOnOrder = allProductsDetails,
                Date = orderDetails.Date,
                State = "Created",
                TotalAmmount = allAmount

            };

            var success = await _OrderDal.Create(OrderToCreate);

            return Created("", success);

        }

        [HttpPut("{orderId}/edit")]
        public async Task<IActionResult> EditOrder(string state, int UserId, int orderId)
        {
            if (UserId != 1)
            {
                return Unauthorized();
            }

            var order = await _OrderDal.GetById(orderId);

            order.State = state;

            if (await _OrderDal.Update(order))
            {
                return NoContent();
            }

            return BadRequest("For some reason we cannot edit that order");
        }

        [HttpDelete("{orderId}/delete")]
        public async Task<IActionResult> Delete(int UserId, int orderId)
        {
            if (UserId != 1)
            {
                return Unauthorized();
            }

            var order = await _OrderDal.GetById(orderId);

            if (await _OrderDal.Delete(order))
            {
                return NoContent();
            }

            return BadRequest("Cant delete that order");
        }
    }


}




