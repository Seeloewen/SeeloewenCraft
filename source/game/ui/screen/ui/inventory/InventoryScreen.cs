using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Geolocation;

namespace SeeloewenCraft.game.ui
{
    internal static class InventoryScreen
    {
        static Inventory inventory { get => Game.world.player.inventory; }

        static InvSlotScreen[] slotScreens;

        static bool itemSelected;
        static string selectedItemID;
        static int selectedItemAmount;

        static bool pressedLeft;
        static bool pressedRight;

        static bool pressedNewLeft;
        static bool unpressedNewLeft;
        static bool pressedNewRight;
        static bool unpressedNewRight;

        static int mouseX;
        static int mouseY;
        static int currentSlotIndex;

        internal static void Update()
        {
            #region pressed updates
            pressedNewLeft = false;
            unpressedNewLeft = false;
            pressedNewRight = false;
            unpressedNewRight = false;
            if (pressedLeft != InputHandler.pressedLeft)
            {
                if (pressedLeft)
                {
                    pressedLeft = true;
                    pressedNewLeft = true;
                }
                else
                {
                    pressedLeft = false;
                    unpressedNewLeft = false;
                }
            }
            if (pressedRight != InputHandler.pressedRight)
            {
                if (pressedRight)
                {
                    pressedRight = true;
                    pressedNewRight = true;
                }
                else
                {
                    pressedRight = false;
                    unpressedNewRight = false;
                }
            }
            #endregion

            #region mouse updates
            bool leftSlot = false;
            int oldSlotIndex = -1;
            bool enteredSlot = false;
            int newSlotIndex = -1;
            if (mouseX != InputHandler.mouseXPixel || mouseY != InputHandler.mouseYPixel)
            {
                mouseX = InputHandler.mouseXPixel;
                mouseY = InputHandler.mouseYPixel;
                if(currentSlotIndex != -1) leftSlot = !slotScreens[currentSlotIndex].IsInBounds(mouseX, mouseY);
                if (leftSlot)
                {
                    oldSlotIndex = currentSlotIndex;
                    currentSlotIndex = -1;
                }
                if (leftSlot || currentSlotIndex == -1)
                {
                    for (int i = 0; i < 36; i++)
                    {
                        if (slotScreens[i].IsInBounds(mouseX, mouseY))
                        {
                            enteredSlot = true;
                            newSlotIndex = i;
                            currentSlotIndex = i;
                            break;
                        }
                    }
                }
            }
            #endregion


            if (leftSlot)
            {
                slotScreens[oldSlotIndex].hovered = false;
            }
            if(enteredSlot)
            {
                slotScreens[newSlotIndex].hovered = true;
            }
            

        }


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

        internal static void Render()
        {
            PrimitiveRenderer.Begin();

            (int mx, int my) = Resolution.ScreenToPixel(0f, 0f);

            //int sizeX = 600, sizeY = 400;

            var background = new Rectangle(mx - InvSizes.slotSize * 4 - InvSizes.slotSize / 2 - 5 * InvSizes.edgeSize,
                my - InvSizes.yOffset - InvSizes.edgeSize,
                mx + InvSizes.slotSize * 4 + InvSizes.slotSize / 2 + 5 * InvSizes.edgeSize,
                my - InvSizes.yOffset + InvSizes.slotSize * 4 + InvSizes.edgeSize * 5);

            PrimitiveRenderer.DrawRectangle(background, new ColorI(0.4f, 0.4f, 0.4f));

            for (int i = 0; i < 4 * 9; i++)
            {
                slotScreens[i].RenderBack();
            }

            PrimitiveRenderer.End();


        }


    }
}
