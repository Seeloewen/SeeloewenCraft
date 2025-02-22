using SeeloewenCraft.game.ui.ui_lib;

namespace SeeloewenCraft.game.ui
{
    internal class InvUI : CRectangle
    {

        internal InvUI() : base(new Color(0f,0f,0f,0f), new Rectangle(-1f,-1f,1f,1f))
        {

            int width = 9 * IS.slotSize + 12 * IS.edgeSize;
            int height = 4 * IS.slotSize + 8 * IS.edgeSize;


            var backplate = new CRectangle(new Color(.9f), new Rectangle(IS.mx - width / 2, IS.my - height / 2, IS.mx + width / 2, IS.my + height / 2));
            backplate.AddChild(new CBorder(3, new Color(.0f)));
            AddChild(backplate);


            for(int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    AddChild(new InvSlotUI(x, y, ItemRenderer.getTextureMap()));
                }
            }


        }





    }
}
