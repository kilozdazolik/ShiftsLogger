using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTO;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.Tests.Services;

public class ShiftServiceTests
{
    private ShiftsLoggerContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ShiftsLoggerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new  ShiftsLoggerContext(options);
    }
    
    [Fact]
    public async Task GetAllShifts_ReturnsAllShifts()
    {
        //Arrange
        using var context = GetInMemoryContext();
        context.Shifts.Add(new Shift { Id = Guid.NewGuid(), StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(8) });
        context.Shifts.Add(new Shift { Id = Guid.NewGuid(), StartTime = DateTime.Now.AddDays(-1), EndTime = DateTime.Now.AddDays(-1).AddHours(8) });
        await context.SaveChangesAsync();
        
        var service = new ShiftService(context);

        //Act
        var result = await service.GetAllShifts();

        //Assert
        result.Should().HaveCount(2); 
        result.First().Should().BeOfType<ShiftDto>(); 
    }

    [Fact]
    public async Task StartShift_ReturnsShift()
    {
        //Arrange
        using var  context = GetInMemoryContext();
        var service = new ShiftService(context);
        
        //Act
        var result = await service.StartShift();
        
        //Assert
        result.Should().BeOfType<Shift>();
        result.Should().NotBeNull();
        context.Shifts.Should().HaveCount(1); 
        result.StartTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1)); 
    }

    [Fact]
    public async Task GetShiftById_ReturnsShift()
    {
        //Arrange
        using var context = GetInMemoryContext();
        var expectedId = Guid.NewGuid();
        context.Shifts.Add(new Shift { Id = expectedId, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(8) });
        await context.SaveChangesAsync();
        var service = new ShiftService(context);
        
        //Act
        var result = await service.GetShiftById(expectedId);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ShiftDto>();
        result.Id.Should().Be(expectedId);
    }

    [Fact]
    public async Task DeleteShift_ReturnsTrue()
    {
        //Arrange
        using var context = GetInMemoryContext();
        var expectedId = Guid.NewGuid();
        context.Shifts.Add(new Shift { Id = expectedId, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(8) });
        await context.SaveChangesAsync();
        var service = new ShiftService(context);
        
        //Act
        await service.DeleteShift(expectedId);
        
        //Assert
        var deletedShift = await context.Shifts.FindAsync(expectedId);
        deletedShift.Should().BeNull();

    }
    
    [Fact]
    public async Task UpdateShift_UpdatesValuesInDatabase()
    {
        //Arrange
        using var context = GetInMemoryContext();
        var id = Guid.NewGuid();
        
        context.Shifts.Add(new Shift 
        { 
            Id = id, 
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = DateTime.Now.AddDays(-1).AddHours(8)
        });
        await context.SaveChangesAsync();
    
        var service = new ShiftService(context);
        
        var updatedShift = new Shift 
        { 
            Id = id, 
            StartTime = DateTime.Now, 
            EndTime = DateTime.Now.AddHours(8) 
        };

        //Act
        await service.UpdateShift(updatedShift);

        //Assert
        var shiftInDb = await context.Shifts.FindAsync(id);
        shiftInDb.Should().NotBeNull();
        shiftInDb.StartTime.Should().Be(updatedShift.StartTime);
    }
}