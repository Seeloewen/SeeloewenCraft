using System.Windows.Controls;

namespace SeeloewenCraft
{
    internal static class TextblockExtensions
    {
        public static void SetAlignedText(this TextBlock textBlock, string text)
        {
            textBlock.Text = text;
            Canvas.SetLeft(textBlock, -text.Length * 4);
        }
    }
}
