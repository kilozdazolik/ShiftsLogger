using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.API.Controllers;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTO;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.Tests.Controller;

public class ShiftControllerTests
{
    private readonly IShiftService _shiftService;
    private readonly ShiftController _controller;
    public ShiftControllerTests()
    {
        _shiftService = A.Fake<IShiftService>();
        _controller = new ShiftController(_shiftService);
    }

    [Fact]
    public async Task GetAllShifts_ReturnOk()
    {
        //Arrange
        var shifts = new List<ShiftDto>
        {
            new ShiftDto { Id = Guid.NewGuid(), StartTime = DateTime.Now },
            new ShiftDto { Id = Guid.NewGuid(), StartTime = DateTime.Now.AddHours(1) }
        };
        A.CallTo(() => _shiftService.GetAllShifts()).Returns(Task.FromResult(shifts));

        //Act
        var result = await _controller.GetAllShifts();

        //Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedShifts = okResult.Value.Should().BeAssignableTo<List<ShiftDto>>().Subject;
        returnedShifts.Should().BeEquivalentTo(shifts);
    }

    [Fact]
    public async Task GetShiftById_WhenShiftExists_ReturnOk()
    {
        //Arrange
        var shiftId = Guid.NewGuid();
        var fakeShift = new ShiftDto
        { 
            Id = shiftId, StartTime = DateTime.Now 
        };
        A.CallTo(() => _shiftService.GetShiftById(shiftId)).Returns(fakeShift);
        
        //Act
        var result = await _controller.GetShiftById(shiftId);

        //Assert
        var actionResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedShift = actionResult.Value.Should().BeAssignableTo<ShiftDto>().Subject;
        returnedShift.Id.Should().Be(shiftId);
    }
    
    [Fact]
    public async Task GetShiftById_WhenShiftDoesNotExist_ReturnNotFound()
    {
        //Arrange
        var shiftId = Guid.NewGuid();
        
        
        //Act
        var result = await _controller.GetShiftById(shiftId);

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteShift_WhenShiftFound_ReturnOk()
    {
        //Arrange
        var shiftId = Guid.NewGuid();

        //Act
        var result = await _controller.DeleteShift(shiftId);

        //Assert
        result.Should().BeOfType<OkResult>();
    }
    
    [Fact]
    public async Task DeleteShift_WhenShiftDoesNotExist_ReturnNotFound()
    {
        //Arrange
        var shiftId = Guid.NewGuid();
        A.CallTo(() => _shiftService.DeleteShift(shiftId))
            .ThrowsAsync(new KeyNotFoundException("Shift not found"));

        //Act
        var result = await _controller.DeleteShift(shiftId);

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    [Fact]
    public async Task UpdateShift_ValidationError_ReturnBadRequest()
    {
        //Arrange
        var bodyId = Guid.NewGuid();
        var urlId = Guid.NewGuid();
        var fakeShift = new Shift
        {
            Id = bodyId, StartTime = DateTime.Now
        };
        
        //Act
        var result = await _controller.UpdateShift(urlId, fakeShift);

        //Assert
        result.Should().BeOfType<BadRequestResult>();
        
    }

    [Fact]
    public async Task UpdateShift_ShiftFound_ReturnOk()
    {
        //Arrange
        var id = Guid.NewGuid();
        var fakeShift = new Shift
        {
            Id = id, StartTime = DateTime.Now
        };

        //Assert
        var result = await _controller.UpdateShift(id, fakeShift);

        //Act
        result.Should().BeOfType<OkResult>();
    }    
    
    [Fact]
    public async Task UpdateShift_ShiftNotFound_ReturnNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();
        var fakeShift = new Shift
        {
            Id = id, StartTime = DateTime.Now
        };
        A.CallTo(() => _shiftService.UpdateShift(fakeShift))
            .ThrowsAsync(new KeyNotFoundException("Shift not found"));

        //Assert
        var result = await _controller.UpdateShift(id, fakeShift);

        //Act
        result.Should().BeOfType<NotFoundObjectResult>();
    } 
}