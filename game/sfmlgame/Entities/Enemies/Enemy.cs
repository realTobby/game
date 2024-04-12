
using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Entities.Particles;
using sfmlgame.Managers;
using sfmlgame.UI;

namespace sfmlgame.Entities.Enemies
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

            SetPosition(initialPosition);

            CanCheckCollision = true;
            //hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16,4), Color.Red, this);
            //GameScene.Instance._uiManager.AddComponent(hpBar);

            NormalNormalColor = base.animateSpriteComponent.NormalColors.FirstOrDefault();

        }

        public Enemy(Texture texture, int rows, int columns, Time frameDuration, float speed, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            this.speed = speed;

            SetPosition(initialPosition);

            CanCheckCollision = true;

            //GameScene.Instance._uiManager.AddComponent(hpBar);

            NormalNormalColor = base.animateSpriteComponent.NormalColors.FirstOrDefault();
        }

        public void SetHP(int maxHP)
        {
            MAXHP = maxHP;
            HP = maxHP;

            hpBar = new UI_ProgressBar(new Vector2f(GetPosition().X, GetPosition().Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16, 4), Color.Red, this);
            //Game.Instance.UIManager.AddComponent(hpBar);
        }

        private void CallDamageNumber(int damage)
        {
            //UniversalLog.LogInfo("NewDamageNumberHere");
            Game.Instance.UIManager.CreateDamageNumber(damage, GetPosition(), 0.4f);
           // GameScene.Instance._uiManager.CreateDamageNumber(damage, Position, GameScene.Instance._viewCamera.view, 0.45f);
        }

        private Color NormalNormalColor;

        public override void ResetFromPool(Vector2f position)
        {
            IsActive = true;
            HP = MAXHP;
            SetPosition(position);
            CanCheckCollision = true;

            // Reset sprite colors to normal
            foreach (var item in base.animateSpriteComponent.sprites.ToList())
            {
                item.Color = NormalNormalColor;
            }

            // Reset other components as needed, for example:
            //hpBar.Reset(); // If you have a method to reset the progress bar
        }

        private void GenerateDamageParticles(Vector2f position)
        {
            int numParticles = 3; // Number of particles to generate
            for (int i = 0; i < numParticles; i++)
            {
                

                Game.Instance.EntityManager.CreateDamageParticle(position);
            }
        }

        public bool TakeDamage(int dmg)
        {
            if (!CanBeDamaged) return false;
            CanBeDamaged = false;
            CanCheckCollision = false;
            SoundManager.Instance.PlayHit();
            Game.Instance.ShakeCamera(0.05f);
            CallDamageNumber(dmg);
            GenerateDamageParticles(GetPosition());
            HP -= dmg;

            if (HP <= 0)
            {
                IsActive = false; // Assuming additional logic for spawning items or cleanup is done elsewhere
                flashTimer = 0; // Reset flash timer on defeat
                return true;
            }
            else
            {
                flashTimer = flashDuration; // Reset flash timer on taking damage

            }
            return false;
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
            if(IsActive)
            {
                hpBar.Draw(renderTexture);
            }
            

            HitFlash(deltaTime);
        }

        private void HitFlash(float deltaTime)
        {
            if (flashTimer > 0f)
            {
                flashTimer -= deltaTime;
                if (flashTimer <= 0f)
                {
                    // Ensure all sprites are reset to the normal color
                    foreach (var item in base.animateSpriteComponent.sprites.ToList())
                    {
                        item.Color = NormalNormalColor;
                    }
                    CanCheckCollision = true;
                }
                else
                {
                    // Change color to black during the flash effect
                    foreach (var item in base.animateSpriteComponent.sprites.ToList())
                    {
                        item.Color = new Color(0, 0, 0, 255); // RGBA for black
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

            hpBar.Update(deltaTime);

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
