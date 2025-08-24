using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace SeeloewenCraft.game.graphics;
    
internal class BatchRenderer<V> where V : struct, IBatch
{
    private float[] floats;
    private int count;
    private readonly int maxCount;
    
    private VertexBuffer vb;
    private Shader shader;
    private TextureMap textureMap;
    
    public BatchRenderer(Shader shader)
    {
        count = 100;
        this.shader = shader;
        this.maxCount = count;
        textureMap = null;
        floats = new float[maxCount * V.GetSize() * 3];
        vb = new VertexBuffer(V.GetVBLayout(), floats);
    }

    internal void SetTexture(TextureMap textureMap)
    {
        if (textureMap != this.textureMap)
        {
            if (count > 0) Flush();
            this.textureMap = textureMap;
        }
    }

    internal void DrawRect(V v0, V v1, V v2, V v3)
    {
        if (count + 2 >= maxCount) Flush();
        int i = count * V.GetSize() * 3;
        v0.Fill(floats, i);
        i += V.GetSize();
        v1.Fill(floats, i);
        i += V.GetSize();
        v2.Fill(floats, i);
        i += V.GetSize();
        v2.Fill(floats, i);
        i += V.GetSize();
        v3.Fill(floats, i);
        i += V.GetSize();
        v0.Fill(floats, i);
        count += 2;
    }
    
    internal void Begin()
    {
        floats = new float[floats.Length];
        count = 0;
    }

    internal void End()
    {
        if (count == 0) return;
        shader.Use();
        vb.SetVertices(floats);
        vb.Bind();
        if(textureMap != null) textureMap.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, 0, count * 3);
    }

    internal void Flush()
    {
        if (count == 0) return;
        End();
        Begin();
    }
    
    
}