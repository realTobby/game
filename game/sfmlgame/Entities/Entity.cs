
using SFML.Graphics;
using SFML.System;
using sfmlgame.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Entities
{
    public abstract class Entity
    {
        internal AnimatedSprite animateSpriteComponent;

        public bool IsActive = false;

        public abstract void ResetFromPool(Vector2f position);

        public Vector2f Position { get; set; }

        public int Damage = 0;

        public bool IsMagnetized = false;

        private void MoveTowardsPlayer(Player player, float deltaTime)
        {
            Vector2f direction = player.Sprite.Position - Position;
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            direction = direction / magnitude; // Normalize the direction vector
            Position += direction * 300f * deltaTime;
            SetPosition(Position);
        }

        public void SetPosition(Vector2f pos)
        {
            animateSpriteComponent.SetPosition(pos);
            Position = pos;
        }

        public void SetScale(float scale)
        {
            animateSpriteComponent.SetScale(scale);
        }

        public Entity(string category, string entityName, int frameCount, Vector2f initialPosition)
        {
            Position = initialPosition;

            animateSpriteComponent = new AnimatedSprite(category, entityName, frameCount);

            IsActive = true;
        }

        public Entity(Texture texture, int rows, int columns, Time frameDuration, Vector2f initialPosition)
        {
            Position = initialPosition;

            animateSpriteComponent = new AnimatedSprite(texture, rows, columns, frameDuration, initialPosition);

            IsActive = true;

        }

        public virtual void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            animateSpriteComponent.Update();


            if (IsMagnetized)
            {
                MoveTowardsPlayer(player, deltaTime);
            }
            
        }

        public virtual void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;
            animateSpriteComponent.Draw(renderTexture, deltaTime);
        }

        public FloatRect GetBounds()
        {
            return animateSpriteComponent.HitBoxDimensions;
        }

        public bool CheckCollision(Entity other)
        {
            if (!IsActive) return false;
            return GetBounds().Intersects(other.GetBounds());
        }

        public void SrtHitBoxDimensions(FloatRect newDimens)
        {
            animateSpriteComponent.HitBoxDimensions = newDimens;
        }

        public FloatRect HitBoxDimensions => animateSpriteComponent.HitBoxDimensions;
    }
}
