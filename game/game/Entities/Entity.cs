using game.Managers;
using game.Models;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public abstract class Entity
    {
        internal AnimatedSprite animateSpriteComponent;


        private Clock deltaClock = new Clock();

        public Vector2f Position { get; set; }

        public int Damage = 0;

        public bool IsMagnetized = false;

        public float GetDeltaTime()
        {
            return deltaClock.Restart().AsSeconds();
        }

        private void MoveTowardsPlayer(Player player, float deltaTime)
        {
            if (GameManager.Instance.IsGamePaused == false)
            {
                Vector2f direction = player.Position - Position;
                float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                direction = direction / magnitude; // Normalize the direction vector
                Position += direction * 300f * deltaTime;
                SetPosition(Position);
            }
        }

        public void SetPosition(Vector2f pos)
        {
            animateSpriteComponent.SetPosition(pos);
        }

        public void SetScale(float scale)
        {
            animateSpriteComponent.SetScale(scale);
        }

        public Entity(string category, string entityName, int frameCount, Vector2f initialPosition)
        {
            Position = initialPosition;

            animateSpriteComponent = new AnimatedSprite(category, entityName, frameCount);
        }

        public Entity(Texture texture, int rows, int columns, Time frameDuration, Vector2f initialPosition)
        {
            Position = initialPosition;

            animateSpriteComponent = new AnimatedSprite(texture, rows, columns, frameDuration, initialPosition);

        }

        public virtual void Update()
        {
            float deltaTime = deltaClock.Restart().AsSeconds();


            animateSpriteComponent.Update();


            if (IsMagnetized)
            {
                MoveTowardsPlayer(GameScene.Instance.player, deltaTime);
            }
            
        }

        public virtual void Draw(float deltaTime)
        {
            animateSpriteComponent.Draw(deltaTime);
        }

        public FloatRect GetBounds()
        {
            return animateSpriteComponent.HitBoxDimensions;
        }

        public bool CheckCollision(Entity other)
        {
            return GetBounds().Intersects(other.GetBounds());
        }

        public void SrtHitBoxDimensions(FloatRect newDimens)
        {
            animateSpriteComponent.HitBoxDimensions = newDimens;
        }

        public FloatRect HitBoxDimensions => animateSpriteComponent.HitBoxDimensions;
    }
}
