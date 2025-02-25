using SeeloewenCraft.game.ui.ui_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.ui
{
    class CSlot : CRectangle
    {
        CText cText;
        CTexture cTexture;

        public CSlot(Rectangle bounds) : base(new Color(0.8f, 0.8f, 0.8f), bounds)
        {
            //Item Texture
            cTexture = new CTexture(ItemRenderer.GetTextureMap(), null, new Rectangle(bounds.x1P + 5, bounds.y1P + 5, bounds.x2P - 5, bounds.y2P - 5));
            AddChild(cTexture);

            //Item 
            cText = new CText("", 2, new TextLayout(bounds.x2P - 2, TextHAlignment.RIGHT, bounds.y2P - 10, TextVAlignment.CENTER)); 
            AddChild(cText);
        }

        public void SetAmount(int amount)
        {            
            cText.SetText(amount > 1 ? amount.ToString() : "");
        }

        public void SetItem(string id)
        {
            cTexture.SetId(id);
        }

        public void SetDurability()
        {
            throw new NotImplementedException();
        }
    }
}
