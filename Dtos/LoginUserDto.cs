
using System.ComponentModel.DataAnnotations;


namespace TestWebApi.Dtos
{

    public class LoginUserDto
    {

        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }

    }
}