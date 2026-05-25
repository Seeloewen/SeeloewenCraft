using Avalonia.Controls;
using Avalonia.Interactivity;
using SeeloewenCraft.game;
using System.IO;
using System.Reflection;

namespace SeeloewenCraft.launcher
{
    public partial class wndAbout : Window
    {
        public wndAbout()
        {
            InitializeComponent();

            //Display the game version and release date
            tblVersion.Text = $"Version {Game.GAME_VERSION} ({Game.VERSION_DATE})";

            //Show the changelog
            LoadChangelog();
        }

        private void LoadChangelog()
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.changelog.txt");
            using (StreamReader reader = new(stream))
            {
                tbChangelog.Text = reader.ReadToEnd();
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Close the about window
            Close();
        }
    }
}
