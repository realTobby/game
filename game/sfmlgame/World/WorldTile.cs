using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
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

        public WorldTile Object;

        public TileType Type;

        public WorldTile(Sprite texture, Vector2f position, TileType type)
        {
            Type = type;

            if (texture == null) return;

            Sprite = new Sprite(texture)
            {
                Position = position
            };
        }

        public void Draw(RenderTexture target)
        {
            target.Draw(Sprite);

            if(Object != null)
            {
                Object.Draw(target);
            }
        }
    }
}
