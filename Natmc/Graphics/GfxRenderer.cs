using Natmc.Core;
using Natmc.Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public class GfxRenderer
    {
        private static readonly LogScope Log = new LogScope("Renderer");

        private static bool ReportedInfo = false;
        public static void ReportInfo()
        {
            Log.Info($"Renderer info:");
            Log.Info($" {GL.GetString(StringName.Vendor)}");
            Log.Info($" {GL.GetString(StringName.Renderer)}");
            Log.Info($" Max texture units: {GL.GetInteger(GetPName.MaxTextureImageUnits)}");
            ReportedInfo = true;
        }

        public StatedWindow Parent { get; }
        public List<Texture> Textures { get; protected set; }

        public int TotalDrawCalls { get; protected set; }
        public int DrawCallsPerFrame { get; set; }

        private UiRenderer UiRenderer;

        public GfxRenderer(StatedWindow parent)
        {
            Parent = parent;
        }

        public void Init()
        {
            Textures = new List<Texture>();
            UiRenderer = new UiRenderer(Parent);

            if (!ReportedInfo)
                ReportInfo();
        }

        public void Deinit()
        {
            UiRenderer.Dispose();
            foreach (var texture in Textures)
            {
                if (texture.Valid)
                    texture.Dispose();
            }
        }

        public Texture CreateTexture(byte[] rawData, int width, int height)
        {
            var texture = new Texture(width, height, rawData);
            Textures.Add(texture);
            return texture;
        }

        public void BeginFrame(float delta)
        {
            DrawCallsPerFrame = 0;
        }

        public void EndFrame()
        {
            TotalDrawCalls += DrawCallsPerFrame;
        }

        public void BeginUi()
        {
            UiRenderer.Begin();
        }

        public void DrawColoredQuad(Vector2 position, Vector2 size, Color4 color)
        {
            UiRenderer.DrawColoredQuad(position, size, color);
        }

        public void DrawTexturedQuad(
            Vector2 position,
            Vector2 size,
            Color4 multiplier,
            Texture texture,
            Vector2? textureOriginOpt = null,
            Vector2? textureSizeOpt = null)
        {
            var textureOrigin = textureOriginOpt ?? Vector2.Zero;
            var textureSize = textureSizeOpt ?? new Vector2(texture.Width, texture.Height);
            UiRenderer.DrawTexturedQuad(position, size, multiplier, texture, textureOrigin, textureSize);
        }

        public void EndUi()
        {
            UiRenderer.End();
        }

        public void BeginWorld()
        {
            throw new NotImplementedException();
        }

        public void DrawSky()
        {
            throw new NotImplementedException();
        }

        public void DrawChunk(/*...*/)
        {
            throw new NotImplementedException();
        }

        public void DrawEntity(/*...*/)
        {
            throw new NotImplementedException();
        }

        public void EndWorld()
        {
            throw new NotImplementedException();
        }

        public void ReportDrawCall()
        {
            DrawCallsPerFrame += 1;
        }
    }
}
