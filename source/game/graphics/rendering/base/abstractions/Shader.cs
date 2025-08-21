using OpenTK.Graphics.OpenGL4;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SeeloewenCraft.game.graphics
{
    internal class Shader
    {

        private int programID;

        internal Shader(string path)
        {
            programID = GL.CreateProgram();
            int vert = CreateShader(ShaderType.VertexShader, $"{path}.vert.glsl");
            int frag = CreateShader(ShaderType.FragmentShader, $"{path}.frag.glsl");
            GL.AttachShader(programID, vert);
            GL.AttachShader(programID, frag);
            GL.LinkProgram(programID);
            GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetProgramInfoLog(programID);
                MessageBox.Show($"Program link error: {log}", "Error");
            }

            GL.DetachShader(programID, vert);
            GL.DetachShader(programID, frag);

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }

        ~Shader()
        {
            //GL.DeleteProgram(programID);
        }

        internal void SetUniform(string name, int value)
        {
            int location = GL.GetUniformLocation(programID, name);
            GL.Uniform1(location, value);
        }

        public void Use()
        {
            GL.UseProgram(programID);
        }

        private int CreateShader(ShaderType type, string path)
        {
            int shaderID = GL.CreateShader(type);
            string source = ReadResourceToString(path);
            GL.ShaderSource(shaderID, source);
            GL.CompileShader(shaderID);
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetShaderInfoLog(shaderID);
                MessageBox.Show($"Shader (type={type}) compile error: {log}", "Error");
            }
            return shaderID;
        }

        private static string ReadResourceToString(string path)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.{path}");
            using (StreamReader reader = new(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
