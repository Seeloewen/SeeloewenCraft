using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeeloewenCraft.game.ui
{
    internal static class InventoryScreen
    {
        static Inventory inventory { get => Game.world.player.inventory; }

        static bool itemSelected;
        static string selectedItemID;
        static int selectedItemAmount;

        static bool pressedLeft;
        static bool pressedRight;

        static InvSlotScreen[] slotScreens;

        internal static void Init()
        {

            slotScreens = new InvSlotScreen[4 * 9];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    slotScreens[y * 9 + x] = new InvSlotScreen(x, y);
                }
            }
        }

        internal static void Update()
        {



        }


        internal static void Render()
        {
            PrimitiveRenderer.Begin();

            (int mx, int my) = Resolution.ScreenToPixel(0f, 0f);

            int sizeX = 600, sizeY = 400;

            var background = new Rectangle(mx-sizeX/2, my-sizeY/2, mx + sizeX / 2, my + sizeY / 2);

            //PrimitiveRenderer.DrawRectangle(background, new ColorI(1f, 1f, 1f));

            for (int i = 0; i < 4 * 9; i++)
            {
                slotScreens[i].RenderBack();
            }

            PrimitiveRenderer.End();


        }


    }
}
