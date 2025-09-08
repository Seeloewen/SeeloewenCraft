using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics.ui_lib;

public class CTextBlock : Component
{

    public CTextBlock(string text, int size, int maxWidth, TextLayout layout) : base(new Rectangle())
    {
        string[] texts = Split(text, size, maxWidth);
        int gap = 2;
        int sizeY = 8 * size * (texts.Length + (texts.Length - 1) * gap);
        int startY = layout.textVAlignment switch
        {
            TextVAlignment.TOP => layout.yAnchor,
            TextVAlignment.CENTER => layout.yAnchor - sizeY / 4,
            TextVAlignment.BOTTOM => layout.yAnchor - sizeY / 2
        };
        int x1 = int.MaxValue, x2 = int.MinValue;
        for (int i = 0; i < texts.Length; i++)
        {
            var l = new TextLayout(layout.xAnchor, layout.textHAlignment, startY + i * 10 * size, TextVAlignment.TOP);
            var r = l.CalcBounds(texts[i], size);
            x1 = Math.Min(x1, r.x1P);
            x2 = Math.Max(x2, r.x2P);
            AddChild(new CText(texts[i], size, l));
        }

        SetBounds(new Rectangle(x1, startY, x2, startY + sizeY));
    }


    static string[] Split(string text, int size, int maxWidth)
    {
        List<string> split = new List<string>();
        string currentLine = "";
        string currentWord = "";
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            
            switch (c)
            {
                case '\n':
                    if (TextRenderer.GetWidth(currentLine + currentWord, size) > maxWidth)
                    {
                        if (TextRenderer.GetWidth(currentWord, size) > maxWidth)
                        {
                            Debug.Assert(currentLine.Length == 0, "logic error");
                            split.Add(currentWord);
                            currentWord = "";
                        }
                        else
                        {
                            currentLine += currentWord;
                            split.Add(currentLine);
                            currentLine = "";
                            currentWord = "";
                        }
                    }
                    else
                    {
                        split.Add(currentLine);
                        currentLine = "";
                        split.Add(currentWord);
                        currentWord = "";
                    }
                    break;
                case ' ':
                    if (TextRenderer.GetWidth(currentLine + currentWord, size) > maxWidth)
                    {
                        if (TextRenderer.GetWidth(currentWord, size) > maxWidth)
                        {
                            Debug.Assert(currentLine.Length == 0, "logic error");
                            split.Add(currentWord);
                            currentWord = "";
                        }
                        else
                        {
                            split.Add(currentLine);
                            currentLine = currentWord;
                            currentWord = " ";
                        }
                    }
                    else
                    {
                        currentLine += currentWord;
                        currentWord = " ";
                    }
                    break;
                default:
                    currentWord += c;
                    break;
            }
        }

        if (TextRenderer.GetWidth(currentLine + currentWord, size) > maxWidth)
        {
            if (TextRenderer.GetWidth(currentWord, size) > maxWidth)
            {
                Debug.Assert(currentLine.Length == 0, "logic error");
                split.Add(currentWord);
            }
            else
            {
                split.Add(currentLine);
                split.Add(currentWord);
            }
        }
        else
        {
            split.Add(currentLine + currentWord);
        }

        return split.ToArray();
    }
    
}