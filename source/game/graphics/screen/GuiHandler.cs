using SeeloewenCraft.game.core.entities.inventory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SeeloewenCraft.game.graphics
{
    internal class GuiHandler
    {
        internal List<CGui> guis = new List<CGui>(); //(Currently) supports two guis being open
        public ObservableCollection<IGuiData> guiData = new ObservableCollection<IGuiData>();

        internal GuiHandler()
        {
            guiData.CollectionChanged += GuiData_CollectionChanged;
        }

        internal void HideGuis()
        {
            for (int i = guiData.Count - 1; i >= 0; i--) guiData[i].Hide();
        }

        internal void ResetGuis()
        {
            //Update guis when the gui data collection changes
            guis.Clear();

            foreach (IGuiData data in guiData)
            {
                CGui gui = GetGui(data);
                guis.Add(gui);
            }

            Screen.guiRoot.Hide();
            Screen.guiRoot.Show();
        }

        private void GuiData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetGuis();
        }

        public CGui GetGui(IGuiData data) => data.guiId switch
        {
            "inventory" => new CInventory(data),
            "crafting_handler" => new CCrafting(data),
            "unchisel_handler" => new CUnchiseler(data),
            "creative_inventory" => new CCreativeInventory(data),
            "notification_handler" => new CNotificationGui(),
            _ => null
        };

        internal void SetMouseFollower(InventorySlot slot)
        {
            //Redirect event to mouse follower
            ((GuiScreen)Screen.guiRoot.component).mouseFollowerSlot.SetSlot(slot);
        }
    }
}
