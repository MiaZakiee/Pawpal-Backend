using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

    [Authorize]
    [HttpGet("user-details")]
    public async Task<IActionResult> GetUserDetails()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("User not found");
        }

        var user = await _userService.GetAsync(userId);

        return Ok(user);
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

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest data)
    {
        if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
        {
            return BadRequest("Invalid request");
        }

        var userFromDb = await _userService.GetByUsername(data.Username);

        if (userFromDb == null)
        {
            return NotFound("User not found");
        }

        if (!userFromDb.VerifyPassword(data.Password))
        {
            return Unauthorized("Invalid password");
        }

        var token = JwtHelper.GenerateToken(userFromDb, "rhabiemaerhabiemaerhabiemaerhabi", "http://localhost:5272", "http://localhost:8081");
        return Ok(new { Token = token });
    }

    public class CheckUserAndEmailRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

    [HttpPost("checkuserandemail")]
    public async Task<IActionResult> CheckUsernameAndEmail([FromBody] CheckUserAndEmailRequest data)
    {
        var username = await _userService.GetByUsername(data.Username);
        var email = await _userService.GetByEmail(data.Email);

        if (username != null)
        {
            return Conflict("Username already exists");
        }

        if (email != null)
        {
            return Conflict("Email already exists");
        }

        return Ok();
    }

    [HttpPost("registerPostman")]
    public async Task<IActionResult> RegisterPostman([FromBody] User newUser)
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {

        var userFromDb = await _userService.GetByUsername(newUser.Username);

        var phonenumber = await _userService.GetByPhoneNumber(newUser.PhoneNumber);

        if (phonenumber != null)
        {
            return Conflict("User with this phone number already exists");
        }

        if (userFromDb != null)
        {
            return Conflict("Username already exists");
        }

        newUser.SetPassword(newUser.Password);

        newUser.Password = null;

        await _userService.CreateAsync(newUser);

        var token = JwtHelper.GenerateToken(newUser, "rhabiemaerhabiemaerhabiemaerhabi", "http://localhost:5272", "http://localhost:8081");

        return CreatedAtAction(nameof(Login), new { token });
    }
    // rhabiemaerhabiemaerhabiemaerhabi
}