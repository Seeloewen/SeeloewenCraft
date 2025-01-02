

using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using System.IO;


namespace SeeloewenCraft.gl_rendering
{
    internal class BlockRenderer
    {

        float[] buffer;
        int index;


        BlockTextureMap textureMap;

        Shader shader;
        VertexBuffer vertexBuffer;

        bool drawing = false;

        internal BlockRenderer(BlockTextureMap textureMap)
        {
            this.textureMap = textureMap;
            shader = new Shader("shader/blockworld");
            vertexBuffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2).AddAttribute(1), 1024);
        }


        internal void DrawBlock(BlockRenderInfo info)
        {
            DrawBlock(info.GetTextureID(), info.x, info.y, info.isBackground);
            if (info.hasForegroundBlock) DrawBlock(info.GetForegroundTextureID(), info.x, info.y, false);
        }

        internal void DrawBlock(string blockID, int blockX, int blockY, bool isBackground)
        {
            Debug.Assert(drawing); //this assert isnt working (secret Axogurkel cameo)
            if (index + 5 * 6 > buffer.Length)
            {
                End();
                Begin();
            }
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture(blockID);
            float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * blockX;
            float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * blockY * Resolution.RATIO;
            float x2 = x1 + GameCamera.blockLength;
            float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;

            float g = isBackground ? 0.69f : 1.0f;
            Put(x1, y1, s1, t1, g);
            Put(x2, y1, s2, t1, g);
            Put(x1, y2, s1, t2, g);
            Put(x2, y1, s2, t1, g);
            Put(x1, y2, s1, t2, g);
            Put(x2, y2, s2, t2, g);

        }

        private void Put(float x, float y, float s, float t, float g)
        {
            buffer[index++] = x;
            buffer[index++] = y;
            buffer[index++] = s;
            buffer[index++] = t;
            buffer[index++] = g;
        }

        internal void Begin()
        {
            buffer = new float[1024];
            index = 0;
            drawing = true;
        }

        internal void End()
        {
            drawing = false;
            vertexBuffer.SetVertices(buffer);
            shader.Use();
            vertexBuffer.Bind();
            textureMap.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }
    }
}
