using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;

[ApiController]
// [Route("api/[controller]")]
// removed for simplicity
[Route("homepage")]
public class HomepageController : ControllerBase
{
    private readonly UserService _userService;

    public HomepageController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var userFromDb = await _userService.GetByUsername(user.Username);

        if (userFromDb == null)
        {
            return NotFound("User not found");
        }

        if (!userFromDb.VerifyPassword(user.Password))
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