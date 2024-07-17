using System.Windows;

namespace SeeloewenCraft
{

    public partial class wndLog : Window
    {
        Log log;

        //-- Constructor --//

        public wndLog(Log log)
        {
            InitializeComponent();
            this.log = log;
        }

        //-- Event Handlers --//

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save the log
            log.Save(true);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //Clear the log
            log.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Close the window
            Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //Open the window for finding something in the log
            wndFindInLog wndFindinLog = new wndFindInLog(log);
            wndFindinLog.Show();
        }
    }
}
