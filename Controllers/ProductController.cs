using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;


using TestWebApi.Models;
using TestWebApi.Services;
using TestWebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace coding.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRepository<Product> _ProductDal;


        public ProductController(IConfiguration config, IRepository<Product> productDal)
        {
            _config = config;
            _ProductDal = productDal;

        }


        /// <summary>
        /// List all products
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /all
        /// </remarks>
        /// <returns>All the products on the API</returns>
        /// <response code="200">Returns all products</response>

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _ProductDal.ListAsync();

            return Ok(allProducts);

        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /new 
        ///            {
        ///        "name": "Bread",
        ///        "description": "A fresh bread!",
        ///        "ammount": 200,
        ///        "slug": "bread",
        ///        "price": 2
        ///        }
        /// </remarks>
        /// <returns>The created product</returns>
        /// <response code="201">Returns the created product</response>
        /// <response code="401">If the user dont have the given role to post products</response> 
        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> NewProduct(ProductForCreateDto newProduct, int UserId)
        {
            // Check user Roles If user isnt seller or admin
            if (UserId == 1 || UserId == 2)
            {

                // Create new product object, we can use mappers, but stick to the legacy code here
                var ForCreate = new Product()
                {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Slug = newProduct.Slug,
                    Price = newProduct.Price,
                    Ammount = newProduct.Ammount,
                    // Hardcode user relation, the user 2 is Product owner
                    UserId = UserId

                };

                // Dummy validation of roles.
                if (UserId == 1 || UserId == 2)
                {
                    // Make a call to Data Access Layer to create the product on DB.
                    var CreatedProduct = await _ProductDal.Create(ForCreate);

                    // We may add some validation to check if the product where created correctly
                    // Return the product
                    return Created("", CreatedProduct);
                }

            }

            // Cant create
            return Unauthorized();

        }

        /// <summary>
        /// Edit product if the user owns it or is admin
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /1/edit 
        /// </remarks>
        /// <returns>A status code 204</returns>
        /// <response code="204">No content</response>
        /// <response code="400">If there was a problem editing the order on db</response> 
        /// <response code="400">If the user hasnt ownership of the product</response> 
        /// <response code="401">If the user hasnt access to edit</response> 

        [HttpPut("{productId}/edit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EditProduct(int UserId, int productId, ProductForUpdateDto updatedData)
        {
            if (UserId == 1 || UserId == 2)
            {

                // Get the product from the db
                var product = await _ProductDal.GetById(productId);

                // Dummy user validation
                if (product.UserId == UserId)
                {

                    // Dummy assignement of properties, we can use mappers to save lines
                    product.Ammount = updatedData.Ammount;
                    product.Description = updatedData.Description;
                    product.Name = updatedData.Name;
                    product.Price = updatedData.Price;
                    product.Slug = updatedData.Slug;

                    // Call the DAL to update the product info
                    var updated = await _ProductDal.Update(product);

                    // Return the REST Verb according to the operation of update
                    return NoContent();
                }


                return BadRequest("Sorry, that product doesnt belong to you");

            }

            // Cant create
            return Unauthorized();

        }

        /// <summary>
        /// Delete product, only by admins or owner
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /1/delete 
        /// </remarks>
        /// <returns>A status code 204</returns>
        /// <response code="204">No content</response>
        [HttpDelete("{productId}/delete")]
        public async Task<IActionResult> Delete(int UserId, int productId)
        {
            // Check user Roles If user isnt seller or admin
            if (UserId == 1 || UserId == 2)
            {
                // Get the product
                var productForDelete = await _ProductDal.GetById(productId);

                // Delete the product
                var deleted = await _ProductDal.Delete(productForDelete);

                return NoContent();
            }

            return Unauthorized();

        }










    }
}