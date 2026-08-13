using MamoScope_API.Data;
using MaMoScope_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace MamoScope_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorTestsController : ControllerBase
    {
        private readonly IMotorDriversRepository _repository;

        public MotorTestsController(IMotorDriversRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<MotorDrivers>>> GetAll()
        {
            var records = await _repository.GetAllAsync();
            return Ok(records);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MotorDrivers record)
        {
            if (record == null)
                return BadRequest("Geçersiz veri.");

            try
            {
                await _repository.AddAsync(record);
                return StatusCode(201, record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Kayıt sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] MotorDrivers record)
        {
            if (record == null || id != record.Id)
                return BadRequest("Geçersiz veri.");

            try
            {
                await _repository.UpdateAsync(record);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Güncelleme sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Silme sırasında hata oluştu: {ex.Message}");
            }
        }
    }
}