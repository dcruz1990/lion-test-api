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
using Microsoft.AspNetCore.Http;

namespace TestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
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

        /// <summary>
        /// List all Orders
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /all
        /// </remarks>
        /// <returns>All the ordes on the API</returns>
        /// <response code="200">Returns all orders</response>
        /// <response code="401">If the user isnt administrator</response> 
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllOrders(int UserId)
        {
            if (UserId == 1 || UserId == 2)
            {

                var allOrders = await _repo.ListAllOrders();

                return Ok(allOrders);
            }

            return Unauthorized();


        }

        /// <summary>
        /// Create a new buy Order
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /new 
        ///     {
        ///        "productsDetails": [
        ///           {
        ///             "ammount": 10,
        ///             "productId": 2
        ///          }
        ///          ],
        ///     "userId": 5,
        ///     "date": "2021-03-11T23:45:24.486Z",
        ///     "state": "string",
        ///     "ammount": 0
        ///    }
        /// </remarks>
        /// <returns>The created order</returns>
        /// <response code="201">Returns the created order</response>
        /// <response code="400">If there isnt stock for a given product</response> 
        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Edit an order
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /1/edit 
        /// </remarks>
        /// <returns>A status code 204</returns>
        /// <response code="204">No content</response>
        /// <response code="400">If there was a problem editing the order on db</response> 
        /// <response code="401">If the user hasnt access to edit</response> 
        [HttpPut("{orderId}/edit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Delete orders, only by admins if the status isnt Confirmed
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /1/delete 
        /// </remarks>
        /// <returns>A status code 204</returns>
        /// <response code="204">No content</response>
        /// <response code="400">If there was a problem editing the order on db</response> 
        /// <response code="401">If the user hasnt access to edit</response> 
        [HttpDelete("{orderId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int UserId, int orderId)
        {
            if (UserId != 1)
            {
                return Unauthorized();
            }

            var order = await _OrderDal.GetById(orderId);

            if (order.State != "Confirmed")
            {
                if (await _OrderDal.Delete(order))
                {
                    return NoContent();
                }

                return BadRequest("There was an issue on DB , we cannot delete that order");

            }

            return BadRequest("That order was confirmed, sorry, we cant delete it!");



        }
    }


}




