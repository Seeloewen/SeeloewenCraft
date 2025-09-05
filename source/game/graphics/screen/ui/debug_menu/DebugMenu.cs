using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics
{

    public interface IDebugMenuTargetable
    {
        public void UpdateDebugMenu();

        public void AddDebugMenu();

    }

    public static class DebugMenu
    {

        public enum Section { WORLD, PLAYER, TARGETED }

        static Dictionary<string, string> linesWorld = new Dictionary<string, string>();
        static Dictionary<string, string> linesPlayer = new Dictionary<string, string>();
        static Dictionary<string, string> linesTargeted = new Dictionary<string, string>();

        public static IDebugMenuTargetable target;

        internal static void Render()
        {
            if (target != null) target.UpdateDebugMenu();
            int y = 125;
            PrimitiveRenderer.Begin();
            foreach (var line in linesWorld)
            {
                int w = TextRenderer.GetWidth($"{line.Key}={line.Value}", 2);
                PrimitiveRenderer.DrawRectangle(5 - 2, y - 2, 5 + w + 2, y + 18, 0.8f, 0.8f, 0.8f, 0.5f);
                y += 20;
            }

            y += 40;
            foreach (var line in linesPlayer)
            {
                int w = TextRenderer.GetWidth($"{line.Key}={line.Value}", 2);
                PrimitiveRenderer.DrawRectangle(5 - 2, y - 2, 5 + w + 2, y + 18, 0.8f, 0.8f, 0.8f, 0.5f);
                y += 20;
            }

            y = 125;
            foreach (var line in linesTargeted)
            {
                string s = $"{line.Key}={line.Value}";
                int w = TextRenderer.GetWidth(s, 2);
                PrimitiveRenderer.DrawRectangle(Resolution.WIDTH - 5 - w - 2, y - 2, Resolution.WIDTH - 3, y + 18, 0.8f, 0.8f, 0.8f, 0.5f);
                y += 20;
            }
            PrimitiveRenderer.End();

            y = 125;
            TextRenderer.Begin();
            foreach (var line in linesWorld)
            {
                TextRenderer.Draw($"{line.Key}={line.Value}", 5, y, 2);
                y += 20;
            }

            y += 40;
            foreach (var line in linesPlayer)
            {
                TextRenderer.Draw($"{line.Key}={line.Value}", 5, y, 2);
                y += 20;
            }

            y = 125;
            foreach (var line in linesTargeted)
            {
                string s = $"{line.Key}={line.Value}";
                TextRenderer.Draw(s, Resolution.WIDTH - 5 - TextRenderer.GetWidth(s, 2), y, 2);
                y += 20;
            }
            TextRenderer.End();
        }

        internal static void NewTargeted(IDebugMenuTargetable target)
        {
            linesTargeted = new Dictionary<string, string>();
            DebugMenu.target = target;
            if (target != null) target.AddDebugMenu();
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

