using System.Windows;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace SeeloewenCraft.game.util;

public class BreakPoint
{

    public static bool breakNext = false;
    public static void Set()
    {
        if (breakNext)
        {
            breakNext = false;
            var box = MessageBoxManager.GetMessageBoxStandard("Debug", "Breakpoint hit", ButtonEnum.Ok, Icon.Database);
            box.ShowAsync();
        }
    }
    
    
    
    
    
}