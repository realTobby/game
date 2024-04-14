using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.UI
{
    public class UI_Group : UIComponent
    {
        public UI_Button baseButton;

        public bool IsOpen = false;

        public List<UIComponent> children = new List<UIComponent>();

        public UI_Group(Vector2f position, string name) : base(position)
        {
            baseButton = new UI_Button(new Vector2f(position.X, position.Y), name, 40, 280, 64, Color.Magenta);
            baseButton.ClickAction = () =>
            {
                // open list of ui_components
                IsOpen = !IsOpen;
            };
        }

        public void AddChild(UIComponent comp)
        {
            children.Add(comp);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            baseButton.Draw(renderTexture);

            if (!IsOpen) return;
            foreach (var child in children)
            {
                child.Draw(renderTexture);
            }
        }

        public override void Update(float deltaTime)
        {
            baseButton.Update(deltaTime);

            if (!IsOpen) return;
            foreach (var child in children)
            {
                child.Update(deltaTime);
            }
        }
    }
}
