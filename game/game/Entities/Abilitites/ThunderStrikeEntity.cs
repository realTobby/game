using game.Entities.Enemies;
using game.Managers;
using game.Models;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Abilitites
{
    public class ThunderStrikeEntity : AbilityEntity
    {
        public bool IsAudioPlaying = false;

        public int HitFrame = 5;

        public ThunderStrikeEntity(Vector2f initialPosition) : base("ThunderStrike", initialPosition, TextureLoader.Instance.GetTexture("thunderStrike", "Entities/Abilities"), 1, 13, Time.FromSeconds(0.08f))
        {
            Damage = 5;
        }

        public override void Update()
        {
            if (!IsActive) return;

            base.Update();

            base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, HitBoxDimensions.Width, HitBoxDimensions.Height));


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
