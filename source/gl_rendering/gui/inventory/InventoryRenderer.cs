using SeeloewenCraft.gl_rendering.math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.gl_rendering
{
    internal class InventoryRenderer
    {

        public static void Render(PrimitiveRenderer renderer)
        {

            renderer.Begin();


            float s = 0.1f, d = 0.02f, m = 0.05f;

            float h = 4 * s + 4 * d + 2 * m;
            h *= 16 / 9.0f;
            float w = 9*s + 8*d + 2 * m;



            renderer.DrawRectangle(w / 2, h / 2, -w / 2, -h / 2, 0.5f, 0.5f, 0.5f); 

            float oX = -w / 2;
            float oY = h / 2;

            for(int x = 0; x < 9; x++)
            {
                float x1 = oX + m + x * (d + s);
                float x2 = x1 + s;
                float y1 = oY - (16 / 9.0f) * (m + d + 3 * (d + s));
                float y2 = y1 - s * 16 / 9.0f;
                renderer.DrawRectangle(x1, y1, x2, y2, 0.9f, 0.9f, 0.9f);

                for(int y = 0; y < 3; y++) {
                    x1 = oX + m + x * (d+s);
                    x2 = x1 + s;
                    y1 = oY - (16/9.0f)*(m + y * (d + s));
                    y2 = y1 - s*16/9.0f;
                    renderer.DrawRectangle(x1, y1, x2, y2, 0.9f, 0.9f, 0.9f);
                }


            }

            renderer.End();

        }

    }
}
