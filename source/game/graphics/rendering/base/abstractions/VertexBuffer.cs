
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics
{

    internal class VBLayout
    {
        internal record Attribute(int count);

        internal List<Attribute> attributes;

        internal VBLayout()
        {
            attributes = new List<Attribute>();
        }

        internal VBLayout AddAttribute(int count)
        {
            attributes.Add(new Attribute(count));
            return this;
        }

        internal int GetStride()
        {
            int s = 0;
            foreach (Attribute a in attributes) s += a.count;
            return s * sizeof(float);
        }
    }

    internal class VertexBuffer
    {
        private readonly int count;

        private readonly int vao, vbo;

        internal VertexBuffer(VBLayout layout, float[] vertices)
                : this(layout, vertices.Length)
        {
            SetVertices(vertices);
        }

        internal VertexBuffer(VBLayout layout, int vertexCount)
        {
            this.count = vertexCount;

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, count * sizeof(float), 0, BufferUsageHint.StaticDraw);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            int stride = layout.GetStride();
            int offset = 0;
            for (int i = 0; i < layout.attributes.Count; i++)
            {
                int count = layout.attributes[i].count;
                GL.EnableVertexArrayAttrib(vao, i);
                GL.VertexAttribPointer(i, count, VertexAttribPointerType.Float, false, stride, offset);
                offset += count * sizeof(float);
            }
        }



        internal void SetVertices(float[] vertices)
        {
            Debug.Assert(vertices.Length <= count); //could be <=
            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * sizeof(float), vertices);
        }

        internal void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindVertexArray(vao);
        }

    }
}
