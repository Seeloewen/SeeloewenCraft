using SeeloewenCraft.game.core.commands;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.graphics.ui_lib;
using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    internal class ChatScreen : CRectangle
    {
        internal List<CText> messageTexts = new List<CText>();
        internal CTextBox cTextboxInput;
        internal CScrollPane cMessageArea;
        internal CButton cButtonSend;

        internal ChatScreen() : base(new Color(0f, 0f, 0f, 0f), new Rectangle(0, 0, Resolution.WIDTH, Resolution.HEIGHT))
        {
            cTextboxInput = new CTextBox(10, Resolution.HEIGHT - 60, Resolution.WIDTH - 130, 50, new Color(0.69f));
            cTextboxInput.GetField().SetText("");
            cTextboxInput.GetField().onEnter = cButtonSend_Click;
            cTextboxInput.GetField().onEscape = cTextboxInput_OnEscape;
            cMessageArea = new CScrollPane(new Color(0.8f, 0.8f, 0.8f, 0.4f), new Rectangle(10, 120, 800, Resolution.HEIGHT - 70), 0);
            cButtonSend = new CButton(cButtonSend_Click, "Send", "sc:button_1", "general", new Rectangle(Resolution.WIDTH - 110, Resolution.HEIGHT - 60, Resolution.WIDTH - 10, Resolution.HEIGHT - 10));

            AddChild(cTextboxInput);
            AddChild(cMessageArea);
            AddChild(cButtonSend);
        }

        protected override void OnUpdate(double dt)
        {
            if (Screen.showChat) cTextboxInput.GetField().editMode = true;

            string[] messages = ChatHandler.GetMessages();

            if (messageTexts.Count != messages.Length) //Only update the messages when new ones appear
            {
                messageTexts.Clear();
                cMessageArea.ClearScrollables();

                for (int i = 0; i < messages.Length; i++)
                {
                    string message = messages[i];
                    CText cText = new CText(message, 2, new TextLayout(20, TextHAlignment.LEFT, 130 + i * 30, TextVAlignment.TOP));
                    messageTexts.Add(cText);
                    cMessageArea.AddScrollable(cText);
                }

                int maxI = Math.Max(0, messages.Length * 30 - cMessageArea.height + 10);
                cMessageArea.maxI = maxI;
                cMessageArea.SetScrollOffset(maxI);
            }
        }

        private void cButtonSend_Click()
        {
            if (string.IsNullOrEmpty(cTextboxInput.GetField().GetText())) return;

            ChatHandler.HandlePlayerMessage(cTextboxInput.GetField().GetText(), Settings.nickname);
            cTextboxInput.GetField().SetText("");
            cTextboxInput.GetField().editMode = true;
        }

        private void cTextboxInput_OnEscape()
        {
            Screen.showChat = false;
            cTextboxInput.GetField().editMode = false;
        }
    }
}
