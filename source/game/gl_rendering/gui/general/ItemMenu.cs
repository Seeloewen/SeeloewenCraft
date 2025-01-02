using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.gl_rendering
{


    internal class ItemSlot
    {
        internal string itemID;
        internal int amount;

        internal bool hovered;

        internal int startX;
        internal int startY;
        internal int endX;
        internal int endY;

        internal bool CheckBounds(int x, int y)
        {
            return startX <= x && startY <= y && endX > x && endY > y;
        }
    }


    internal class ItemMenu // inventory or crafting menus
    {

        Dictionary<int, ItemSlot> itemSlots;

        ItemSlot slot = null;

        string itemID = null;
        int itemAmount = 0;



        #region Interface logic

        internal void Update()
        {
            (int x, int y) = (InputHandler.mouseXPixel, InputHandler.mouseYPixel);
            var newSlot = GetItemSlot(x, y);
            if (newSlot != slot) // slot was switched
            {
                if (slot != null) slot.hovered = false;
                if(newSlot != slot) slot.hovered = true;

            }

        }

        private ItemSlot GetItemSlot(int x, int y)
        {
            foreach (var pair in itemSlots)
            {
                ItemSlot slot = pair.Value;
                if (slot.CheckBounds(x, y)) return slot;
            }
            return null;
        }

        internal void OnMouseMove(int x, int y)
        {

        }

        internal void OnMouseDownLeft(int x, int y)
        {

        }

        internal void OnMouseDownRight(int x, int y)
        {

        }

        internal void OnMouseUpLeft(int x, int y)
        {

        }

        internal void OnMouseUpRight(int x, int y)
        {

        }


        #endregion
        #region Rendering



        #endregion

    }
}
