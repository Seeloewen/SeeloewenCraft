using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
