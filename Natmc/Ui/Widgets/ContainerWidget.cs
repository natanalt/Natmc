using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Widgets
{
    public class ContainerWidget : Widget
    {
        public List<Widget> Children { get; protected set; }

        public ContainerWidget(UiManager owner) : base(owner)
        {
            Children = new List<Widget>();
        }

        public override void Draw()
        {
            foreach (var widget in Children)
                widget.Draw();
        }

        public void Remove(Widget widget)
        {
            widget.OnRemove();
            Children.Remove(widget);
        }
        public T Add<T>(T widget) where T : Widget
        {
            Children.Add(widget);
            return widget;
        }
        public T Add<T>() where T : Widget => Add(Activator.CreateInstance<T>());
    }
}
