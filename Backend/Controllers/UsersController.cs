using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

// [ApiController]
// [Route("api/[controller]")]
// removed for simplicity

public class UpdateProfileRequest
{
    public string Username { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
    public IFormFile ProfilePicture { get; set; }
}
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new { user.Id, user.Username, user.ProfilePicture, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.Bio });
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

        return Ok(new { user.Id, user.Username, user.PhoneNumber, user.Email, user.ProfilePicture, user.FirstName, user.LastName,  user.Bio, user.Services, user.Pets});
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
        return Ok(new { token, userFromDb.Id, userFromDb.Username, userFromDb.ProfilePicture, userFromDb.FirstName, userFromDb.LastName });
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
        newUser.SetPassword(newUser.password);

        newUser.password = null;

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

        newUser.SetPassword(newUser.password);

        newUser.password = null;

        await _userService.CreateAsync(newUser);

        var token = JwtHelper.GenerateToken(newUser, "rhabiemaerhabiemaerhabiemaerhabi", "http://localhost:5272", "http://localhost:8081");

        return CreatedAtAction(nameof(Login), new { token });
    }
    // rhabiemaerhabiemaerhabiemaerhabi

    [HttpGet("fetch-name")]
    public async Task<IActionResult> FetchName([FromQuery] string id)
    {
        var user = await _userService.GetAsync(id);
        return Ok(new { user.FirstName, user.LastName });
    }

    [Authorize]
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("User not found");
        }

        var user = await _userService.GetAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Username = request.Username;
        user.PhoneNumber = request.ContactNumber;
        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;

        if (request.ProfilePicture != null)
        {
            await _userService.SaveProfilePictureAsync(userId, request.ProfilePicture);
        }

        await _userService.UpdateAsync(userId, user);

        return Ok("Profile updated successfully!");
    }
}