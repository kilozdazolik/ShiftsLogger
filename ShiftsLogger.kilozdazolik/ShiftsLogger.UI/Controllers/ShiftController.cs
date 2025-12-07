using ShiftsLogger.UI.Services;
using Spectre.Console; 

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
            AnsiConsole.MarkupLine("[yellow]No shift has found.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");

        foreach (var shift in shifts)
        {
            string endTimeString = shift.EndTime.HasValue ? shift.EndTime.Value.ToString() : "[green]On going...[/]";
            string durationString = shift.Duration.HasValue ? shift.Duration.Value.ToString(@"hh\:mm") : "-";

            table.AddRow(shift.Id.ToString(), shift.StartTime.ToString(), endTimeString, durationString);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press a key to continue...");
        Console.ReadKey();
    }

    public async Task ShowShiftDetails()
    {
        var id = GetShiftIdFromUser("Which shift do you want to check?");
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
        var id = GetShiftIdFromUser("Which  shift do you want to stop?");
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
        var id = GetShiftIdFromUser("Which shift do you want to delete?");
        
        if(!AnsiConsole.Confirm("Are you sure you want to delete?")) return;

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
    
    private Guid? GetShiftIdFromUser(string promptText)
    {
        var idString = AnsiConsole.Ask<string>(promptText + " (or type: 'exit'):");
        
        if (idString.Trim().ToLower() == "exit") return null;
        
        while (!Guid.TryParse(idString, out _))
        {
            AnsiConsole.MarkupLine("[red]This is not a valid ID![/]");
            idString = AnsiConsole.Ask<string>("Try again or type to return: 'exit'");

            if (idString.Trim().ToLower() == "exit") return null;
        }

        return Guid.Parse(idString);
    }
}