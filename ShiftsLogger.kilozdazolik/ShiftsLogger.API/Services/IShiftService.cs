using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTO;

namespace ShiftsLogger.API.Services;

public interface IShiftService
{
    Task<Shift> StartShift();
    Task StopShift(Guid id, DateTime endTime);
    Task<List<ShiftDto>> GetAllShifts();
    Task<ShiftDto?> GetShiftById(Guid id);
    Task DeleteShift(Guid id);
    Task UpdateShift(Shift shift);
}