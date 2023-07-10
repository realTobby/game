using game.Controllers;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public class UI_Text : UIComponent
    {
        SFML.Graphics.Text textComp;

        public UI_Text(string text, int size, Vector2f pos) : base(pos)
        {
            textComp = new SFML.Graphics.Text(text, new SFML.Graphics.Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            textComp.Position = new SFML.System.Vector2f(pos.X, pos.Y);
        }

        public override void Draw()
        {
            Game.Instance.GetRenderWindow().Draw(textComp);
        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }

    }
}
