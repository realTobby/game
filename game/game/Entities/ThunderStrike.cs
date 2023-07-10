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

namespace game.Entities
{
    public class ThunderStrike : AnimatedSprite
    {
        public bool IsAudioPlaying = false;

        public int HitFrame = 5;

        Sound sound = new Sound();

        public ThunderStrike() : base(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.095f))
        {
            SoundBuffer buffer = new SoundBuffer("Assets/SFX/explosion.wav");
            sound.Volume = 25;
            sound.SoundBuffer = buffer;
        }

        public override void Update()
        {
            base.Update();

            if (base.currentFrame == HitFrame)
            {
                PlaySFX();
            }
            else if (base.currentFrame == 0 && IsAudioPlaying) // Animation has been reset
            {
                IsAudioPlaying = false;
            }
        }

        private void PlaySFX()
        {
            if (!IsAudioPlaying)
            {
                IsAudioPlaying = true;
                sound.Play();
            }
        }
    }

}
