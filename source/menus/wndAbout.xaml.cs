using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;

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
            tblVersion.Text = $"Version {Game.GAME_VERSION} ({Game.VERSION_DATE})";

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
