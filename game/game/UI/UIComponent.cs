using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public abstract class UIComponent
    {
        public Vector2f Position { get; set; }

        public UIComponent(Vector2f position)
        {
            Position = position;
        }

        public abstract void Update();
        public abstract void Draw();
    }

    
}
