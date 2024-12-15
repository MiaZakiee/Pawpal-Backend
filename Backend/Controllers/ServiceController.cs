using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;

[ApiController]
[Route("services")]
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

    [HttpPost("addservice")]
    public async Task<IActionResult> AddService([FromBody] Service service)
    {
        await _serviceService.CreateServiceAsync(service);
        return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string id)
    {
        await _serviceService.DeleteServiceAsync(id);
        return NoContent();
    }
}
