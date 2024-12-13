using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;

// [ApiController]
// [Route("api/[controller]")]
// removed for simplicity
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetUser([FromQuery] string username, [FromQuery] string email)
    {
        var user = await _userService.GetByUsername(username);
        if (user != null)
        {
            return Conflict("Username already exists");
        }

        user = await _userService.GetByEmail(email);
        if (user != null)
        {
            return Conflict("Email already exists");
        }

        return Ok();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] string username, [FromBody] string password)
    {
        var userFromDb = await _userService.GetByUsername(username);

        if (userFromDb == null)
        {
            return NotFound("User not found");
        }

        if (!userFromDb.VerifyPassword(password))
        {
            return Unauthorized("Invalid password");
        }

        // var token = JwtHelper.GenerateToken(userFromDb);

        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        var userFromDb = await _userService.GetByUsername(newUser.Username);

        if (userFromDb != null)
        {
            return Conflict("Username already exists");
        }
        newUser.SetPassword(newUser.Password);

        newUser.Password = null;

        await _userService.CreateAsync(newUser);


        return CreatedAtAction(nameof(Login), newUser);
    }
}