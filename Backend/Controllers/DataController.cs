using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[ApiController]
// [Route("api/[controller]")]
// removed for simplicity
[Route("user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] User newUser)
    {
        await _userService.CreateAsync(newUser);
        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Hello World");
    }

    [HttpGet("{id}/profile-picture")]
    public async Task<IActionResult> GetProfilePicture(string id) {
        var user = await _userService.GetAsync(id);
        if (user?.ProfilePicture == null) {
            return File(user.ProfilePicture, "image/jpeg/png");
        }
        return NotFound();
    }
}

// [ApiController]
// [Route("api/[controller]")]
// public class DataController : ControllerBase
// {
//     private readonly UserService _userService;

//     public DataController(UserService userService)
//     {
//         _userService = userService;
//     }

//     [HttpPost("users")]
//     public async Task<IActionResult> CreateUser([FromBody] User newUser)
//     {
//         await _userService.CreateAsync(newUser);
//         return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
//     }

//     [HttpGet("users/{id}")]
//     public async Task<IActionResult> GetUser(string id)
//     {
//         var user = await _userService.GetAsync(id);

//         if (user == null)
//         {
//             return NotFound();
//         }

//         return Ok(user);
//     }
// }
