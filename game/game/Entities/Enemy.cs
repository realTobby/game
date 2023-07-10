using game.Controllers;
using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public class Enemy : Entity
    {
        public float MinDistance { get; set; } = 25f;

        private float speed;

        public Enemy(string category, string entityName, int frameCount, Vector2f initialPosition, float speed)
            : base(category, entityName, frameCount, initialPosition)
        {
            this.speed = speed;
        }

        public Enemy(Texture texture, int rows, int columns, Time frameDuration, float speed, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            this.speed = speed;
        }

        public virtual void Update(Player player, float deltaTime)
        {
            base.Update();

            Vector2f direction = player.Position - Position;
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude != 0)
            {
                direction = direction / magnitude; // Normalize the direction vector

                if (direction.X < 0)
                {
                    FlipSprite(true);
                }
                else
                {
                    FlipSprite(false);
                }

                // Check if the enemy is within the minimum distance threshold
                if (magnitude > MinDistance)
                {
                    Position += direction * speed * deltaTime;
                }
            }
            else
            {
                direction = new Vector2f(0, 0); // Or handle this case as appropriate for your game
            }

            // Debug: print the enemy's position
            Console.WriteLine("Enemy position: " + Position.X + ", " + Position.Y);

            SetPosition(Position);
        }

        public void SetPosition(Vector2f position)
        {
            Position = position;
            base.SetPosition(position);
        }

    }
}
