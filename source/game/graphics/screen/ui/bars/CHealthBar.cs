using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CHealthBar : CProgressBar
    {
        private readonly string description = "Health";
        private readonly CText cContent; 
        private readonly CBorder cBorder;

        internal CHealthBar() : base(new Color(0.69f), new Color(1f, 0f, 0f), new Rectangle(Resolution.WIDTH - 420, 20, Resolution.WIDTH - 20, 55))
        {
            cContent = new CText(description, 2, new TextLayout(Resolution.WIDTH - 410, TextHAlignment.LEFT, 30, TextVAlignment.TOP));
            AddChild(cContent);

            cBorder = new CBorder(2, new Color(0.4f));
            AddChild(cBorder);
        }

        protected override void OnUpdate(double dt)
        {
            SetProgress((float)(Game.world.player.hp / Player.MAX_HP));
            cContent.SetText($"{description} ({Game.world.player.hp}/{Player.MAX_HP})");
        }
    }
}
