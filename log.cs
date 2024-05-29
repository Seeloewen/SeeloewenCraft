using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SeeloewenCraft.Properties;
using SeeloewenLib;

namespace SeeloewenCraft
{
    public class Log
    {
        wndLog wndLog;
        SeeloewenLibTools seeloewenLibTools = new SeeloewenLibTools();
        private List<string> messages = new List<string>();
        Paragraph paragraph;

        public void Show()
        {
            //Create the log window
            wndLog = new wndLog(this);

            //Add all messages to the log richtextbox in the respective color
            paragraph = new Paragraph();
            foreach (string message in messages)
            {
                string[] messageSplit = message.Split(';');
                Run line;
                if (messageSplit[1] == "red")
                {
                    line = new Run($"{messageSplit[0]}\n") { Foreground = new SolidColorBrush(Colors.Red) };
                }
                else if (messageSplit[1] == "orange")
                {
                    line = new Run($"{messageSplit[0]}\n") { Foreground = new SolidColorBrush(Colors.DarkOrange) };
                }
                else
                {
                    line = new Run($"{messageSplit[0]}\n") { Foreground = new SolidColorBrush(Colors.Blue) };
                }
                paragraph.Inlines.Add(line);
            }
            wndLog.rtbLog.Document.Blocks.Add(paragraph);

            //Show the log
            wndLog.ShowDialog();
        }

        public void Save(bool showMessageBoxes)
        {
            //Setup the save file dialog
            SaveFileDialog sfdLog = new SaveFileDialog();
            sfdLog.Filter = "Text (*.txt)|*.txt|All (*.*)|*.*";
            sfdLog.FileName = $"SeeloewenCraft_Log_{DateTime.Now.ToString().Replace(":", "-").Replace(".", "-").Replace(" ", "-")}";
            sfdLog.ShowDialog();

            //Save the log content to the selected file
            try
            {
                foreach (string line in messages)
                {
                    //Split the additional color information from the log
                    string[] lineSplit = line.Split(';');
                    File.AppendAllText(sfdLog.FileName, lineSplit[0] + "\n");
                }
                if (showMessageBoxes)
                {
                    System.Windows.MessageBox.Show($"Successfully saved the log to {sfdLog.FileName}!", "Saved log", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                {
                    System.Windows.MessageBox.Show($"Error while saving log to {sfdLog.FileName}: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Write($"Error while saving log to {sfdLog.FileName}: {ex}", "Error");
            }
        }

        public void Save(string location, bool showMessageBoxes)
        {

            showMessageBoxes = true;
            location = "D:\\test";
            //Save the log content to the selected file
            try
            {
                foreach (string line in messages)
                {
                    //Split the additional color information from the log
                    string[] lineSplit = line.Split(';');
                    File.AppendAllText(location, lineSplit[0] + "\n");
                }
                if (showMessageBoxes)
                {
                    {
                        System.Windows.MessageBox.Show($"Successfully saved the log to {location}!", "Saved log", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                {
                    System.Windows.MessageBox.Show($"Error while saving log to {location}: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Write($"Could not save log to {location}: {ex}", "Error");
            }
        }

        public void Clear()
        {
            //Ask the user whether they want to clear the log
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure that you want to clear your current log? You will not be able to recover it!", "Clear Log", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (result)
            {
                //Clear the log
                case MessageBoxResult.Yes:
                    messages.Clear();
                    paragraph.Inlines.Clear();
                    break;
            }
        }

        public void Write(string message, string type)
        {
            //Write a message to log depending on the message type.
            string prefix = "";
            string color = "";

            if (type == "Error")
            {
                prefix = "ERROR";
                color = "red";
            }
            else if (type == "Warning")
            {
                prefix = "WARNING";
                color = "orange";
            }
            else if (type == "Info")
            {
                prefix = "INFO";
                color = "blue";
            }

            messages.Add($"[{DateTime.Now}] [{prefix}] {message};{color}");
        }
    }
}
