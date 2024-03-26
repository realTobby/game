using game.Entities.Enemies;
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
    public class FireballEntity : AbilityEntity
    {
        public Clock killTimer = new Clock();

        public List<Enemy> previousTargets = new List<Enemy>();

        public Enemy target;

        public FireballEntity(Vector2f initialPosition, Enemy targetEnemy) : base("Fireball", initialPosition, TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f))
        {
            CanCheckCollision = true;

            target = targetEnemy;

            SetPosition(initialPosition);

            Damage = 1;
        }



        public override void Draw(float deltaTime)
        {
            if (!IsActive) return;

            base.Draw(deltaTime);
        }

        public void SetTarget(Enemy enemyTarget)
        {
            target = enemyTarget;
            killTimer.Restart();
            IsActive = true;
        }

        public override void Update()
        {
            if (!IsActive) return;

            if(killTimer.ElapsedTime > Time.FromSeconds(4))
            {
                //EntityManager.Instance.RemoveEntity(this);
                IsActive = false;
            }

            base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, HitBoxDimensions.Width, HitBoxDimensions.Height));

            base.Update();

            if(target != null)
            {
                var direction = target.Position - Position;
                var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));
                Position += normalizedDirection * 2f;
                SetPosition(Position);

                // Check if the fireball has reached its target
                if (CheckCollisionWithEnemy())
                {
                    IsActive = false;
                    //EntityManager.Instance.RemoveEntity(this);
                }

                if (target.IsActive == false) IsActive = false;
            }

            

        }

        private bool CheckCollisionWithEnemy()
        {

            foreach (Enemy enemy in EntityManager.Instance.Enemies.ToList().Where(x =>x.IsActive))
            {
                if (CheckCollision(enemy))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
