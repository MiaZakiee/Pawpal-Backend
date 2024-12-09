using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly ServiceServices _serviceService;

    public ServiceController(ServiceServices serviceServices)
    {
        _serviceService = serviceServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById(string id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);
        if (service == null)
            return NotFound();
        return Ok(service);
    }

    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] Service newService)
    {
        await _serviceService.CreateServiceAsync(newService);
        return CreatedAtAction(nameof(GetServiceById), new { id = newService.Id }, newService);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string id)
    {
        await _serviceService.DeleteServiceAsync(id);
        return NoContent();
    }
}
    