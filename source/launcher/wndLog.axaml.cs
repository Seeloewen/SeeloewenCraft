using Avalonia.Controls;
using Avalonia.Interactivity;
using SeeloewenCraft.game.util.logging;

namespace SeeloewenCraft.launcher
{

    public partial class wndLog : Window
    {
        public wndLog()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save the log
            Log.Save(true);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //Clear the log
            Log.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Close the window
            Close();
        }

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            Log.isOpen = false;
        }
    }
}
