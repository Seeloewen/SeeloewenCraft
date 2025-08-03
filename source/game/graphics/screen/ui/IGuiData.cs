using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.graphics
{
    public interface IGuiData
    {
        public string guiId { get; set; }

        public void Show()
        {
            Screen.guiData.Add(this);
            Screen.showGui = true;
        }

        public void Hide() 
        {
            Screen.guiData.Remove(this);
            Screen.showGui = false;
        }
    }
}
