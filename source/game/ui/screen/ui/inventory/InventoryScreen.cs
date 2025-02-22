

namespace SeeloewenCraft.game.ui
{
    internal static class InventoryScreen
    {
        static Inventory inventory { get => Game.world.player.inventory; }

        static InvSlotScreen[] slotScreens;
        //static InvSlotScreen currentSlot { get => slotScreens[currentSlotIndex]; }


        static bool itemSelected;
        static string selectedItemID;
        static int selectedItemAmount;

        static bool pressedLeft;
        static bool pressedRight;


        static int mouseX;
        static int mouseY;
        static int currentSlotIndex;

        static bool notPressedOnCurrentSlot;

        internal static void Update()
        {
            #region pressed updates
            bool pressedNewLeft = false;
            bool unpressedNewLeft = false;
            bool pressedNewRight = false;
            bool unpressedNewRight = false;
            if(!pressedLeft ) notPressedOnCurrentSlot = true;
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
                    unpressedNewLeft = true;
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
                            notPressedOnCurrentSlot = false;
                            newSlotIndex = i;
                            currentSlotIndex = i;
                            break;
                        }
                    }
                }
            }
            #endregion

            if (pressedNewLeft) Log.WriteD("pressedNewLeft");
            if (pressedNewRight) Log.WriteD("pressednewRight");

            if (leftSlot)
            {
                slotScreens[oldSlotIndex].hovered = false;
            }
            if(enteredSlot)
            {
                slotScreens[newSlotIndex].hovered = true;
            }
            
            if(pressedNewLeft)
            {
                if (notPressedOnCurrentSlot)
                {
                    slotScreens[currentSlotIndex].pressed = true;
                }
            }
            if(unpressedNewLeft)
            {
                //if(currentSlotIndex == -1) slotScreens[currentSlotIndex].pressed = false; why tf doesnt this work??
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

            

            //int sizeX = 600, sizeY = 400;

            var background = new Rectangle(InvSizes.mx - InvSizes.slotSize * 4 - InvSizes.slotSize / 2 - 5 * InvSizes.edgeSize,
                InvSizes.my - InvSizes.yOffset - InvSizes.edgeSize,
                InvSizes.mx + InvSizes.slotSize * 4 + InvSizes.slotSize / 2 + 5 * InvSizes.edgeSize,
                InvSizes.my - InvSizes.yOffset + InvSizes.slotSize * 4 + InvSizes.edgeSize * 5);

            PrimitiveRenderer.DrawRectangle(background, new Color(0.3f, 0.3f, 0.3f));

            for (int i = 0; i < 4 * 9; i++)
            {
                slotScreens[i].RenderBack();
            }

            PrimitiveRenderer.End();

            TextureRenderer.Begin();

            for (int i = 0; i < 4 * 9; i++)
            {
                slotScreens[i].RenderMid();
            }

            TextureRenderer.End();

        }


    }
}
