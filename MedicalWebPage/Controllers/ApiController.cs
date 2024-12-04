using Microsoft.AspNetCore.Mvc;
using MedicalWebPage.Data;
using MedicalWebPage.Models;
using System.Text.Json;
using System.Security.Claims;

namespace MedicalWebPage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SensorDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveData([FromBody] JsonElement jsonData)
        {
            try
            {
                // Parse RGB property
                if (!jsonData.TryGetProperty("RGB", out var rgbElement) || rgbElement.ValueKind != JsonValueKind.Array)
                    return BadRequest(new { error = "Invalid or missing RGB field" });

                if (!jsonData.TryGetProperty("Volume", out var volumeElement) || volumeElement.ValueKind != JsonValueKind.Number)
                    return BadRequest(new { error = "Invalid or missing Volume field" });

                if (!jsonData.TryGetProperty("Temperature", out var tempElement) || tempElement.ValueKind != JsonValueKind.Number)
                    return BadRequest(new { error = "Invalid or missing Temperature field" });

                var rgb = rgbElement.EnumerateArray().Select(e => e.GetInt32()).ToList();
                double volume = volumeElement.GetDouble();
                double temperature = tempElement.GetDouble();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Create SensorData object
                var sensorData = new SensorData
                {
                    Volume = volume,
                    Temperature = temperature,
                    RGB = rgb,
                    DateTimeCollected = DateTime.Now,
                    UserId = userId ?? "theuserID"
                };

                // Save to database (optional)
                _context.SensorData.Add(sensorData);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Data received successfully" });
            }
            catch (Exception ex)
            {
                // Handle parsing errors
                return BadRequest(new { error = ex.Message, inner = ex.InnerException?.Message });
            }
        }


    }
}
