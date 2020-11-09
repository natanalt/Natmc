using Natmc.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui
{
    public abstract class Widget
    {
        public UiManager Owner { get; }
        public GfxRenderer Renderer => Owner.Parent.Renderer;

        public Widget Parent;
        public Vector2 Position;
        public Vector2 Size;
        public bool Visible;

        public Vector2 GlobalPosition => Parent == null ? Position : Parent.GlobalPosition + Position;

        protected Widget(UiManager owner)
        {
            Owner = owner;
            Visible = true;
        }

        public virtual void Draw() { }
        public virtual void OnAdd() { }
        public virtual void OnRemove() { }
    }
}
