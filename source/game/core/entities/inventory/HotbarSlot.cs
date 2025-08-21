using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft.game.core.entities.inventory
{
    public class HotbarSlot
    {
        //References
        public Border bdrSlot = new Border();
        public Canvas cvsSlot = new Canvas();
        public TextBlock tblItemAmount = new TextBlock();
        public ProgressBar pbDurability = new ProgressBar();
        public InventorySlot slot;

        //Variables
        public int xPos;
        public bool isSelected;

        //-- Constructor --//

        public HotbarSlot(int xPos, InventorySlot slot)
        {
            //Set the hotbar slot attributes
            this.xPos = xPos;
            this.slot = slot;
            this.slot.hotbarSlot = this;

            //Create the hotbar slot border, canvas and textblock
            bdrSlot.Height = 75;
            bdrSlot.Width = 75;
            bdrSlot.BorderThickness = new Thickness(3, 3, 3, 3);
            bdrSlot.Background = new SolidColorBrush(Colors.DarkGray);
            bdrSlot.Child = cvsSlot;
            bdrSlot.MouseDown += bdrSlot_MouseDown;

            //Setup progressbar
            pbDurability.Width = 36;
            pbDurability.Height = 7;
            Canvas.SetTop(pbDurability, 48);
            Canvas.SetLeft(pbDurability, 0);
            pbDurability.Visibility = Visibility.Hidden;
            cvsSlot.Children.Add(pbDurability);

            //Setup textblock
            tblItemAmount.FontSize = 18;
            cvsSlot.Children.Add(tblItemAmount);
            Canvas.SetLeft(tblItemAmount, 40);
            Canvas.SetTop(tblItemAmount, 38);

            //Setup canvas
            cvsSlot.Margin = new Thickness(0, 0, 0, 0);
            cvsSlot.Height = 55;
            cvsSlot.Width = 55;
        }

        //-- Custom Methods --//

        public void Select()
        {
            //Unselect all other slots
            foreach (HotbarSlot slot in slot.inventory.hotbarSlotList)
            {
                slot.isSelected = false;
            }

            //Select the slot
            isSelected = true;
        }

        private void bdrSlot_MouseDown(object sender, EventArgs e)
        {
            //Select the slot when being clicked
            Select();
        }
    }
}
