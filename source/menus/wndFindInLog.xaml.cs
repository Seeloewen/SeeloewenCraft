using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SeeloewenCraft
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
            //Highlight the text specified while keeping the formatting
            TextPointer currentPointer = richTextBox.Document.ContentStart;
            while (currentPointer != null && currentPointer.CompareTo(richTextBox.Document.ContentEnd) < 0)
            {
                if (currentPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = currentPointer.GetTextInRun(LogicalDirection.Forward);
                    int index = textRun.IndexOf(searchText);
                    if (index >= 0)
                    {
                        TextPointer start = currentPointer.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(searchText.Length);
                        TextRange searchRange = new TextRange(start, end);
                        searchRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                        currentPointer = end;
                    }
                    else
                    {
                        currentPointer = currentPointer.GetNextContextPosition(LogicalDirection.Forward);
                    }
                }
                else
                {
                    currentPointer = currentPointer.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
        }

        private void UnhighlightAll(RichTextBox richTextBox)
        {
            //Unhighlight all text while keeping the formatting
            TextPointer currentPointer = richTextBox.Document.ContentStart;
            while (currentPointer != null && currentPointer.CompareTo(richTextBox.Document.ContentEnd) < 0)
            {
                if (currentPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    TextPointer nextPointer = currentPointer.GetNextContextPosition(LogicalDirection.Forward);
                    if (nextPointer != null)
                    {
                        TextRange textRange = new TextRange(currentPointer, nextPointer);
                        textRange.ApplyPropertyValue(TextElement.BackgroundProperty, null);
                        currentPointer = nextPointer;
                    }
                }
                else
                {
                    currentPointer = currentPointer.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
