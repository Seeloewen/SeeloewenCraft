using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace SeeloewenCraft
{
    public partial class wndAbout : Window
    {
        wndMenu wndMenu;

        //-- Constructor --//

        public wndAbout(wndMenu wndMenu)
        {
            InitializeComponent();

            this.wndMenu = wndMenu;

            //Display the game version and release date
            tblVersion.Text = $"Version {wndMenu.gameVersion} ({wndMenu.versionDate})";

            //Show the changelog
            LoadChangelog();
        }

        //-- Custom Methods --/

        private void LoadChangelog()
        {
            //Read changelog from file
            Uri uri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/changelog.txt", UriKind.Absolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using StreamReader reader = new(info.Stream);
            tbChangelog.Text = reader.ReadToEnd();
        }

        //-- Event Handlers --//

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Close the about window
            Close();
        }
    }
}
