using System.Windows;

namespace SeeloewenCraft
{

    public partial class wndLog : Window
    {
        //-- Constructor --//

        public wndLog()
        {
            InitializeComponent();
        }

        //-- Event Handlers --//

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

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //Open the window for finding something in the log
            wndFindInLog wndFindinLog = new wndFindInLog();
            wndFindinLog.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.isOpen = false;
        }
    }
}
