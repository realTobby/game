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

        }

        public override void Update()
        {
            base.Update();

            HitBoxDimensions = new FloatRect(Position.X, Position.Y, HitBoxDimensions.Width, HitBoxDimensions.Height);


            if (base.currentFrame == HitFrame)
            {
                CanCheckCollision = true;
                PlaySFX();
            }

            if (base.currentFrame >= base.sprites.Length - 1)
            {
                GameManager.Instance.RemoveEntity(this);
            }

        }

        private void PlaySFX()
        {
            SoundManager.Instance.PlayExplosion();
            

        }
    }

}
