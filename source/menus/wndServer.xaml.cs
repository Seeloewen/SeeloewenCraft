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

    public partial class wndServer : Window
    {
        wndMenu wndMenu;

        public wndServer(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;

            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            Game.client = new Client();
            Game.client.Connect(tbIp.Text, Convert.ToInt32(tbPort.Text));
            Game.world = new World(wndMenu, DateTime.Now.Microsecond.ToString(), true, 0, "");
            wndMenu.Hide();
            Close();
        }
    }
}
