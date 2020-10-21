using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Base
{
    public enum ShaderBuildPhase
    {
        VertexCompile, FragmentCompile, Link
    }

    public class ShaderProgram
    {
        public bool Valid => Handle != -1;
        public int Handle { get; protected set; }
        protected Dictionary<string, int> UniformCache;

        public ShaderProgram(int handle)
        {
            Handle = handle;
            UniformCache = new Dictionary<string, int>();
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int totalUniforms);
            for (int i = 0; i < totalUniforms; i += 1)
            {
                string name = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, name);
                UniformCache[name] = location;
            }
        }

        private static int CompileSingle(
            ShaderType type,
            string source,
            out string error)
        {
            int handle = GL.CreateShader(type);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);

            GL.GetShader(handle, ShaderParameter.CompileStatus, out int status);
            if (status == 0)
            {
                error = GL.GetShaderInfoLog(handle);
                GL.DeleteShader(handle);
                return -1;
            }

            error = null;
            return handle;
        }

        public static ShaderProgram TryCompile(
            string vertexSource,
            string fragmentSource,
            out ShaderBuildPhase? errorPhase,
            out string error)
        {
            int vertex = CompileSingle(ShaderType.VertexShader, vertexSource, out error);
            if (vertex == -1)
            {
                errorPhase = ShaderBuildPhase.VertexCompile;
                return null;
            }

            int fragment = CompileSingle(ShaderType.FragmentShader, fragmentSource, out error);
            if (vertex == -1)
            {
                errorPhase = ShaderBuildPhase.FragmentCompile;
                return null;
            }

            int handle = GL.CreateProgram();
            GL.AttachShader(handle, vertex);
            GL.AttachShader(handle, fragment);
            GL.LinkProgram(handle);

            GL.DetachShader(handle, vertex);
            GL.DetachShader(handle, fragment);
            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);

            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int linkStatus);
            if (linkStatus == 0)
            {
                errorPhase = ShaderBuildPhase.Link;
                error = GL.GetProgramInfoLog(handle);
                GL.DeleteProgram(handle);
                return null;
            }

            errorPhase = null;
            error = null;
            return new ShaderProgram(handle);
        }

        public static ShaderProgram Compile(string vertexSource, string fragmentSource)
        {
            var shader = TryCompile(vertexSource, fragmentSource, out var phase, out var error);
            if (phase == null)
                Native.ErrorBoxAndExit($"Shader build error: {phase.Value}", error);
            return shader;
        }

        public void Use()
        {
            VerifyValidity();
            GL.UseProgram(Handle);
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(UniformCache[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(UniformCache[name], data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(UniformCache[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(UniformCache[name], data);
        }

        public void Dispose()
        {
            VerifyValidity();
            GL.DeleteProgram(Handle);
            Handle = -1;
        }

        private void VerifyValidity()
        {
            if (!Valid)
                throw new InvalidOperationException();
        }
    }
}
