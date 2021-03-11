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


        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _ProductDal.ListAsync();

            return Ok(allProducts);

        }

        [HttpPost("new")]
        public async Task<IActionResult> NewProduct()
        {
            return NoContent();
        }




    }
}