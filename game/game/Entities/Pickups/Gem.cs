using game.Managers;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Pickups
{
    public class Gem : Entity
    {
        public int XPGAIN = 1;

        public Gem(Vector2f initialPosition) : base(TextureLoader.Instance.GetTexture("gem_blue", "Entities/Pickups"), 1, 4, Time.FromSeconds(0.2f), initialPosition)
        {
            SetScale(.7f);

            SetPosition(initialPosition);

        }

        public Gem(Vector2f initialPosition, Texture gem) : base(gem, 1, 4, Time.FromSeconds(0.2f), initialPosition)
        {
            SetScale(.8f);

            SetPosition(initialPosition);
        }


        public virtual int Pickup()
        {
            SoundManager.Instance.PlayGemPickup();
            EntityManager.Instance.RemoveEntity(this);
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
