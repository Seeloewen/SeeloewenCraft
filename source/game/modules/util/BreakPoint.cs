using System.Windows;

namespace SeeloewenCraft.source.game.modules.util;

public class BreakPoint
{

    public static bool breakNext = false;
    public static void Set()
    {
        if (breakNext)
        {
            breakNext = false;
            MessageBox.Show("Breakpoint hit");
        }
    }
    
    
    
    
    
}