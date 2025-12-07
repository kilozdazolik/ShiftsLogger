using System.ComponentModel;
using ShiftsLogger.UI.Controllers;
using ShiftsLogger.UI.Enums;
using ShiftsLogger.UI.Services;
using Spectre.Console;

namespace ShiftsLogger.UI;

public class UserInterface
{
    private readonly ShiftController _controller;

    public UserInterface(ShiftController controller)
    {
        _controller = controller;
    }
    
    public async Task MainMenu()
    {
        AnsiConsole.MarkupLine("Welcome to the Shift Logger!");
        var choices = Enum.GetValues<MenuAction>()
            .Select(v => (Value: v,
                Text: v.GetType()
                    .GetField(v.ToString())?
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .Cast<DescriptionAttribute>()
                    .FirstOrDefault()?.Description ?? v.ToString()))
            .ToDictionary(x => x.Text, x => x.Value);

        while (true)
        {
            AnsiConsole.MarkupLine("\n----------------------------------------\n");
            var choiceText = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What do you want to do [green]next[/]?")
                    .AddChoices(choices.Keys));
            var choice = choices[choiceText];
            AnsiConsole.Clear();
            switch (choice)
            {
                case MenuAction.StartShift:
                    await _controller.StartShift();
                    break;
                case MenuAction.EndShift:
                    await _controller.StopShift();
                    break;
                case MenuAction.GetAllShift:
                    await _controller.ListAllShifts();
                    break;
                case MenuAction.GetShiftDetails:
                    await _controller.ShowShiftDetails();
                    break;
                case MenuAction.DeleteShift:
                    await _controller.DeleteShift();
                    break;
                case MenuAction.UpdateShiftDetails:
                    await _controller.UpdateShift();
                    break;
                case MenuAction.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }

}