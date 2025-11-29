using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTO;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    private readonly IShiftService _shiftService;
    public ShiftController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> StartShift()
    {
        var newShift = await _shiftService.StartShift();
        return Ok(newShift);
    }

    [HttpGet]
    public async Task<ActionResult<List<ShiftDto>>> GetAllShifts()
    {
        var shifts = await _shiftService.GetAllShifts();
        return Ok(shifts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftDto>> GetShiftById(Guid id)
    {
        var shift = await _shiftService.GetShiftById(id);

        if (shift == null) return NotFound();

        return Ok(shift);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShift(Guid id)
    {
        try
        {
            await _shiftService.DeleteShift(id);
            return Ok();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPut("{id}/stop")]
    public async Task<ActionResult> StopShift(Guid id)
    {
        try
        {
            await _shiftService.StopShift(id, DateTime.UtcNow);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateShift(Guid id, Shift shift)
    {
        if (id != shift.Id) return BadRequest();
        try
        {
            await _shiftService.UpdateShift(shift);
            return Ok();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}