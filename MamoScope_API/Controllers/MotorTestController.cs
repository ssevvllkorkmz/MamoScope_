using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MamoScope_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorTestsController : ControllerBase
    {
        private readonly MotorDriverService _service;

        public MotorTestsController(MotorDriverService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MotorDriverDto>>> GetAll()
        {
            var records = await _service.GetAllAsync();

            return Ok(records);
        }

        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<MotorDriverDto>> GetBySerialNumber(
            string serialNumber)
        {
            var record = await _service.GetBySerialNumberAsync(serialNumber);

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<MotorDriverDto>> Create(
            CreateMotorDriverDto dto)
        {
            try
            {
                var created = await _service.AddAsync(dto);

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MotorDriverDto>> Update(
            int id,
            CreateMotorDriverDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}