using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.UI
{
    public class UI_DebugConsole : UIComponent
    {
        RectangleShape backgroundShape;



        public UI_DebugConsole(Vector2f position) : base(position)
        {
            Width = 1000;
            Height = 600;

            backgroundShape = new RectangleShape(new Vector2f(Width, Height));
            backgroundShape.FillColor = Color.Black;
            backgroundShape.Position = position;

            

        }

        public override void Draw(RenderTexture renderTexture)
        {
            throw new NotImplementedException();
        }

        public override void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
