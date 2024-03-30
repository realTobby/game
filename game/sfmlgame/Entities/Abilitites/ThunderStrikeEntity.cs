
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Managers;


namespace game.Entities.Abilitites
{
    public class ThunderStrikeEntity : AbilityEntity
    {
        public bool IsAudioPlaying = false;

        public int HitFrame = 5;

        public ThunderStrikeEntity(Vector2f initialPosition) : base("ThunderStrike", initialPosition, GameAssets.Instance.TextureLoader.GetTexture("thunderStrike", "Entities/Abilities"), 1, 13, Time.FromSeconds(0.08f))
        {
            Damage = 1;
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
                //EntityManager.Instance.RemoveEntity(this);
            }

        }

        private void PlaySFX()
        {
            SoundManager.Instance.PlayExplosion();
            

        }

        

    }

}
