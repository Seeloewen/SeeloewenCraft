using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using SeeloewenLib;
using Windows.Security.Isolation;
using static System.Environment;

namespace SeeloewenCraft
{
    public static class Log
    {
        public static wndMenu wndMenu;
        private static Paragraph paragraph;
        public static wndLog wndLog;
        private static SeeloewenLibTools seeloewenLibTools = new SeeloewenLibTools();
        private static List<string> messages = new List<string>();
        public static bool isOpen;

        //-- Custom Methods --//

        public static void Show()
        {
            if (!isOpen)
            {
                wndLog = new wndLog();

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
                wndLog.rtbLog.Document.Blocks.Clear();
                wndLog.rtbLog.Document.Blocks.Add(paragraph);

                //Show the log
                wndLog.Show();
                isOpen = true;
            }
        }

        public static void Save(bool showMessageBoxes)
        {
            //Setup the save file dialog
            SaveFileDialog sfdLog = new SaveFileDialog();
            sfdLog.Filter = "Text (*.txt)|*.txt|All (*.*)|*.*";
            sfdLog.FileName = $"SeeloewenCraft_Log_{DateTime.Now.ToString().Replace(":", "-").Replace(".", "-").Replace(" ", "-")}.txt";
            sfdLog.ShowDialog();

            //Save the log content to the selected file
            try
            {
                //Split the color info at the end from the message and only save the messages
                List<string> onlyMessages = new List<string>();
                foreach (string message in messages)
                {
                    string[] messageSplit = message.Split(';');
                    onlyMessages.Add(messageSplit[0]);
                }
                File.WriteAllLines(sfdLog.FileName, onlyMessages);

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

        public static void Save(string location, bool showMessageBoxes)
        {
            //If location is invalid, use fallback
            if (string.IsNullOrEmpty(location) || !File.Exists(location))
            {
                location = $"{GetFolderPath(SpecialFolder.ApplicationData)}/SeeloewenCraft/logs";
            }

            //Save the log content to the selected file
            try
            {
                //Split the color info at the end from the message and only save the messages
                List<string> onlyMessages = new List<string>();
                foreach (string message in messages)
                {
                    string[] messageSplit = message.Split(';');
                    onlyMessages.Add(messageSplit[0]);
                }
                File.WriteAllLines($"{location}/SeeloewenCraft_Log_{DateTime.Now.ToString().Replace(":", "-").Replace(".", "-").Replace(" ", "-")}.txt", onlyMessages);

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

        public static void Clear()
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

        public static void Write(string message, string type)
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

            if (wndLog != null)
            {
                //Add all messages to the log richtextbox in the respective color
                paragraph = new Paragraph();
                foreach (string mes in messages)
                {
                    string[] messageSplit = mes.Split(';');
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
                wndLog.rtbLog.Document.Blocks.Clear();
                wndLog.rtbLog.Document.Blocks.Add(paragraph);
                wndLog.rtbLog.ScrollToEnd();
            }
        }

        public static void CreateCrashDump(Exception ex)
        {
            //Log all relevant information for a crash
            Write("-------------------------------------", "Error");
            Write($"SeeloewenCraft {Game.GAME_VERSION} - A crash has been detected!", "Error");
            Write($"Exception: {ex.GetType().ToString()}!", "Error");
            Write($"Message: {ex.Message}", "Error");
            Write($"Source: {ex.Source}", "Error");
            Write($"Additional Data:\n{ex.StackTrace}", "Error");
            Write("-------------------------------------", "Error");
        }
    }
}
