using Natmc.Core;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Ogl3
{
    public class UiRenderer : IDisposable
    {
        public const int DefaultMaxQuads = 10000;
        public const int VerticesPerQuad = 4;
        public const int IndicesPerQuad = 6;

        public StatedWindow Window { get; }
        public Ogl3RenderingApi RenderingApi { get; }
        public ShaderProgram UiShader { get; }

        public int MaxQuads { get; protected set; }
        public int MaxVertices { get; protected set; }
        public int MaxIndices { get; protected set; }
        public int MaxTextures { get; protected set; }

        private List<UiVertex> PendingVertices;
        private List<uint> PendingIndices;
        private List<Ogl3Texture> PendingTextures;
        public int RemainingVertices => MaxVertices - PendingVertices.Count;
        public int RemainingIndices => MaxIndices - PendingIndices.Count;
        public int RemainingTextures => MaxTextures - PendingTextures.Count;
        public bool HasRemainingQuads => RemainingVertices >= VerticesPerQuad && RemainingIndices >= IndicesPerQuad;

        private readonly VertexArray<UiVertex> Vao;

        public UiRenderer(StatedWindow window, int maxQuadCount = DefaultMaxQuads)
        {
            Window = window;
            RenderingApi = (Ogl3RenderingApi)Window.RenderingApi;
            UiShader = ShaderProgram.Link(new ShaderSingle[]
            {
                ShaderSingle.Compile(@"
                                
                    #version 330 core
                
                    layout (location = 0) in vec2 a_Position;
                    layout (location = 1) in vec4 a_Color;
                    layout (location = 2) in vec2 a_TextureCoords;
                    layout (location = 3) in float a_SamplerIndex;
                
                    uniform mat4 u_Projection;
                
                    out vec4 f_Color;
                    out vec2 f_TextureCoords;
                    flat out int f_SamplerIndex;
                
                    void main()
                    {
                        f_Color = a_Color;
                        f_TextureCoords = a_TextureCoords;
                        f_SamplerIndex = int(a_SamplerIndex);
                        gl_Position = u_Projection * vec4(a_Position, -1.0f, 1.0f);
                        
                        // Shift the vertex coordinates, so that (0,0) points to bottom left corner
                        gl_Position.x -= 1;
                        gl_Position.y -= 1;
                    }
                
                ", ShaderType.VertexShader),

                ShaderSingle.Compile(@"
                
                    #version 330 core
                
                    in vec4 f_Color;
                    in vec2 f_TextureCoords;
                    flat in int f_SamplerIndex;
                
                    uniform sampler2D u_Textures[${MaxTextureUnits}];
                
                    out vec4 o_Color;
                
                    void main()
                    {
                        if (f_SamplerIndex == 0)
                            o_Color = f_Color;
                        else
                            o_Color = texture(u_Textures[f_SamplerIndex - 1], f_TextureCoords) * f_Color;
                    }
                
                ".Replace("${MaxTextureUnits}", GL.GetInteger(GetPName.MaxTextureImageUnits).ToString()), ShaderType.FragmentShader),
            });

            MaxQuads = maxQuadCount;
            MaxVertices = MaxQuads * VerticesPerQuad;
            MaxIndices = MaxQuads * IndicesPerQuad;
            MaxTextures = GL.GetInteger(GetPName.MaxTextureImageUnits);

            PendingVertices = new List<UiVertex>();
            PendingIndices = new List<uint>();
            PendingTextures = new List<Ogl3Texture>();

            Vao = new VertexArray<UiVertex>(
                BufferUsageHint.DynamicDraw,
                new UiVertex[MaxVertices],
                new uint[MaxIndices]);
            Vao.EnableAttributeArray(0, "Position");
            Vao.EnableAttributeArray(1, "Color");
            Vao.EnableAttributeArray(2, "TextureCoords");
            Vao.EnableAttributeArray(3, "SamplerIndex");
        }

        public void Dispose()
        {
            UiShader.Dispose();
            Vao.Dispose();
        }

        public void Begin()
        {
            ClearPendingBuffers();
        }

        public void End()
        {
            DrawPending();
        }

        public void DrawColoredQuad(
            Vector2 position,
            Vector2 size,
            Color4 color)
        {
            if (!HasRemainingQuads)
                DrawPending();

            var vertexBase = (uint)PendingVertices.Count;
            PendingVertices.Add(new UiVertex(position, color));
            PendingVertices.Add(new UiVertex(position + new Vector2(size.X, 0), color));
            PendingVertices.Add(new UiVertex(position + new Vector2(0, size.Y), color));
            PendingVertices.Add(new UiVertex(position + new Vector2(size.X, size.Y), color));

            PendingIndices.AddRange(new uint[]
            {
                vertexBase + 0,
                vertexBase + 1,
                vertexBase + 2,
                vertexBase + 1,
                vertexBase + 2,
                vertexBase + 3,
            });
        }

        public void DrawTexturedQuad(
            Vector2 position,
            Vector2 size,
            Color4 color,
            ITexture texture,
            Vector2 textureOrigin,
            Vector2 textureSize)
        {
            if (!HasRemainingQuads)
                DrawPending();

            if (!(texture is Ogl3Texture))
                throw new InvalidOperationException("Invalid texture type");
            var samplerIndex = AddTexture((Ogl3Texture)texture) + 1;

            textureOrigin = texture.PixelToNormalized(new Vector2i((int)textureOrigin.X, (int)textureOrigin.Y));
            textureSize = texture.PixelToNormalized(new Vector2i((int)textureSize.X, (int)textureSize.Y));

            var vertexBase = (uint)PendingVertices.Count;

            PendingVertices.Add(new UiVertex(
                position,
                color,
                textureOrigin,
                samplerIndex));

            PendingVertices.Add(new UiVertex(
                position + new Vector2(size.X, 0),
                color,
                textureOrigin + new Vector2(textureSize.X, 0),
                samplerIndex));
            
            PendingVertices.Add(new UiVertex(
                position + new Vector2(0, size.Y),
                color,
                textureOrigin + new Vector2(0, textureSize.Y),
                samplerIndex));
            
            PendingVertices.Add(new UiVertex(
                position + new Vector2(size.X, size.Y),
                color,
                textureOrigin + new Vector2(textureSize.X, textureSize.Y),
                samplerIndex));

            PendingIndices.AddRange(new uint[]
            {
                vertexBase + 0,
                vertexBase + 1,
                vertexBase + 2,
                vertexBase + 1,
                vertexBase + 2,
                vertexBase + 3,
            });
        }

        private void DrawPending()
        {
            if (PendingIndices.Count == 0)
                return;

            UiShader.Use();
            UiShader.SetMatrix4("u_Projection", Matrix4.CreateOrthographic(Window.Size.X, Window.Size.Y, 0.0001f, 1000.0f));

            var samplerData = PrepareSamplerIndices();
            //UiShader.SetInt("u_Textures[0]", samplerData);

            GL.Uniform1(GL.GetUniformLocation(UiShader.Handle, "u_Textures"), samplerData.Length, samplerData);
            Console.WriteLine(GL.GetError());

            Vao.Bind();
            Vao.UpdateIndices(PendingIndices.ToArray());
            Vao.UpdateVertices(PendingVertices.ToArray());

            Vao.Draw(UiShader, BeginMode.Triangles);
            RenderingApi.ReportDrawCall();

            ClearPendingBuffers();
        }

        private int AddTexture(Ogl3Texture texture)
        {
            var existingOne = PendingTextures.FindIndex(x => x.GlHandle == texture.GlHandle);
            if (existingOne != -1)
                return existingOne;

            if (RemainingTextures == 0)
                DrawPending();
            PendingTextures.Add(texture);
            return PendingTextures.Count - 1;
        }

        private int[] PrepareSamplerIndices()
        {
            var result = new int[PendingTextures.Count];
            for (int i = 0; i < PendingTextures.Count; i += 1)
            {
                result[i] = i;
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, PendingTextures[i].GlHandle);
                
            }
            return result;
        }

        private void ClearPendingBuffers()
        {
            PendingVertices.Clear();
            PendingIndices.Clear();
            PendingTextures.Clear();
        }
    }
}
