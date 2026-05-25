using Avalonia.Controls;
using AvRichTextBox;
using SeeloewenCraft.game.util.logging;

namespace SeeloewenCraft.launcher
{
    public partial class wndFindInLog : Window
    {

        public wndFindInLog()
        {
            InitializeComponent();
            UnhighlightAll(Log.wndLog.rtbLog);
        }

        private void HighlightText(RichTextBox richTextBox, string searchText)
        {
            //TODO: Avalonia Rework
        }

        private void UnhighlightAll(RichTextBox richTextBox)
        {
            //TODO: Avalonia Rework
        }

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            //Unhighlight all text before closing
            UnhighlightAll(Log.wndLog.rtbLog);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //High the specified text
            UnhighlightAll(Log.wndLog.rtbLog);
            if (!string.IsNullOrEmpty(tbSearch.Text))
            {
                HighlightText(Log.wndLog.rtbLog, tbSearch.Text);
            }
        }
    }
}
