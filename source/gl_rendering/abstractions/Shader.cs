using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;

using OpenTK.Graphics.OpenGL;

namespace SeeloewenCraft.gl_rendering
{
    internal class Shader
    {

        private int programID;

        public Shader() { 
            programID = GL.CreateProgram();
            int vert = CreateShader(ShaderType.VertexShader, "shader/vert.glsl");
            int frag = CreateShader(ShaderType.FragmentShader, "shader/frag.glsl");
            GL.AttachShader(programID, vert);
            GL.AttachShader(programID, frag);
            GL.LinkProgram(programID);
            GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetProgramInfoLog(programID);
                Log.Write($"Program link error: {log}", "Error");
            }

            GL.DetachShader(programID, vert);
            GL.DetachShader(programID, frag);

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }

        public void Use()
        {
            GL.UseProgram(programID);
        }

        private int CreateShader(ShaderType type, string path) {
            int shaderID = GL.CreateShader(type);
            string source = ReadResourceToString(path);
            GL.ShaderSource(shaderID, source);
            GL.CompileShader(shaderID);
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetShaderInfoLog(shaderID);
                Log.Write($"Shader (type={type}) compile error: {log}", "Error");
            }
            return shaderID;
        }

        private static string ReadResourceToString(string path)
        {
            Uri uri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/{path}", UriKind.Absolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            string text;
            using (StreamReader reader = new(info.Stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
