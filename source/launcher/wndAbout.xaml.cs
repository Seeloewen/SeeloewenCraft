using SeeloewenCraft.game;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using Windows.Storage;

namespace SeeloewenCraft.launcher
{
    public partial class wndAbout : Window
    {
        //-- Constructor --//

        public wndAbout()
        {
            InitializeComponent();

            //Display the game version and release date
            tblVersion.Text = $"Version {Game.GAME_VERSION} ({Game.VERSION_DATE})";

            //Show the changelog
            LoadChangelog();
        }

        //-- Custom Methods --/

        private void LoadChangelog()
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.changelog.txt");
            using (StreamReader reader = new(stream))
            {
                tbChangelog.Text = reader.ReadToEnd();
            }
        
        }

        //-- Event Handlers --//

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Close the about window
            Close();
        }
    }
}
