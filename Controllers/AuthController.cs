using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using System.Text;
using TestWebApi.Models;
using TestWebApi.Services;
using TestWebApi.Dtos;
using Microsoft.AspNetCore.Authentication;

namespace TestWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRepository<User> _userDal;


        public AuthController(IConfiguration config, IRepository<User> userDal)
        {
            _config = config;
            _userDal = userDal;

        }



        [HttpPost("login")]
        public async Task Login()
        {
            await HttpContext.ChallengeAsync("Bearer", new AuthenticationProperties() { RedirectUri = "/" });

        }
    }
}