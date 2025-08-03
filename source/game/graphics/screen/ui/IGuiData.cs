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
        }

        public void Hide() 
        {
            Screen.guiData.Remove(this);
        }
    }
}
