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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePet([FromBody] Pet updatedPet, string id)
    {
        Pet updated = await _petsService.UpdatePetAsync(id, updatedPet);

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePet(string id)
    {
        var pet = await _petsService.GetPetByIdAsync(id);

        if (pet == null)
            return NotFound();

        // Delete the pet
        await _petsService.RemovePetAsync(id);

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPet(string id)
    {
        var pet = await _petsService.GetPetByIdAsync(id);

        if (pet == null)
            return NotFound();

        return Ok(pet);
    }

    [HttpGet("owners/{userId}")]
    public async Task<IActionResult> GetPets(string userId)
    {
        var pets = await _petsService.GetPetsForUserAsync(userId);

        return Ok(pets);
    }
}