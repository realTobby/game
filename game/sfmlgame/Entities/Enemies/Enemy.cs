using SFML.Graphics;
using SFML.System;
using sfmlgame.Entities.Abilitites;
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
        private Clock invincibleClock = new Clock(); // Renamed for clarity
        private float invincibilityDuration = .2f; // Duration in seconds for invincibility after being hit

        

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

        public virtual bool TakeDamage(int dmg)
        {
            if (!CanBeDamaged) return false;

            CanBeDamaged = false; // Enemy can't be damaged again until timer expires
            HP -= dmg;
            flashTimer = flashDuration; // Start the flash effect
            invincibleClock.Restart(); // Restart the invincibility timer
            SoundManager.Instance.PlayHit();
            Game.Instance.ShakeCamera(0.05f);
            CallDamageNumber(dmg);
            GenerateDamageParticles(GetPosition());

            if (HP <= 0)
            {
                IsActive = false; // Enemy defeated
                Game.Instance.EntityManager.CreateGem(MAXHP, GetPosition());
                return true;
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
                foreach (var sprite in base.animateSpriteComponent.sprites)
                {
                    sprite.Color = Color.Red; // Flash color, could be set to any noticeable color
                }
                if (flashTimer <= 0f)
                {
                    foreach (var sprite in base.animateSpriteComponent.sprites)
                    {
                        sprite.Color = NormalNormalColor; // Reset to normal color
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
            if (!IsStatic)
            {
                MoveTowardsPlayer(player, deltaTime);
            }
            

            // Check if invincibility duration has expired
            if (!CanBeDamaged && invincibleClock.ElapsedTime.AsSeconds() >= invincibilityDuration)
            {
                CanBeDamaged = true; // Enemy can be damaged again
            }

            CheckCollisionWithAbilityEntities();

            HitFlash(deltaTime); // Process any ongoing hit flash
            hpBar.Update(deltaTime); // Update the health bar
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
                        if(CanBeDamaged)
                        {
                            ability.CollidedWith(this);
                            //Console.WriteLine("I got hit by " + ability.AbilityName);
                            TakeDamage(Game.Instance.PLAYER.Damage + ability.Damage);
                        }
                        
                    }
                }

            }
        }
    }
}
