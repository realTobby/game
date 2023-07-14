using game.Abilities;
using game.Controllers;
using game.Entities.Abilitites;
using game.Entities.Pickups;
using game.Managers;
using game.Scenes;
using game.UI;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Enemies
{
    public class Enemy : Entity
    {
        public float MinDistance { get; set; } = 25f;

        private float speed;

        public int HP;
        public int MAXHP;

        private UI_ProgressBar hpBar;

        private float flashDuration = .1f; // Duration in seconds for the flash effect
        private float flashTimer = 0f; // Timer for the flash effect


        public bool CanBeDamaged = true;
        private Clock invisibleClock = new Clock();
        private float invisDuration = .5f;
        

        public Enemy(string category, string entityName, int frameCount, Vector2f initialPosition, float speed)
            : base(category, entityName, frameCount, initialPosition)
        {
            this.speed = speed;
            MAXHP = 2;
            HP = MAXHP;

            hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16,4), Color.Red, this);
            GameScene.Instance._uiManager.AddComponent(hpBar);

        }

        public Enemy(Texture texture, int rows, int columns, Time frameDuration, float speed, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            this.speed = speed;
            MAXHP = 2;
            HP = MAXHP;
            hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16, 4), Color.Red, this);
            GameScene.Instance._uiManager.AddComponent(hpBar);
        }

        public bool TakeDamage(int dmg)
        {
            if(!CanBeDamaged) return false;
            CanBeDamaged = false;
            SoundManager.Instance.PlayHit();
            HP -= dmg;
            if (HP <= 0)
            {
                var bluegem = new Gem(Position);
                EntityManager.Instance.AddEntity(bluegem);

                GameScene.Instance._uiManager.RemoveComponent(hpBar);

                EntityManager.Instance.RemoveEntity(this);

                return true;
            }
            else
            {
                flashTimer = flashDuration;
            }
            return false;
        }

        public override void Draw(float deltaTime)
        {
            base.Draw(deltaTime);

            HitFlash(deltaTime);
        }

        private void HitFlash(float deltaTime)
        {
            if (flashTimer > 0f)
            {
                flashTimer -= deltaTime;
                if (flashTimer <= 0f)
                {
                    for (int i = 0; i < sprites.Count(); i++)
                    {
                        sprites[i].Color = NormalColors[i];
                    }
                }
                else
                {
                    foreach (var item in sprites.ToList())
                    {
                        item.Color = new Color(0, 0, 0, 255);
                    }
                }
            }
        }

        private void FlipSprite(Vector2f direction)
        {
            if (direction.X < 0)
            {
                FlipSprite(true);
            }
            else
            {
                FlipSprite(false);
            }
        }

        private void MoveTowardsPlayer(Player player, float deltaTime)
        {
            Vector2f direction = player.Position - Position;
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude != 0)
            {
                direction = direction / magnitude; // Normalize the direction vector

                FlipSprite(direction);

                if (magnitude > MinDistance)
                {
                    Position += direction * speed * deltaTime;

                }
            }
            else
            {
                direction = new Vector2f(0, 0); // Or handle this case as appropriate for your game
            }
        }

        public virtual void Update(Player player, float deltaTime)
        {
            base.Update();
            MoveTowardsPlayer(player, deltaTime);
            SetPosition(Position);

            if(hpBar != null)
            {
                hpBar.Position = new Vector2f(Position.X, Position.Y - base.HitBoxDimensions.Height);
            }

            

            if(CanBeDamaged == false)
            {
                if(invisibleClock.ElapsedTime.AsSeconds() >= invisDuration)
                {
                    CanBeDamaged = true;
                    invisibleClock.Restart();
                }
            }
            else
            {
                CheckCollisionWithEntityType(new Type[] { typeof(AbilityEntity) });
            }

        }

        public void AbilityCollision(AbilityEntity entity)
        {
            TakeDamage(entity.Damage);
        }

        private void CheckCollisionWithEntityType(Type[] entityTypse)
        {
            var abilityEntities = GameManager.Instance.GetEntities(entityTypse);
            //Console.WriteLine(abilityEntities.Count() + " abilities could hurt me!");
            foreach (AbilityEntity ability in abilityEntities)
            {
                if(ability.CanCheckCollision)
                {
                    if (CheckCollision(ability))
                    {
                        //Console.WriteLine("I got hit by " + ability.AbilityName);
                        TakeDamage(ability.Damage);
                    }
                }
                
            }
        }

        public void SetPosition(Vector2f position)
        {
            Position = position;
            base.SetPosition(position);
        }

    }
}
