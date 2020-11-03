using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Ogl3
{
    public class ShaderProgram : IDisposable
    {
        public int Handle { get; protected set; }
        public Dictionary<string, int> UniformCache { get; protected set; }

        public ShaderProgram(int handle)
        {
            Handle = handle;
            UniformCache = new Dictionary<string, int>();

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var uniformCount);
            for (var i = 0; i < uniformCount; i += 1)
            {
                var key = GL.GetActiveUniformName(Handle, i);
                var loc = GL.GetUniformLocation(Handle, key);
                UniformCache[key] = loc;
            }
        }

        public static ShaderProgram TryLink(ICollection<ShaderSingle> shaders, out string errorMessage)
        {
            var handle = GL.CreateProgram();
            foreach (var shader in shaders)
                GL.AttachShader(handle, shader.Handle);
            GL.LinkProgram(handle);

            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out var status);
            if (status == 0)
            {
                GL.GetProgramInfoLog(handle, out errorMessage);
                GL.DeleteProgram(handle);
                return null;
            }

            errorMessage = null;
            return new ShaderProgram(handle);
        }

        public static ShaderProgram Link(ICollection<ShaderSingle> shaders)
        {
            var shader = TryLink(shaders, out string errorMessage);
            if (shader == null)
                throw new FormatException($"Couldn't link shader program\n\n{errorMessage}");
            return shader;
        }

        public void Use() => GL.UseProgram(Handle);
        public void SetInt(string name, int value) => GL.Uniform1(UniformCache[name], value);
        public void SetInt(string name, int[] value) => GL.Uniform1(UniformCache[name], value.Length, value);
        public void SetUint(string name, uint value) => GL.Uniform1(UniformCache[name], value);
        public void SetUint(string name, uint[] value) => GL.Uniform1(UniformCache[name], value.Length, value);
        public void SetFloat(string name, float value) => GL.Uniform1(UniformCache[name], value);
        public void SetFloat(string name, float[] value) => GL.Uniform1(UniformCache[name], value.Length, value);
        public void SetVector2(string name, Vector2 value) => GL.Uniform2(UniformCache[name], value);
        public void SetVector3(string name, Vector3 value) => GL.Uniform3(UniformCache[name], value);
        public void SetVector4(string name, Vector4 value) => GL.Uniform4(UniformCache[name], value);
        public void SetMatrix2(string name, Matrix2 value) => GL.UniformMatrix2(UniformCache[name], false, ref value);
        public void SetMatrix3(string name, Matrix3 value) => GL.UniformMatrix3(UniformCache[name], false, ref value);
        public void SetMatrix4(string name, Matrix4 value) => GL.UniformMatrix4(UniformCache[name], false, ref value);
        public void Dispose() => GL.DeleteProgram(Handle);
    }
}
