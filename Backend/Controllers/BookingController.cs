using Microsoft.AspNetCore.Mvc;
using PawpalBackend.Models;
using PawpalBackend.Services;
using System.Threading.Tasks;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService) {
        _bookingService = bookingService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
    {
        await _bookingService.CreateBookingAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetBookingListAsync();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(string id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(string id, [FromBody] Booking booking)
    {
        await _bookingService.UpdateBookingAsync(id, booking);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(string id)
    {
        await _bookingService.RemoveBookingAsync(id);
        return NoContent();
    }
}