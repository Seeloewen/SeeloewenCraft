using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.launcher;
using SeeloewenLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;

namespace SeeloewenCraft.game.util.logging
{
    public enum LogType
    {
        GENERAL,
        WORLD_GENERATION,
        STRUCTURE_GENERATION,
        NETWORK,
        ENTITIES,
        RENDERING,
        TEST
    }

    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR
    }

    public static class Log
    {
        public static wndMenu wndMenu;
        private static Paragraph paragraph;
        public static wndLog wndLog;
        private static List<(string text, LogLevel level)> messages = new List<(string, LogLevel)>();
        public static bool isOpen;

        //-- Custom Methods --//

        public static void Show()
        {
            if (!isOpen)
            {
                wndLog = new wndLog();

                //Add all messages to the log richtextbox in the respective color
                paragraph = new Paragraph();

                foreach (var message in messages)
                {
                    paragraph.Inlines.Add(new Run($"{message.text}\n") { Foreground = GetColor(message.level) });
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

            if (sfdLog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            //Save the log content to the selected file
            try
            {
                //Split the color info at the end from the message and only save the messages
                List<string> onlyMessages = new List<string>();
                foreach (var message in messages)
                {
                    onlyMessages.Add(message.text);
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
                Write($"Error while saving log to {sfdLog.FileName}: {ex}\n{ex.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
            }
        }

        public static void Save(string location, bool showMessageBoxes)
        {
            //If location is invalid, use fallback
            if (string.IsNullOrEmpty(location) || !File.Exists(location))
            {
                location = FolderUtil.logsFolder;
            }

            //Save the log content to the selected file
            try
            {
                //Split the color info at the end from the message and only save the messages
                List<string> onlyMessages = new List<string>();
                foreach (var message in messages)
                {
                    onlyMessages.Add(message.text);
                }
                File.WriteAllLines($"{location}/SeeloewenCraft_Log_{DateTime.Now.ToString().Replace(":", "-").Replace(".", "-").Replace(" ", "-")}.txt", onlyMessages);

                if (showMessageBoxes)
                {
                    System.Windows.MessageBox.Show($"Successfully saved the log to {location}!", "Saved log", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                {
                    System.Windows.MessageBox.Show($"Error while saving log to {location}: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Write($"Could not save log to {location}: {ex}\n{ex.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
            }
        }

        public static void Clear()
        {
            //Ask the user whether they want to clear the log
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure that you want to clear your current log? You will not be able to recover it!", "Clear Log", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //Clear the log
            if (result == MessageBoxResult.Yes)
            {
                messages.Clear();
                paragraph.Inlines.Clear();
            }
        }

        public static string GetPrefix(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.ERROR:
                    return "Error";
                case LogLevel.WARNING:
                    return "Warning";
                default:
                    return "Info";
            }
        }

        public static SolidColorBrush GetColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.ERROR:
                    return new SolidColorBrush(Colors.Red);
                case LogLevel.WARNING:
                    return new SolidColorBrush(Colors.DarkOrange);
                default:
                    return new SolidColorBrush(Colors.Blue);
            }
        }


        public static void WriteD(string msg)
        {
            Write(msg, LogType.GENERAL, LogLevel.WARNING);
        }

        public static void Write(string message, LogType type, LogLevel level)
        {
            if (!LogTypeEnabled(type))
            {
                return;
            }

            //Write a message to log depending on the message type.
            messages.Add(($"[{DateTime.Now}] [{GetPrefix(level)}] {message}", level));

            //If there's too many messages, remove the first one
            if (messages.Count > 2000)
            {
                messages.Remove(messages[0]);
            }

            if (wndLog != null)
            {
                //Add all messages to the log richtextbox in the respective color
                paragraph = new Paragraph();

                foreach (var mes in messages)
                {
                    paragraph.Inlines.Add(new Run($"{mes.text}\n") { Foreground = GetColor(mes.level) });
                }

                wndLog.rtbLog.Document.Blocks.Clear();
                wndLog.rtbLog.Document.Blocks.Add(paragraph);
                wndLog.rtbLog.ScrollToEnd();
            }
        }

        public static void CreateCrashDump(Exception ex)
        {
            //Log all relevant information for a crash
            Write("-------------------------------------", LogType.GENERAL, LogLevel.ERROR);
            Write($"SeeloewenCraft {Game.GAME_VERSION} - A crash has been detected!", LogType.GENERAL, LogLevel.ERROR);
            Write($"Exception: {ex.GetType().ToString()}!", LogType.GENERAL, LogLevel.ERROR);
            Write($"Message: {ex.Message}", LogType.GENERAL, LogLevel.ERROR);
            Write($"Source: {ex.Source}", LogType.GENERAL, LogLevel.ERROR);
            Write($"Stack Trace:\n{ex.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
            Write("-------------------------------------", LogType.GENERAL, LogLevel.ERROR);
        }

        private static bool LogTypeEnabled(LogType type)
        {
            return type == LogType.GENERAL && Settings.logGeneral ||
                type == LogType.WORLD_GENERATION && Settings.logWorldGeneration ||
                type == LogType.STRUCTURE_GENERATION && Settings.logStructureGeneration ||
                type == LogType.NETWORK && Settings.logNetwork ||
                type == LogType.ENTITIES && Settings.logEntities ||
                type == LogType.RENDERING && Settings.logRendering;

        }
    }
}
