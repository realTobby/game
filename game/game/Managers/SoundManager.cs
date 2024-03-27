using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class SoundManager
    {
        Random random = new Random();

        [AllowNull]
        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        private const string SFX_PATH = "Assets/SFX/";
        private const string SFX_EXPLOSION = "explosion.wav";
        private const string SFX_PICKUP = "pickup.wav";
        private const string SFX_LEVELUP = "powerUp.wav";
        private const string SFX_HIT = "hitHurt.wav";

        private Sound sound = new Sound();

        private Sound[] soundChannels = new Sound[32
            ];

        private string LastSound = string.Empty;
        private Sound LastChannel = null;

        public SoundManager()
        {
            if(_instance == null) _instance = this;

            for(int i = 0; i < soundChannels.Length; i++)
            {
                soundChannels[i] = new Sound();
                soundChannels[i].Stop();
            }

        }

        public int GetActiveChannels() => soundChannels.Length;

        private Sound FindFreeSoundChannel()
        {
            for(int i = 0; i < soundChannels.Length; i++)
            {
                if (soundChannels[i].Status == SoundStatus.Paused || soundChannels[i].Status == SoundStatus.Stopped)
                {
                    return soundChannels[i];
                }
            }

            // add a new sound channel
            Array.Resize(ref soundChannels, soundChannels.Length + 1);
            soundChannels[soundChannels.Length - 1] = new Sound();
            return soundChannels[soundChannels.Length - 1];
        }

        private Sound InitSound(string sfxToPlay, int volume)
        {
            if (LastSound.Equals(sfxToPlay)) return LastChannel;

           

            SoundBuffer buffer = new SoundBuffer(string.Format("{0}/{1}", SFX_PATH, sfxToPlay));

            LastChannel = FindFreeSoundChannel();
            LastSound = sfxToPlay;

            LastChannel.Volume = volume;
            LastChannel.SoundBuffer = buffer;

            return LastChannel;
        }

        public void PlayGemPickup()
        {
            InitSound(SFX_PICKUP, 25);
            LastChannel.Pitch = (float)random.NextDouble() * 0.2f + 0.9f;
            LastChannel.Play();
        }

        public void PlayLevelUp()
        {
            InitSound(SFX_LEVELUP, 25);
            LastChannel.Play();
        }

        public void PlayHit()
        {
            InitSound(SFX_HIT, 25);
            LastChannel.Pitch = (float)random.NextDouble() * 0.2f + 0.9f;
            LastChannel.Play();
        }

        public void PlayExplosion()
        {
            InitSound(SFX_EXPLOSION, 15);
            LastChannel.Pitch = (float)random.NextDouble() * 0.2f + 0.9f;
            LastChannel.Play();
        }

        internal void PlayLevelup()
        {
            InitSound(SFX_LEVELUP, 16);
            LastChannel.Pitch = (float)random.NextDouble() * 0.2f + 0.9f;
            LastChannel.Play();

        }
    }
}
