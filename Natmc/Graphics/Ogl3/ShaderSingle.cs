using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Ogl3
{
    public class ShaderSingle : IDisposable
    {
        public int Handle { get; protected set; }
        public ShaderType Type { get; protected set; }

        public ShaderSingle(int handle, ShaderType type)
        {
            Handle = handle;
            Type = type;
        }

        public static ShaderSingle TryCompile(string source, ShaderType type, out string errorMessage)
        {
            var handle = GL.CreateShader(type);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);

            GL.GetShader(handle, ShaderParameter.CompileStatus, out var status);
            if (status == 0)
            {
                GL.GetShaderInfoLog(handle, out errorMessage);
                GL.DeleteShader(handle);
                return null;
            }

            errorMessage = null;
            return new ShaderSingle(handle, type);
        }

        public static ShaderSingle Compile(string source, ShaderType type)
        {
            var shader = TryCompile(source, type, out var errorMessage);
            if (shader == null)
                throw new FormatException($"Couldn't compile shader of type {type}:\n\n{errorMessage}");
            return shader;
        }

        public void Dispose() => GL.DeleteShader(Handle);
    }
}
