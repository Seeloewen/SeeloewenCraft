using SeeloewenCraft.game.graphics.ui_lib;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal class CTextBox : CRectangle
    {
        private CTextField cTextField;
        private CBorder cBorder;

        internal CTextBox(int x, int y, int width, int height, Color c) : base(c, new Rectangle(x, y, x + width, y + height))
        {
            cTextField = new CTextField(2, new TextLayout(x + width / 2 - 10, TextHAlignment.LEFT, y + (height / 2) - 8, TextVAlignment.TOP), Math.Max(width / 8 - 20, 3));
            AddChild(cTextField);

            cBorder = new CBorder(3, new Color(0.23f));
            AddChild(cBorder);
        }

        protected override void OnClickEvent(ClickEvent mouseClickEvent)
        {
            cTextField.editMode = true;
        }

        internal void SetText(string text)
        {
            cTextField.SetText(text);
        }

        internal string GetText() => cTextField.GetText();
    }
}
