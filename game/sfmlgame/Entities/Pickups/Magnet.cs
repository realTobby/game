using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Managers;


namespace sfmlgame.Entities.Pickups
{
    public class Magnet : Pickup
    {
        public int XPGAIN = 1;


        public Magnet(Vector2f initialPosition) : base(new Sprite(GameAssets.Instance.TextureLoader.GetTexture("magnet", "Entities/Pickups")), initialPosition)
        {
            base.animateSpriteComponent.SetScale(.7f);

            CanCheckCollision = true;

            SetPosition(initialPosition);

        }

        public override void PickItUp()
        {
            //UniversalLog.LogInfo("Magnet was picked up!");
            // magnetize all gems
            Game.Instance.EntityManager.MagnetizeAllGems();

            SoundManager.Instance.PlayGemPickup();
            IsActive = false;
        }

        public override int PickItUpInt()
        {
            PickItUp();
            return 0;
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
