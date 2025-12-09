using ShiftsLogger.UI;
using ShiftsLogger.UI.Controllers;
using ShiftsLogger.UI.Services;

HttpClient client = new HttpClient();
ShiftService shiftService = new ShiftService(client);
ShiftController shiftController = new ShiftController(shiftService);
UserInterface ui = new UserInterface(shiftController);

await ui.MainMenu();