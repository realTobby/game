using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Models
{
    using System;
    using game;
    using game.Managers;
    using SFML.Graphics;
    using SFML.System;

    public class AnimatedSprite
    {
        private Clock animationTimer;
        private Time frameDuration;
        private bool isFlipped;

        public int currentFrame;
        public Sprite[] sprites;
        public Color[] NormalColors;
        public Action OnAnimationFinish;
        public bool IsSingleShotAnimation = false;
        public bool IsFinished { get; private set; }

        public FloatRect HitBoxDimensions { get; set; }

        public AnimatedSprite(string category, string entityName, int frameCount)
        {
            var animationFrames = TextureLoader.Instance.GetAnimations($"Assets/Entities/{entityName}", entityName, frameCount);
            sprites = new Sprite[frameCount];
            NormalColors = new Color[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                sprites[i] = new Sprite(animationFrames[i]);
                NormalColors[i] = sprites[i].Color;
            }

            HitBoxDimensions = sprites[0].GetGlobalBounds();

            this.frameDuration = Time.FromSeconds(0.1f);
            animationTimer = new Clock();
            isFlipped = false; // Initialize the flag to false
        }

        public AnimatedSprite(Texture spriteSheet, int rows, int columns, Time frameDuration, Vector2f initialPos)
        {
            int frameWidth = (int)spriteSheet.Size.X / columns;
            int frameHeight = (int)spriteSheet.Size.Y / rows;

            int frameCount = rows * columns;
            sprites = new Sprite[frameCount];
            NormalColors = new Color[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                int row = i / columns;
                int column = i % columns;

                IntRect textureRect = new IntRect(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
                sprites[i] = new Sprite(spriteSheet, textureRect);
                NormalColors[i] = sprites[i].Color;
            }

            HitBoxDimensions = sprites[0].GetGlobalBounds();

            this.frameDuration = frameDuration;
            animationTimer = new Clock();
            isFlipped = false;

            SetPosition(initialPos);
        }

        public void SetScale(float scale)
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                sprites[i].Scale = new Vector2f(scale, scale);
            }
        }

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

        public void Draw(float deltaTime)
        {
            Game.Instance.GetRenderWindow().Draw(sprites[currentFrame]);
        }

        public void SetPosition(Vector2f position)
        {
            Vector2f textureSize = new Vector2f(sprites[0].TextureRect.Width, sprites[0].TextureRect.Height);

            foreach (Sprite sprite in sprites)
            {
                sprite.Position = position;
                sprite.Origin = textureSize / 2f;
            }
        }

        public void FlipSprite(bool flipped)
        {
            if (flipped && !isFlipped)
            {
                foreach (Sprite sprite in sprites)
                {
                    sprite.Scale = new Vector2f(-1, 1);
                }
                isFlipped = true;
            }
            else if (!flipped && isFlipped)
            {
                foreach (Sprite sprite in sprites)
                {
                    sprite.Scale = new Vector2f(1, 1);
                }
                isFlipped = false;
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
