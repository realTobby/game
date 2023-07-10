using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Models
{
    using System;
    using game.Managers;
    using SFML.Graphics;
    using SFML.System;

    public class AnimatedSprite
    {
        private int currentFrame;
        private Sprite[] sprites;
        private Clock animationTimer;
        private Time frameDuration;

        public AnimatedSprite(string category, string entityName, int frameCount)
        {
            var animationFrames = TextureLoader.Instance.GetAnimations($"Assets/Entities/{entityName}", entityName, frameCount);
            sprites = new Sprite[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                //IntRect textureRect = new IntRect(i * frameWidth, 0, frameWidth, frameHeight);
                sprites[i] = new Sprite(animationFrames[i]);
            }

            this.frameDuration = Time.FromSeconds(0.1f);
            animationTimer = new Clock();
        }

        public void Update()
        {
            if (animationTimer.ElapsedTime > frameDuration)
            {
                currentFrame = (currentFrame + 1) % sprites.Length;
                //Console.WriteLine("AnimatedSprite: " + currentFrame);
                animationTimer.Restart();
            }
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(sprites[currentFrame]);
        }

        public void SetPosition(Vector2f position)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Position = position;
            }
        }
    }
}
