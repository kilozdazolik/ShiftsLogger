using ShiftsLogger.UI.Services;
using Spectre.Console; 
using ShiftsLogger.UI.Models;

namespace ShiftsLogger.UI.Controllers;

public class ShiftController
{
    private readonly ShiftService _shiftService;
    public ShiftController(ShiftService shiftService)
    {
        _shiftService = shiftService;
    }
    
public async Task ListAllShifts()
    {
        var shifts = await _shiftService.GetAllShiftsAsync();

        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No shifts have been found.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");

        foreach (var shift in shifts)
        {
            string endTimeString = shift.EndTime.HasValue ? shift.EndTime.Value.ToString() : "[green]On going...[/]";
            string durationString = shift.Duration.HasValue ? shift.Duration.Value.ToString(@"hh\:mm") : "-";

            table.AddRow(shift.StartTime.ToString(), endTimeString, durationString);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press a key to continue...");
        Console.ReadKey();
    }

    public async Task ShowShiftDetails()
    {
        List<ShiftDto> allShifts = await _shiftService.GetAllShiftsAsync();
        var id = GetShiftIdFromUser(allShifts,"Which shift do you want to check?");
        var shift = await _shiftService.GetShiftByIdAsync(id.Value);

        if (shift == null)
        {
            AnsiConsole.MarkupLine("[red]No shift has found.[/]");
        }
        else
        {
            var panel = new Panel($"""
                ID: {shift.Id}
                Start: {shift.StartTime}
                End: {shift.EndTime}
                Duration: {shift.Duration}
                """);
            panel.Header = new PanelHeader("Shift Details");
            AnsiConsole.Write(panel);
        }
        
        Console.ReadKey();
    }
    
    public async Task StartShift()
    {
        var shift = await _shiftService.StartShiftAsync();
        if (shift != null)
        {
            AnsiConsole.MarkupLine($"[green]Shift has started at: {shift.StartTime}[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]An unexpected error has occured.[/]");
        }
        Console.ReadKey();
    }
    
    public async Task StopShift()
    {
        List<ShiftDto> allShifts = await _shiftService.GetAllShiftsAsync();
        var id = GetShiftIdFromUser(allShifts, "Which  shift do you want to stop?");
        if (id == null) return;
        try 
        {
            await _shiftService.StopShiftAsync(id.Value);
            AnsiConsole.MarkupLine("[green]Shift has been stopped![/]");
        }
        catch(Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Console.ReadKey();
    }
    
    public async Task DeleteShift()
    {
        List<ShiftDto> allShifts = await _shiftService.GetAllShiftsAsync();
        var id = GetShiftIdFromUser(allShifts, "Which shift you want to delete?");
        
        if(!await AnsiConsole.ConfirmAsync("Are you sure you want to delete?")) return;

        try
        {
            await _shiftService.DeleteShiftAsync(id.Value);
            AnsiConsole.MarkupLine("[green]Shift has been deleted![/]");
        }
        catch (Exception ex)
        {
             AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Console.ReadKey();
    }
    
    private Guid? GetShiftIdFromUser(List<ShiftDto> shifts, string message)
    {
        AnsiConsole.MarkupLine($"[yellow]{message}[/]");
        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No available data![/]");
            return null;
        }
        
        var selectedShift = AnsiConsole.Prompt(
            new SelectionPrompt<ShiftDto>()
                .Title("Choose one from the list:")
                .PageSize(10) 
                .AddChoices(shifts) 
                .UseConverter(shift => 
                {
                    string endTime = shift.EndTime.HasValue ? shift.EndTime.ToString() : "Ongoing...";
                    return $"{shift.StartTime} - {endTime}";
                })
        );
        return selectedShift.Id;
    }
    
    public async Task UpdateShift()
{
    List<ShiftDto> allShifts = await _shiftService.GetAllShiftsAsync();
    var id = GetShiftIdFromUser(allShifts, "Which shift do you want to update?");

    if (id == null) return; 
    
    var shiftToUpdate = await _shiftService.GetShiftByIdAsync(id.Value);
    
    if (shiftToUpdate == null)
    {
        AnsiConsole.MarkupLine("[red]Shift not found.[/]");
        Console.ReadKey();
        return;
    }
    
    AnsiConsole.MarkupLine("Update values (Press [green]Enter[/] to keep current value):");
    
    shiftToUpdate.StartTime = AnsiConsole.Prompt(
        new TextPrompt<DateTime>("Start Time:")
            .DefaultValue(shiftToUpdate.StartTime)
            
            .ValidationErrorMessage("[red]Invalid date format![/]")
    );
    
    if (shiftToUpdate.EndTime != null)
    {
        shiftToUpdate.EndTime = AnsiConsole.Prompt(
            new TextPrompt<DateTime>("End Time:")
                .DefaultValue(shiftToUpdate.EndTime.Value)
                .ValidationErrorMessage("[red]Invalid date format![/]")
        );
    }
    else
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>("End Time (Leave empty to keep 'Ongoing'):")
                .AllowEmpty());

        if (!string.IsNullOrWhiteSpace(input) && DateTime.TryParse(input, out DateTime parsedDate))
        {
            shiftToUpdate.EndTime = parsedDate;
        }
    }
    
    try
    {
        await _shiftService.UpdateShiftAsync(shiftToUpdate);
        AnsiConsole.MarkupLine("[green]Shift updated successfully![/]");
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]Error during update: {ex.Message}[/]");
    }

    Console.ReadKey();
}
}