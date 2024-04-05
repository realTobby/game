
using sfmglame.Helpers;
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

        public int Damage = 0;

        public bool IsMagnetized = false;

        private RectangleShape debugDraw;

        public bool CanCheckCollision = false;

        private void MoveTowardsPlayer(Player player, float deltaTime)
        {
            Vector2f direction = player.GetPosition() - GetPosition();
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            direction = direction / magnitude; // Normalize the direction vector
            SetPosition(animateSpriteComponent.GetPosition() + direction * 300f * deltaTime);
        }

        public virtual void CollidedWith(Entity collision)
        {

        }

        public void SetRotation(float rotation)
        {
            animateSpriteComponent.SetRotation(rotation);
        }

        public void SetPosition(Vector2f pos)
        {
            animateSpriteComponent.SetPosition(pos);
        }

        public Vector2f GetPosition()
        {
            return animateSpriteComponent.GetPosition();
        }

        public void SetScale(float scale)
        {
            animateSpriteComponent.SetScale(scale);
        }

        public Entity(string category, string entityName, int frameCount, Vector2f initialPosition)
        {
            animateSpriteComponent = new AnimatedSprite(category, entityName, frameCount);
            animateSpriteComponent.SetPosition(initialPosition);

            SetDebugDraw();

            IsActive = true;
        }

        public Entity(Texture texture, int rows, int columns, Time frameDuration, Vector2f initialPosition)
        {

            animateSpriteComponent = new AnimatedSprite(texture, rows, columns, frameDuration, initialPosition);

            animateSpriteComponent.SetPosition(initialPosition);

            SetDebugDraw();

            IsActive = true;
        }

        public Entity(Sprite sprite, Vector2f initialPosition)
        {
            animateSpriteComponent = new AnimatedSprite(sprite, initialPosition);

            animateSpriteComponent.SetPosition(initialPosition);

            SetDebugDraw();

            IsActive = true;
        }

        private void SetDebugDraw()
        {
            var width = animateSpriteComponent.sprites[0].TextureRect.Width;
            var height = animateSpriteComponent.sprites[0].TextureRect.Height;

            debugDraw = new RectangleShape(new Vector2f(width / 2, height / 2))
            {
                Position = GetPosition(),
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
                debugDraw.Position = new Vector2f(animateSpriteComponent.sprites[0].GetGlobalBounds().Left, animateSpriteComponent.sprites[0].GetGlobalBounds().Top);
                debugDraw.Size = new Vector2f(animateSpriteComponent.sprites[0].GetGlobalBounds().Width, animateSpriteComponent.sprites[0].GetGlobalBounds().Height);

                //debugDraw.Position = new Vector2f(GetPosition().X  - width / 4, GetPosition().Y - height / 4);
            }

            if (IsMagnetized)
            {
                //UniversalLog.LogInfo("actually moving towards player!");
                MoveTowardsPlayer(player, deltaTime);
            }

            SetHitBoxDimensions(new FloatRect(GetPosition().X - animateSpriteComponent.sprites[0].GetGlobalBounds().Width/2, GetPosition().Y - animateSpriteComponent.sprites[0].GetGlobalBounds().Height / 2, animateSpriteComponent.sprites[0].GetGlobalBounds().Width, animateSpriteComponent.sprites[0].GetGlobalBounds().Height));
        }


        private void ShowDebugBoundaries(RenderTexture target)
        {
            if(Game.Instance.Debug)
            {
                if (debugDraw == null) return;
                target.Draw(debugDraw);
            }
            
        }

        public virtual void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;
            animateSpriteComponent.Draw(renderTexture, deltaTime);

            if(CanCheckCollision)
                ShowDebugBoundaries(renderTexture);
        }

        public FloatRect GetBounds()
        {
            return animateSpriteComponent.HitBoxDimensions;
        }

        public bool CheckCollision(Entity other)
        {
            if (!IsActive) return false;

            bool collided = animateSpriteComponent.sprites[0].GetGlobalBounds().Intersects(other.animateSpriteComponent.sprites[0].GetGlobalBounds());

            if(collided)
            {
                other.CollidedWith(this);
            }

            return collided;
        }

        public void SetHitBoxDimensions(FloatRect newDimens)
        {
            animateSpriteComponent.HitBoxDimensions = newDimens;
        }
    }
}
