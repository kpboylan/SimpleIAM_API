using Microsoft.AspNetCore.Mvc;
using SimpleIAM_API.DTO;
using SimpleIAM_API.Service;

namespace SimpleIAM_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LegacyUsersController : ControllerBase
    {
        private readonly IUserService _adapter;

        public LegacyUsersController(IUserService adapter)
        {
            _adapter = adapter;
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            var user = _adapter.RegisterAsync(dto.Email, dto.Password);

            return Ok(user);
        }

    }
}
