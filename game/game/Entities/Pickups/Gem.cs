using game.Managers;
using game.Scenes;
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
            base.animateSpriteComponent.SetScale(.7f);

            SetPosition(initialPosition);

        }

        public Gem(Vector2f initialPosition, Texture gem) : base(gem, 1, 4, Time.FromSeconds(0.2f), initialPosition)
        {
            
            base.SetScale(.8f);

            SetPosition(initialPosition);
        }


        public virtual int Pickup()
        {
            SoundManager.Instance.PlayGemPickup();
            IsActive = false;
            //EntityManager.Instance.RemoveEntity(this);
            return XPGAIN;
        }


        public override void Update()
        {
            if (!IsActive) return;

            base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, 16, 16));

            base.Update();

            
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;
            base.Draw(renderTexture, deltaTime);
        }

        public override void ResetFromPool(Vector2f position)
        {
            base.IsActive = true;
            IsMagnetized = false;
            SetPosition(position);
        }
    }
}
