using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class HotbarSlot
    {
        //References
        private World world;
        public Border bdrSlot = new Border();
        public Canvas cvsSlot = new Canvas();
        public TextBlock tblItemAmount = new TextBlock();
        public InventorySlot slot;

        //Variables
        public int xPos;
        public bool isSelected;

        //-- Constructor --//

        public HotbarSlot(World world, int xPos, InventorySlot slot)
        {
            //Set the hotbar slot attributes
            this.world = world;
            this.xPos = xPos;
            this.slot = slot;

            //Create the hotbar slot border, canvas and textblock
            bdrSlot.Height = 75;
            bdrSlot.Width = 75;
            bdrSlot.BorderThickness = new Thickness(3, 3, 3, 3);
            bdrSlot.Background = new SolidColorBrush(Colors.DarkGray);
            bdrSlot.Child = cvsSlot;
            bdrSlot.MouseDown += bdrSlot_MouseDown;

            cvsSlot.Margin = new Thickness(0, 0, 0, 0);
            cvsSlot.Height = 55;
            cvsSlot.Width = 55;

            tblItemAmount.FontSize = 18;
            cvsSlot.Children.Add(tblItemAmount);
            Canvas.SetLeft(tblItemAmount, 33);
            Canvas.SetTop(tblItemAmount, 30);
        }

        //-- Custom Methods --//

        public void Select()
        {           
            //Unselect all other slots
            foreach (HotbarSlot slot in slot.inventory.hotbarSlotList)
            {
                slot.bdrSlot.BorderBrush = null;
                slot.bdrSlot.BorderThickness = new Thickness(3, 3, 3, 3);
                slot.isSelected = false;
            }

            //Select the slot
            bdrSlot.BorderBrush = new SolidColorBrush(Color.FromArgb(100, 18, 18, 18));
            bdrSlot.BorderThickness = new Thickness(5, 5, 5, 5);
            isSelected = true;
        }

        private void bdrSlot_MouseDown(object sender, EventArgs e)
        {
            //Select the slot when being clicked
            Select();
        }
    }
}
