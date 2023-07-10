using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Models
{
    public class OverworldTile
    {
        public Vector2f Position
        {
            get { return Sprite.Position; }
            set { Sprite.Position = value; }
        }

        public Sprite Sprite { get; set; }

        public OverworldTile(Texture texture, Vector2f pos)
        {
            Sprite = new Sprite(texture);
            Position = pos;
        }

        public Sprite Object { get; set; } = null;


    }
}
