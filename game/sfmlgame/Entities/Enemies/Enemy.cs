
using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Entities.Pickups;
using sfmlgame.Managers;
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

        //private UI_ProgressBar hpBar;

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

            SetPosition(initialPosition);

            CanCheckCollision = true;
            //hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16,4), Color.Red, this);
            //GameScene.Instance._uiManager.AddComponent(hpBar);

        }

        public Enemy(Texture texture, int rows, int columns, Time frameDuration, float speed, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            this.speed = speed;
            MAXHP = 2;
            HP = MAXHP;

            SetPosition(initialPosition);

            CanCheckCollision = true;
            //hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16, 4), Color.Red, this);
            //GameScene.Instance._uiManager.AddComponent(hpBar);
        }

        private void CallDamageNumber(int damage)
        {
            //UniversalLog.LogInfo("NewDamageNumberHere");
            Game.Instance.UIManager.CreateDamageNumber(damage, GetPosition(), 0.5f);
           // GameScene.Instance._uiManager.CreateDamageNumber(damage, Position, GameScene.Instance._viewCamera.view, 0.45f);
        }

        public override void ResetFromPool(Vector2f position)
        {
            IsActive = true;
            HP = MAXHP;
            SetPosition(position);
            CanCheckCollision = true;

            // Reset sprite colors to normal
            if (base.animateSpriteComponent.NormalColors != null)
            {
                for (int i = 0; i < base.animateSpriteComponent.sprites.Count(); i++)
                {
                    base.animateSpriteComponent.sprites[i].Color = base.animateSpriteComponent.NormalColors[i];
                }
            }

            // Reset other components as needed, for example:
            // hpBar.Reset(); // If you have a method to reset the progress bar
        }

        public bool TakeDamage(int dmg)
        {
            //UniversalLog.LogInfo("Entity took damage " + dmg);

            if(!CanBeDamaged) return false;
            CanBeDamaged = false;
            CanCheckCollision = false;
            SoundManager.Instance.PlayHit();

            CallDamageNumber(dmg);

            HP -= dmg;

           
            //GameScene.Instance._viewCamera.ShakeCamera(3f, 0.115f);

            //GameScene.Instance.particleSystem.SpawnDamageParticles(Position, 3, 50, 1); // spawns 10 particles with a speed spread of 50 and a lifetime of 1 second


            if (HP <= 0)
            {
                var rndCall = Random.Shared.Next(0, 100);

                if(rndCall < 98)
                {
                    var bluegem = Game.Instance.EntityManager.CreateGem(2, GetPosition());
                    bluegem.IsActive = true;
                }
                else
                {
                    var magnet = Game.Instance.EntityManager.CreateMagnet(GetPosition());
                    magnet.IsActive = true;
                }


                IsActive = false;

                return true;
            }
            else
            {
                flashTimer = flashDuration;
            }
            return false;
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);

            HitFlash(deltaTime);
        }

        private void HitFlash(float deltaTime)
        {
            if (flashTimer > 0f)
            {
                flashTimer -= deltaTime;
                if (flashTimer <= 0f)
                {
                    if (base.animateSpriteComponent.NormalColors == null) return;

                    for (int i = 0; i < base.animateSpriteComponent.sprites.Count(); i++)
                    {
                        base.animateSpriteComponent.sprites[i].Color = base.animateSpriteComponent.NormalColors[i];
                    }
                }
                else
                {
                    foreach (var item in base.animateSpriteComponent.sprites.ToList())
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
                base.animateSpriteComponent.FlipSprite(true);
            }
            else
            {
                base.animateSpriteComponent.FlipSprite(false);
            }
        }

        private void MoveTowardsPlayer(Player player, float deltaTime)
        {
            
            
            Vector2f direction = player.GetPosition() - GetPosition();
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude != 0)
            {
                direction = direction / magnitude; // Normalize the direction vector

                FlipSprite(direction);

                if (magnitude > MinDistance)
                {
                    //Console.WriteLine($"Speed: {speed}, DeltaTime: {deltaTime}, Position Change: {direction * speed * deltaTime}");

                    SetPosition(GetPosition() + direction * speed * deltaTime);
                }
            }
            else
            {
                // we chillin
            }
            //Console.WriteLine("Moving towards player..." + Position.ToString());
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);
            MoveTowardsPlayer(player, deltaTime);

            //base.SetHitBoxDimensions(new FloatRect(GetPosition().X, GetPosition().Y, GetBounds().Width, GetBounds().Height));

            if (CanBeDamaged == false)
            {
                if (invisibleClock.ElapsedTime.AsSeconds() >= invisDuration)
                {
                    CanBeDamaged = true;
                    CanCheckCollision = true;
                    invisibleClock.Restart();
                }
            }
            else
            {
                CheckCollisionWithAbilityEntities();
            }
        }

        public void AbilityCollision(AbilityEntity entity)
        {
            TakeDamage(entity.Damage);
        }

        private void CheckCollisionWithAbilityEntities()
        {
            var abilityEntities = Game.Instance.EntityManager.AbilityEntities;
            //Console.WriteLine(abilityEntities.Count() + " abilities could hurt me!");
            foreach (AbilityEntity ability in abilityEntities.Where(x => x.IsActive))
            {


                if (ability.CanCheckCollision)
                {
                    if (CheckCollision(ability))
                    {

                        ability.CollidedWith(this);
                        //Console.WriteLine("I got hit by " + ability.AbilityName);
                        TakeDamage(ability.Damage);
                    }
                }

            }
        }
    }
}
