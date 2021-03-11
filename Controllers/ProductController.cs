using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;


using TestWebApi.Models;
using TestWebApi.Services;
using TestWebApi.Dtos;
using Microsoft.AspNetCore.Authorization;

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



        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _ProductDal.ListAsync();

            return Ok(allProducts);

        }

        [HttpPost("new")]
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

        [HttpPut("{productId}/edit")]
        public async Task<IActionResult> EditProduct(int UserId, int productId, ProductForUpdateDto updatedData)
        {
            if (UserId == 1 || UserId == 2)
            {

                // Get the product from the db
                var product = await _ProductDal.GetById(productId);


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

            // Cant create
            return Unauthorized();

        }

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