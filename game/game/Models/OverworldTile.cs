using game.Helpers;
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
            set {
                UniversalLog.LogInfo("Pos changed lol! : " + value.ToString());
                Sprite.Position = value; }
        }

        public Sprite Sprite { get; set; }

        public OverworldTile(Sprite sprite, Vector2f pos)
        {
            Sprite = sprite;
            Position = pos;
        }

        public Sprite Object { get; set; } = null;


    }
}
