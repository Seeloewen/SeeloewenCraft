using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SeeloewenCraft
{
    public class DebugMenu
    {
        public Canvas cvsDebugMenu = new Canvas();
        public TextBlock tblGameStats = new TextBlock();
        public TextBlock tblBlockStats = new TextBlock();
        World world;

        public DebugMenu(World world)
        {
            this.world = world;

            //Setup debug canvas
            cvsDebugMenu.Height = 630;
            cvsDebugMenu.Width = 1180;
            world.wndGame.cvsGame.Children.Add(cvsDebugMenu);
            Canvas.SetLeft(cvsDebugMenu, 10);
            Canvas.SetTop(cvsDebugMenu, 100);

            //Setup world stats textblock
            cvsDebugMenu.Children.Add(tblGameStats);
            tblGameStats.FontSize = 20;
            tblGameStats.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(tblGameStats, 10);

            //Setup block stats textblock
            cvsDebugMenu.Children.Add(tblBlockStats);
            tblBlockStats.FontSize = 20;
            tblBlockStats.FontWeight = FontWeights.Bold;
            tblBlockStats.TextAlignment = TextAlignment.Right;
            Canvas.SetRight(tblBlockStats, 10);
        }

        public void Update(TextBlock textBlock, string line, string content)
        {
            //If the line exists update it, else add it.
            if (!world.debugMenu.tblBlockStats.Text.Contains(line))
            {
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, line);
            }
            else
            {
                world.debugMenu.ChangeLine(world.debugMenu.tblBlockStats, line, content);
            }
        }

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
            for(int i = 0; i < splitContent.Length; i++)
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

            foreach (string l in splitContent)
            {
                AddLine(textBlock, l);
            }
        }
    }
}
