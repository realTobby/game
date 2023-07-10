using game.Managers;
using game.Models;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public class Enemy
    {
        public Vector2f Position { get; set; }

        private Vector2f target;
        private float followDelay;
        private Clock followTimer;

        private Time spawnCooldown;
        private Clock spawnTimer;

        public Enemy(Vector2f initialPosition, Vector2f target, float followDelay, Time spawnCooldown)
        {
            Position = initialPosition;
            this.target = target;
            this.followDelay = followDelay;
            this.spawnCooldown = spawnCooldown;

            followTimer = new Clock();
            spawnTimer = new Clock();
        }

        public void Update(Vector2f newTarget)
        {
            this.target = newTarget;

            // Follow the target with a delay
            if (followTimer.ElapsedTime.AsSeconds() > followDelay)
            {
                Position = target;
                followTimer.Restart();
            }

            // Spawn a new AnimatedSprite every few seconds
            if (spawnTimer.ElapsedTime > spawnCooldown)
            {
                SpawnAnimatedSprite();
                spawnTimer.Restart();
            }
        }

        Random rnd = new Random();

        private void SpawnAnimatedSprite()
        {
            // Create a new AnimatedSprite at the current position
            // You'll need to provide the necessary arguments for the AnimatedSprite constructor
            ThunderStrike newThunder = new ThunderStrike();
            //AnimatedSprite newSprite = new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f));
            newThunder.IsSingleShotAnimation = true;

            Position = new Vector2f((int)rnd.Next((int)Position.X-100, (int)Position.X+100), (int)rnd.Next((int)Position.Y - 100, (int)Position.Y + 100));

            newThunder.SetPosition(Position);

            // Add the new sprite to a list of sprites in your game
            // You'll need to provide this list
            game.Controllers.Game.Instance.animatedSprites.Add(newThunder);
        }

        public void Draw(RenderWindow window)
        {
        }
    }
}
