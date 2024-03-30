
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

        private RectangleShape debugDraw;

        private void MoveTowardsPlayer(Player player, float deltaTime)
        {
            Vector2f direction = player.Sprite.Position - Position;
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            direction = direction / magnitude; // Normalize the direction vector
            Position += direction * 300f * deltaTime;
            SetPosition(Position);
        }

        public virtual void CollidedWith(Entity collision)
        {

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

            SetDebugDraw();

            IsActive = true;
        }

        public Entity(Texture texture, int rows, int columns, Time frameDuration, Vector2f initialPosition)
        {
            Position = initialPosition;

            animateSpriteComponent = new AnimatedSprite(texture, rows, columns, frameDuration, initialPosition);

            SetDebugDraw();

            IsActive = true;

            

        }

        private void SetDebugDraw()
        {
            var width = animateSpriteComponent.sprites[0].TextureRect.Width;
            var height = animateSpriteComponent.sprites[0].TextureRect.Height;

            debugDraw = new RectangleShape(new Vector2f(width / 2, height / 2))
            {
                Position = this.Position,
                FillColor = Color.Transparent,
                OutlineColor = Color.Red,
                OutlineThickness = 1,
                Origin = new Vector2f(0, 0)
            };
        }

        public virtual void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            animateSpriteComponent.Update();

            var width = animateSpriteComponent.sprites[0].TextureRect.Width;
            var height = animateSpriteComponent.sprites[0].TextureRect.Height;
            if (debugDraw != null)
            {
                debugDraw.Position = new Vector2f(Position.X - width / 4, Position.Y - height / 4);
            }

            if (IsMagnetized)
            {
                MoveTowardsPlayer(player, deltaTime);
            }
            
        }

        private void ShowDebugBoundaries(RenderTexture target)
        {
            if (debugDraw == null) return;
            target.Draw(debugDraw);
        }


        public virtual void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;
            animateSpriteComponent.Draw(renderTexture, deltaTime);

            ShowDebugBoundaries(renderTexture);
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
