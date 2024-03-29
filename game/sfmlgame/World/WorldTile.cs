using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.World
{
    public class WorldTile
    {
        public Sprite Sprite;

        public WorldTile(Sprite texture, Vector2f position)
        {
            if (texture == null) return;

            Sprite = new Sprite(texture)
            {
                Position = position
            };
        }

        public void Draw(RenderTexture target)
        {
            target.Draw(Sprite);
        }
    }
}
