using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTO;

namespace ShiftsLogger.API.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftsLoggerContext _dbContext;

    public ShiftService(ShiftsLoggerContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Shift> StartShift()
    {
        Shift shift = new()
        {
            Id = Guid.NewGuid(),
            StartTime = DateTime.UtcNow,
            EndTime = null,
        };
        
        _dbContext.Shifts.Add(shift);
        await _dbContext.SaveChangesAsync();

        return shift;
    }

    public async Task StopShift(Guid id, DateTime endTime)
    {
        var shift = await _dbContext.Shifts.FindAsync(id);
        
        if(shift == null) throw new KeyNotFoundException("Shift not found");
        if(shift.EndTime != null) throw new Exception("Shift already stopped"); 
        
        shift.EndTime = endTime;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ShiftDto>> GetAllShifts()
    {
      return await  _dbContext.Shifts
            .OrderByDescending(s => s.StartTime)
            .Select(s => new ShiftDto
            {
                Id = s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Duration = s.EndTime.HasValue ? (s.EndTime - s.StartTime) : null
            })
            .ToListAsync();
    }

    public async Task<ShiftDto?> GetShiftById(Guid id)
    {
        return await _dbContext.Shifts
            .Where(s => s.Id == id)
            .Select(s => new ShiftDto
            {
                Id =  s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Duration = s.EndTime.HasValue ? (s.EndTime.Value - s.StartTime) : null
            })
            .FirstOrDefaultAsync();
    }

    public async Task DeleteShift(Guid id)
    {
        var shift = await _dbContext.Shifts.FindAsync(id);

        if (shift == null)
        {
            throw new KeyNotFoundException("Shift ID not found");
        }

        _dbContext.Shifts.Remove(shift);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateShift(Shift shift)
    {
        var shiftToUpdate = await _dbContext.Shifts.FindAsync(shift.Id);
        if (shiftToUpdate == null) throw new KeyNotFoundException("Shift not found");
        
        shiftToUpdate.StartTime = shift.StartTime;
        shiftToUpdate.EndTime = shift.EndTime;
        
        _dbContext.Shifts.Update(shiftToUpdate);
        await _dbContext.SaveChangesAsync();
    }
}