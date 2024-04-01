
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Managers;


namespace sfmlgame.Entities.Pickups
{
    public class Gem : Entity
    {
        public int XPGAIN = 1;


        public Gem(Vector2f initialPosition) : base(GameAssets.Instance.TextureLoader.GetTexture("gem_blue", "Entities/Pickups"), 1, 4, Time.FromSeconds(0.2f), initialPosition)
        {
            base.animateSpriteComponent.SetScale(.7f);
            
            

            CanCheckCollision = true;

            SetPosition(initialPosition);
        }

        public Gem(Vector2f initialPosition, Texture gem) : base(gem, 1, 4, Time.FromSeconds(0.2f), initialPosition)
        {
            
            base.SetScale(.8f);
            
            

            CanCheckCollision = true;

            SetPosition(initialPosition);
        }


        public virtual int Pickup()
        {
            SoundManager.Instance.PlayGemPickup();
            IsActive = false;
            //EntityManager.Instance.RemoveEntity(this);
            return XPGAIN;
        }


        public override void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            base.SetHitBoxDimensions(new FloatRect(GetPosition().X, GetPosition().Y, 16, 16));

            base.Update(player, deltaTime);

            
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

            CanCheckCollision = true;
        }
    }
}
