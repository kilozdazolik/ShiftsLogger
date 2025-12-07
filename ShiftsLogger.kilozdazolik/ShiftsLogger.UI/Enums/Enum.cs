using System.ComponentModel;

namespace ShiftsLogger.UI.Enums;

internal enum MenuAction {
    [Description("Start Shift")]
    StartShift,
    
    [Description("End Shift")]
    EndShift,

    [Description("Get All Shift")]
    GetAllShift,

    [Description("Get Shift Details")]
    GetShiftDetails,
    
    [Description("Update Shift Details")]
    UpdateShiftDetails,
    
    [Description("Delete Shift")]
    DeleteShift,

    [Description("Exit")]
    Exit
}