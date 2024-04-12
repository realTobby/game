

using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Managers;


namespace sfmlgame.Entities.Abilitites
{
    public class ThunderStrikeEntity : AbilityEntity
    {
        public bool IsAudioPlaying = false;

        public int HitFrame = 5;

        public ThunderStrikeEntity(Vector2f initialPosition) : base("ThunderStrike", initialPosition, GameAssets.Instance.TextureLoader.GetTexture("thunderStrike", "Entities/Abilities"), 1, 13, Time.FromSeconds(0.08f))
        {
            Damage = 1;
            CanCheckCollision = false;
        }

        public override void ResetFromPool(Vector2f position)
        {
            base.ResetFromPool(position);

            CanCheckCollision = false;

        }


        public override void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            base.Update(player, deltaTime);

            //base.SrtHitBoxDimensions(new FloatRect(GetPosition().X, GetPosition().Y, HitBoxDimensions.Width, HitBoxDimensions.Height));


            if (base.animateSpriteComponent.currentFrame == HitFrame)
            {
                CanCheckCollision = true;
                PlaySFX();
            }

            if (base.animateSpriteComponent.currentFrame >= base.animateSpriteComponent.sprites.Length - 1)
            {
                IsActive = false;
                CanCheckCollision = false;
                //EntityManager.Instance.RemoveEntity(this);
            }

        }

        private void PlaySFX()
        {
            SoundManager.Instance.PlayExplosion();
            

        }

        

    }

}
