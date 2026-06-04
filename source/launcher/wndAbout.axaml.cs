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

            //Load the different resources
            tbChangelog.Text = LoadResource("changelog.txt");
            tbSpecialThanks.Text = LoadResource("special_thanks.txt");
            tbThirdParty.Text = LoadResource("third-party_licenses.txt");
            tbAbout.Text = LoadResource("about.txt");
        }

        private string LoadResource(string path) //TODO: This should definitely go into a util class
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.{path}");
            using (StreamReader reader = new(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Close the about window
            Close();
        }
    }
}
