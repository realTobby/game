using game.Managers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Pickups
{
    public class MaxiGem : Gem
    {
        public MaxiGem(Vector2f initialPosition, int xp) : base(initialPosition, TextureLoader.Instance.GetTexture("gem_green", "Entities/Pickups"))
        {
            XPGAIN = xp;

            SetScale(.7f);

            SetPosition(initialPosition);

        }

        public override int Pickup()
        {
            SoundManager.Instance.PlayGemPickup();

            GameManager.Instance.RemoveEntity(this);

            return XPGAIN;
        }

        public override void Update()
        {
            HitBoxDimensions = new FloatRect(Position.X, Position.Y, 16, 16);

            base.Update();
        }

        public override void Draw(float deltaTime)
        {
            base.Draw(deltaTime);
        }
    }
}
