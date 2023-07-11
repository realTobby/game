using game.Managers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace game.Entities.Abilitites
{
    public class FireballEntity : Entity
    {
        public List<Enemy> previousTargets = new List<Enemy>();

        public Enemy target;

        public FireballEntity(Vector2f initialPosition, Enemy targetEnemy) : base(TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f), initialPosition)
        {
            target = targetEnemy;

            SetPosition(initialPosition);
        }

        public override void Draw(float deltaTime)
        {
            base.Draw(deltaTime);
        }

        public override void Update()
        {
            base.Update();

            if(target == null)
            {
                GameManager.Instance.RemoveEntity(this);
            }

            var direction = target.Position - Position;
            var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));
            Position += normalizedDirection * 2f;
            SetPosition(Position);

            // Check if the fireball has reached its target
            if (distance <= .5f) // Adjust the threshold as needed
            {
                // Deal damage to the enemy
                if(target.TakeDamage(1))
                {
                    target = null;
                    GameManager.Instance.RemoveEntity(this);
                    return;
                }

                if (target != null)
                {
                    previousTargets.Add(target);
                    SetPosition(target.Position);
                    Enemy nearestEnemy = GameScene.Instance.FindNearestEnemy(target.Position, GameManager.Instance._waveManager.CurrentEnemies, previousTargets);
                    if (nearestEnemy == null)
                        return;

                    GameManager.Instance.AddEntity(new FireballEntity(target.Position, nearestEnemy) { previousTargets = previousTargets });
                }

                // Remove the fireball entity from the game
                GameManager.Instance.RemoveEntity(this);

            }
        }
    }
}
