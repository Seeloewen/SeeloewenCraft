

using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.IO;


namespace SeeloewenCraft.gl_rendering
{
    internal class BlockRenderer
    {

        float[] buffer;
        int index;

        float blockLength = 0.01f;
        float blockXAnchor = -0.8f;
        float blockYAnchor = -0.8f;
        float ratio = 16 / 9.0f;

        BlockTextureMap textureMap;

        Shader shader;
        VertexBuffer vertexBuffer;

        bool drawing = false;

        internal BlockRenderer(BlockTextureMap textureMap)
        {
            this.textureMap = textureMap;
            shader = new Shader();
            vertexBuffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2), 1024);
        }



        internal void DrawBlock(string blockID, int blockX, int blockY)
        {
            Debug.Assert(drawing); //this assert isnt working (secret Axogurkel cameo)
            if (index + 4 * 6 > buffer.Length)
            {
                End();
                Begin();
            }
            (float s1, float t1, float s2, float t2) = textureMap.getTexture(blockID);
            float x1 = blockXAnchor + blockLength * blockX;
            float y1 = blockYAnchor + blockLength * blockY * ratio;
            float x2 = x1 + blockLength;
            float y2 = y1 + blockLength * ratio;
            Put(x1, y1, s1, t1);
            Put(x2, y1, s2, t1);
            Put(x1, y2, s1, t2);
            Put(x2, y1, s2, t1);
            Put(x1, y2, s1, t2);
            Put(x2, y2, s2, t2);

        }

        private void Put(float x, float y, float s, float t)
        {
            buffer[index++] = x;
            buffer[index++] = y;
            buffer[index++] = s;
            buffer[index++] = t;
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
