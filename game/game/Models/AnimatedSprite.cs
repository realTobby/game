using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Models
{
    using System;
    using game.Controllers;
    using game.Managers;
    using SFML.Graphics;
    using SFML.System;

    public class AnimatedSprite
    {
        public int currentFrame;
        public Sprite[] sprites;
        private Clock animationTimer;
        private Time frameDuration;

        public Action OnAnimationFinish;

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

        public AnimatedSprite(Texture spriteSheet, int rows, int columns, Time frameDuration)
        {
            int frameWidth = (int)spriteSheet.Size.X / columns;
            int frameHeight = (int)spriteSheet.Size.Y / rows;

            int frameCount = rows * columns;
            sprites = new Sprite[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                int row = i / columns;
                int column = i % columns;

                IntRect textureRect = new IntRect(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
                sprites[i] = new Sprite(spriteSheet, textureRect);
            }

            this.frameDuration = frameDuration;
            animationTimer = new Clock();
        }

        public bool IsSingleShotAnimation = false;

        // Property to indicate whether the animation has finished
        public bool IsFinished { get; private set; }

        public virtual void Update()
        {
            if (animationTimer.ElapsedTime > frameDuration)
            {
                currentFrame = (currentFrame + 1) % sprites.Length;
                animationTimer.Restart();

                // If we've looped back to the beginning, the animation has finished
                if (currentFrame == 0 && IsSingleShotAnimation == true)
                {
                    IsFinished = true;
                    OnAnimationFinish?.Invoke();
                }
            }
        }

        public void Draw()
        {
            Game.Instance.GetRenderWindow().Draw(sprites[currentFrame]);
        }

        public void SetPosition(Vector2f position)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Position = position;
            }
        }

        public void DebugDrawAllFrames()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                // Copy the sprite so we can modify its position without affecting the original
                Sprite spriteCopy = new Sprite(sprites[i]);

                // Position each frame next to the previous one
                float spacing = 10f; // Adjust as needed for your sprites
                spriteCopy.Position = new Vector2f(i * (sprites[i].GetGlobalBounds().Width + spacing), 0);

                // Draw a rectangle around the sprite's bounds
                RectangleShape boundsRect = new RectangleShape(new Vector2f(spriteCopy.GetGlobalBounds().Width, spriteCopy.GetGlobalBounds().Height))
                {
                    Position = spriteCopy.Position,
                    OutlineColor = Color.Magenta,
                    FillColor = Color.Transparent,
                    OutlineThickness = 1f
                };
                Game.Instance.GetRenderWindow().Draw(boundsRect);

                // Draw the sprite
                Game.Instance.GetRenderWindow().Draw(spriteCopy);
            }
        }

    }
}
