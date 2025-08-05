using SeeloewenCraft.game.graphics.ui_lib;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal class CProgressBar : CRectangle
    {
        private CRectangle cProgress;

        internal CProgressBar(Color backColor, Color progressColor, Rectangle bounds) : base(backColor, bounds)
        {
            cProgress = new CRectangle(progressColor, new Rectangle(0, 0, 0, 0));
            AddChild(cProgress);
        }

        internal void SetProgress(float progress) //Progress should be between 0 and 1
        {
            cProgress.SetBounds(new Rectangle(bounds.x1P, bounds.y1P, bounds.x1P + (int)Math.Round(width * progress), bounds.y2P));
        }

        internal void SetProgressColor(Color c)
        {
            cProgress.SetColor(c);
        }
    }
}
