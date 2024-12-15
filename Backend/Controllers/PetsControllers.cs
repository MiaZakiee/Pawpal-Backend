using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;

// [PetsController]
// [Route("api/[controller]")]
// removed for simplicity
[Route("pets")]
public class PetsController : ControllerBase
{
    private readonly PetsService _petsService;

    public PetsController(PetsService petsService)
    {
        _petsService = petsService;
    }

    [HttpPost("addpet")]
    public async Task<IActionResult> AddPet([FromBody] Pet newPet)
    {
        await _petsService.CreatePetAsync(newPet);

        // return CreatedAtAction(nameof(GetPets), newPet);
        return CreatedAtAction(nameof(GetPets), new { userId = newPet.Owner }, newPet);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPets(string userId)
    {
        var pets = await _petsService.GetPetsForUserAsync(userId);

        return Ok(pets);
    }
}