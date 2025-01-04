using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;

namespace SeeloewenCraft.gl_rendering
{

    public interface IDebugMenuTargetable
    {
        public void UpdateDebugMenu();
        
        public void AddDebugMenu();

    }

    public static class DebugMenu
    {

        public enum Section { WORLD, PLAYER, TARGETED}

        static Dictionary<string, string> linesWorld = new Dictionary<string, string>();
        static Dictionary<string, string> linesPlayer = new Dictionary<string, string>();
        static Dictionary<string, string> linesTargeted = new Dictionary<string, string>();

        private static IDebugMenuTargetable target;

        internal static void Render(TextRenderer textRenderer)
        {
            target.UpdateDebugMenu();
            int y = 125;
            foreach(var line in linesWorld)
            {
                textRenderer.Draw($"{line.Key}={line.Value}", 5, y, 2);
                y += 20;
            }

            y = 125;
            foreach(var line in linesTargeted)
            {
                string s = $"{line.Key}={line.Value}";
                textRenderer.Draw(s, Resolution.WIDTH - 5 - TextRenderer.GetWidth(s, 2), y, 2);
                y += 20;
            }

        }

        internal static void NewTargeted (IDebugMenuTargetable target)
        {
            linesTargeted = new Dictionary<string, string>();
            DebugMenu.target = target;
            target.AddDebugMenu();
        }


        public static void AddLine(Section section, string name)
        {
            AddLine(section, name, "");
        }

        public static void AddLine(Section section, string name, string value)
        {
            var lines = GetLines(section);
            //Debug.Assert(!lines.ContainsKey(name));
            lines[name] = value;
        }


        public static void RemoveLine(Section section, string name)
        {
            var lines = GetLines(section);
            Debug.Assert(lines.ContainsKey(name));
            lines.Remove(name);
        }

        public static void UpdateLine(Section section, string name, string value)
        {
            var lines = GetLines(section);
            Debug.Assert(lines.ContainsKey(name));
            lines[name] = value;
        }

        

        static Dictionary<string, string> GetLines(Section section)
        {
            return section switch
            {
                Section.WORLD => linesWorld,
                Section.PLAYER => linesPlayer,
                Section.TARGETED => linesTargeted,
            };
        }

    }


}
