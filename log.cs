using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SeeloewenCraft.Properties;
using SeeloewenLib;

namespace SeeloewenCraft
{
    public class Log
    {
        frmLog frmLog;
        SeeloewenLibTools seeloewenLibTools = new SeeloewenLibTools();
        private List<string> messages = new List<string>();
        private string directory;
        System.Windows.Forms.RichTextBox rtbLog = new System.Windows.Forms.RichTextBox();

        public Log(string directory)
        {
            rtbLog.Width = 835;
            rtbLog.Height = 460;
            rtbLog.SelectionFont = new Font("Arial", 12);
            rtbLog.Location = new System.Drawing.Point(15, 55);
        }

        public void Show()
        {
            //Show the log window
            frmLog = new frmLog();
            frmLog.Controls.Add(rtbLog);
            frmLog.ShowDialog();
        }

        public void Save()
        {

        }

        public void Clear()
        {

        }

        public void Write(string message, string type)
        {
            //Write a message to log depending on the message type.
            if (type == "Error")
            {
                rtbLog.SelectionColor = System.Drawing.Color.Red;
                rtbLog.AppendText(message);
            }
            else if (type == "Warning")
            {
                rtbLog.SelectionColor = System.Drawing.Color.Orange;
                rtbLog.AppendText(message);
            }
            else if (type == "Info")
            {
                rtbLog.SelectionColor = System.Drawing.Color.Blue;
                rtbLog.AppendText(message);
            }
        }
    }
}
