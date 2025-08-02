using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.ui
{
    public interface IGuiData
    {
        public string guiId { get; set; }

        public void Show()
        {
            Screen.guiScreen.datas.Add(this);
        }

        public void Hide() 
        {
            Screen.guiScreen.datas.Remove(this);
        }
    }
}
