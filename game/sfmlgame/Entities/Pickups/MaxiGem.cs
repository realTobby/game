
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Managers;


namespace sfmlgame.Entities.Pickups
{
    public class MaxiGem : Gem
    {
        public MaxiGem(Vector2f initialPosition, int xp) : base(initialPosition, GameAssets.Instance.TextureLoader.GetTexture("gem_green", "Entities/Pickups"))
        {
            XPGAIN = xp;

            SetScale(.7f);

            SetPosition(initialPosition);

        }

        public override int Pickup()
        {
            SoundManager.Instance.PlayGemPickup();

            IsActive = false;
            //EntityManager.Instance.RemoveEntity(this);

            return XPGAIN;
        }

        public override void Update(Player player, float deltaTime)
        {
            base.SetHitBoxDimensions(new FloatRect(GetPosition().X, GetPosition().Y, 16, 16));

            base.Update(player, deltaTime);
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
        }
    }
}
