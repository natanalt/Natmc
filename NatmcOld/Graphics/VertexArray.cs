using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Natmc.Graphics
{
    public class VertexArray<VertexType> : IDisposable where VertexType : struct
    {
        public static readonly int VertexSize = Marshal.SizeOf<VertexType>();

        public int Handle { get; protected set; }
        public int VboHandle { get; protected set; }
        public int EboHandle { get; protected set; }
        public BufferUsageHint UsageHint { get; protected set; }
        public int VerticesCount { get; protected set; }
        public int IndicesCount { get; protected set; }

        private int ActualIndexCount;

        public VertexArray(
            BufferUsageHint usageHint,
            VertexType[] vertices,
            uint[] indices)
        {
            Handle = GL.GenVertexArray();
            UsageHint = usageHint;
            VerticesCount = vertices.Length;
            IndicesCount = indices.Length;

            Bind();
            VboHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexSize, vertices, usageHint);
            EboHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, usageHint);
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EboHandle);
            GL.BindVertexArray(Handle);
        }

        public void EnableAttributeArray(
            int location,
            string field,
            int componentAmount,
            VertexAttribPointerType type = VertexAttribPointerType.Float,
            bool normalize = false)
        {
            Bind();
            var fieldOffset = Marshal.OffsetOf<VertexType>(field);
            GL.VertexAttribPointer(
                location,
                componentAmount,
                type,
                normalize,
                VertexSize,
                fieldOffset);
            GL.EnableVertexAttribArray(location);
        }

        public void EnableAttributeArray(int location, string fieldName, bool normalize = false)
        {
            var typeMap = new Dictionary<Type, Tuple<int, VertexAttribPointerType>>
            {
                [typeof(Vector2)] = new Tuple<int, VertexAttribPointerType>(2, VertexAttribPointerType.Float),
                [typeof(Vector3)] = new Tuple<int, VertexAttribPointerType>(3, VertexAttribPointerType.Float),
                [typeof(Vector4)] = new Tuple<int, VertexAttribPointerType>(4, VertexAttribPointerType.Float),
                [typeof(Color4)] = new Tuple<int, VertexAttribPointerType>(4, VertexAttribPointerType.Float),
                [typeof(Vector2i)] = new Tuple<int, VertexAttribPointerType>(2, VertexAttribPointerType.Int),
                [typeof(Vector3i)] = new Tuple<int, VertexAttribPointerType>(3, VertexAttribPointerType.Int),
                [typeof(Vector4i)] = new Tuple<int, VertexAttribPointerType>(4, VertexAttribPointerType.Int),
                [typeof(int)] = new Tuple<int, VertexAttribPointerType>(1, VertexAttribPointerType.Int),
                [typeof(uint)] = new Tuple<int, VertexAttribPointerType>(1, VertexAttribPointerType.UnsignedInt),
                [typeof(float)] = new Tuple<int, VertexAttribPointerType>(1, VertexAttribPointerType.Float),
            };

            var field = typeof(VertexType).GetField(fieldName);
            if (!typeMap.ContainsKey(field.FieldType))
                throw new InvalidOperationException($"Can't convert {field.FieldType} {fieldName}");

            EnableAttributeArray(
                location,
                fieldName,
                typeMap[field.FieldType].Item1,
                typeMap[field.FieldType].Item2,
                normalize);
        }

        public void DisableAttributeArray(int location)
        {
            Bind();
            GL.DisableVertexAttribArray(location);
        }

        public void UpdateVertices(VertexType[] vertices, int offset = 0)
        {
            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, vertices.Length * VertexSize, vertices);
        }

        public void UpdateIndices(uint[] indices, int offset = 0)
        {
            Bind();
            ActualIndexCount = indices.Length;
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)offset, indices.Length * sizeof(uint), indices);
        }

        public void Draw(ShaderProgram shader, BeginMode mode, int indices = -1)
        {
            if (indices == -1)
                indices = ActualIndexCount;

            Bind();
            shader.Use();
            GL.DrawElements(mode, indices, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            if (Handle == -1)
                throw new InvalidOperationException();
            GL.DeleteVertexArray(Handle);
            Handle = -1;
        }
    }
}
