using Microsoft.AspNetCore.Mvc;
using SimpleIAM_API.DTO;
using SimpleIAM_API.Service;

namespace SimpleIAM_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto.Email, dto.Password);

                return Created("/api/users/" + user.Email, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registerWithGroup")]
        public async Task<IActionResult> RegisterWithGroup([FromBody] RegisterUserDto dto)
        {
            try
            {
                var user = await _userService.RegisterWithGroupAsync(dto.Email, dto.Password, dto.GroupName);

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto dto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);

                var response = new AuthenticatedUserDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Groups = user.Groups.Select(g => g.Group.Name).ToList()
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("assign-group")]
        public async Task<IActionResult> AssignGroup([FromBody] AssignGroupDto dto)
        {
            try
            {
                await _userService.AssignGroupAsync(dto.Email, dto.GroupId);
                return Ok($"Group '{dto.GroupId}' assigned to '{dto.Email}'");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{Email}")]
        public async Task<IActionResult> GetUser(string Email)
        {
            try
            {
                var user = await _userService.GetByEmailAsync(Email);

                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetAllAsync();

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("Groups")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var result = await _userService.GetAllGroupsAsync();

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


    }
}
