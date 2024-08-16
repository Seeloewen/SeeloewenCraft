using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SeeloewenCraft
{
    public class DebugMenu
    {
        
        public Canvas cvsDebugMenu = new Canvas() { Height = 630, Width = 1250 };
        public TextBlock tblGameStats = new TextBlock() { FontSize = 20, FontWeight = FontWeights.Bold };
        public TextBlock tblBlockStats = new TextBlock() { FontSize = 20, FontWeight = FontWeights.Bold};
        public TextBlock tblPlayerStats = new TextBlock() { FontSize = 20, FontWeight = FontWeights.Bold };
        public TextBox tbDebug = new TextBox() { TextWrapping = TextWrapping.Wrap, Width = 1090, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Height = 28, FontSize = 15 };
        public Button btnDebug = new Button() { Content = "Send", Width = 115, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Height = 28, FontSize = 15 };
        public bool isEnabled = false;

        //-- Constructor --//

        public DebugMenu()
        {
            

            //Setup debug canvas
            Game.world.wndGame.cvsGame.Children.Add(cvsDebugMenu);
            Canvas.SetLeft(cvsDebugMenu, 10);
            Canvas.SetTop(cvsDebugMenu, 100);

            //Setup world stats textblock
            cvsDebugMenu.Children.Add(tblGameStats);
            Canvas.SetLeft(tblGameStats, 10);

            //Setup block stats textblock
            cvsDebugMenu.Children.Add(tblBlockStats);
            tblBlockStats.TextAlignment = TextAlignment.Right;
            Canvas.SetRight(tblBlockStats, 10);

            //Setup block stats textblock
            cvsDebugMenu.Children.Add(tblPlayerStats);
            Canvas.SetTop(tblPlayerStats, 100);
            Canvas.SetLeft(tblPlayerStats, 10);

            //Setup debug chat button
            cvsDebugMenu.Children.Add(btnDebug);
            btnDebug.Click += btnDebug_Click;
            Canvas.SetLeft(btnDebug, 1113);
            Canvas.SetTop(btnDebug, 535);
            Panel.SetZIndex(btnDebug, 1);

            //Setup debug chat textbox
            cvsDebugMenu.Children.Add(tbDebug);
            Canvas.SetLeft(tbDebug, 10);
            Canvas.SetTop(tbDebug, 535);
            Panel.SetZIndex(tbDebug, 1);

            Hide();
        }

        //-- Custom Methods --//

        public void AddLine(TextBlock textBlock, string content)
        {
            //If there's already content in the textblock, append it. If not, set it as the first line
            if (!string.IsNullOrEmpty(textBlock.Text))
            {
                textBlock.Text = textBlock.Text + "\n" + content;
            }
            else
            {
                textBlock.Text = content;
            }
        }

        public void ChangeLine(TextBlock textBlock, string line, string content)
        {
            string[] splitContent = textBlock.Text.Split('\n');

            //Search for the line that contains the existing content and replace
            for (int i = 0; i < splitContent.Length; i++)
            {
                if (splitContent[i].Contains(line))
                {
                    splitContent[i] = content;
                }
            }

            //Readd the lines to the string
            textBlock.Text = "";
            foreach (string l in splitContent)
            {
                AddLine(textBlock, l);
            }
        }

        public void RemoveLine(TextBlock textBlock, string line)
        {
            string[] splitContent = textBlock.Text.Split('\n');

            //Check for each line if it has the specified content and clear it
            for (int i = 0; i < splitContent.Length; i++)
            {
                if (splitContent[i].Contains(line))
                {
                    splitContent[i] = "";
                }
            }

            //Remove the clear lines from the array and add the array back
            splitContent = splitContent.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            textBlock.Text = "";
            foreach (string l in splitContent)
            {
                AddLine(textBlock, l);
            }
        }

        public void Show()
        {
            //Show the debug menu
            cvsDebugMenu.Visibility = Visibility.Visible;
            isEnabled = true;
        }

        public void Hide()
        {
            //Hide the debug menu
            cvsDebugMenu.Visibility = Visibility.Hidden;
            isEnabled = false;
        }

        //-- Event Handlers --//

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            //Check if the first character is a "/" and handle the command
            if (tbDebug.Text.Length > 0 && tbDebug.Text[0] == '/')
            {
                CommandHandler.HandleCommand(tbDebug.Text);
            }
            else
            {
                MessageBox.Show("Invalid command! Type /help for a list of commands.", "Invalid command", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            tbDebug.Clear();
        }
    }
}
