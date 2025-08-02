using SeeloewenCraft.game.ui.ui_lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SeeloewenCraft.game.ui
{
    internal class GuiScreen : CRectangle
    {
        internal List<CGui> guis = new List<CGui>(); //(Currently) supports two guis being open
        public ObservableCollection<IGuiData> datas = new ObservableCollection<IGuiData>();

        internal GuiScreen() : base(new Color(0f, 0f, 0f, 0f), new Rectangle(-1f, -1f, 1f, 1f)) //Goes over the entire screen as an invisible background to catch click events
        {
            datas.CollectionChanged += GuiData_CollectionChanged;
        }

        protected override void OnUpdate()
        {
            foreach (CGui gui in guis)
            {
                if (gui == null) return;

                gui.Update();
            }
        }

        protected override void OnRender()
        {
            if (guis.Count == 0) return; //If no gui is available don't render anything

            if (guis.Count == 1)
            {
                int x = width / 2 - guis[0].width / 2;
                int y = height - (height / 2 - guis[0].height / 2);

                if (guis[0].GetBounds().x1P != x || guis[0].GetBounds().y1P != y)
                {
                    guis[0].MoveTo(x, y);
                }
            }
            else
            {
                int x1 = width / 2 - guis[0].width / 2;
                int y1 = height - (height / 2 - (guis[0].height + guis[1].height) / 2);

                int x2 = width / 2 - guis[1].width / 2;
                int y2 = height - (y1 + guis[0].height);

                guis[0].MoveTo(x1, y1);
                guis[1].MoveTo(x2, y2);
            }

            base.OnRender();
        }

        private void GuiData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Update guis when the gui data collection changes
            guis.Clear();
            ClearChildren();

            foreach(IGuiData data in datas)
            {
                CGui gui = GetGui(data);
                guis.Add(gui);
                AddChild(gui);
            }
        }

        public static CGui GetGui(IGuiData data) => data.guiId switch
        {
            "inventory" => new CInventory(data),
            _ => null
        };
    }
}
