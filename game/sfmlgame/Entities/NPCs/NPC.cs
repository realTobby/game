using SFML.Graphics;
using SFML.System;
using sfmlgame.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Entities.NPCs
{
    public class NPC : Entity
    {
        public NPC(Vector2f initialPosition) : base("Entities/priestess", "priestess", 5, initialPosition)
        {
            base.IsStatic = true;

        }

        public override void ResetFromPool(Vector2f position)
        {
            SetPosition(position);
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);
        }
    }
}
