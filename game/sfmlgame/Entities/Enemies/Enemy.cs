
using SFML.Graphics;
using SFML.System;
using sfmlgame.Entities;
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
            //hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16,4), Color.Red, this);
            //GameScene.Instance._uiManager.AddComponent(hpBar);

        }

        public Enemy(Texture texture, int rows, int columns, Time frameDuration, float speed, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            this.speed = speed;
            MAXHP = 2;
            HP = MAXHP;

            SetPosition(initialPosition);

            //hpBar = new UI_ProgressBar(new Vector2f(initialPosition.X, initialPosition.Y), new UIBinding<int>(() => HP), new UIBinding<int>(() => MAXHP), new Vector2f(16, 4), Color.Red, this);
            //GameScene.Instance._uiManager.AddComponent(hpBar);
        }

        private void CallDamageNumber(int damage)
        {
            //GameScene.Instance._uiManager.CreateDamageNumber(damage, Position, GameScene.Instance._viewCamera.view, 0.45f);
        }

        public override void ResetFromPool(Vector2f position)
        {
            IsActive = true;
            HP = MAXHP;
            SetPosition(position);
            //GameScene.Instance._uiManager.AddComponent(hpBar);
        }

        public bool TakeDamage(int dmg)
        {
            //UniversalLog.LogInfo("Entity took damage " + dmg);

            if(!CanBeDamaged) return false;
            CanBeDamaged = false;
            //SoundManager.Instance.PlayHit();

            CallDamageNumber(dmg);

            HP -= dmg;

            return false;
            //GameScene.Instance._viewCamera.ShakeCamera(3f, 0.115f);

            //GameScene.Instance.particleSystem.SpawnDamageParticles(Position, 3, 50, 1); // spawns 10 particles with a speed spread of 50 and a lifetime of 1 second


            //if (HP <= 0)
            //{
            //    var bluegem = EntityManager.Instance.CreateGem(1, this.Position);
            //    //EntityManager.Instance.AddEntity(bluegem);

            //    GameScene.Instance._uiManager.RemoveComponent(hpBar);

            //    IsActive = false;
            //    //EntityManager.Instance.RemoveEntity(this);

            //    //GameScene.Instance._uiManager.RemoveComponent(hpBar);

            //    return true;
            //}
            //else
            //{
            //    flashTimer = flashDuration;
            //}
            //return false;
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
            
            Vector2f direction = player.Sprite.Position - Position;
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude != 0)
            {
                direction = direction / magnitude; // Normalize the direction vector

                FlipSprite(direction);

                if (magnitude > MinDistance)
                {
                    var lastPos = Position;

                    Position += direction * speed * deltaTime;


                    SetPosition(Position);
                }
            }
            else
            {
                direction = new Vector2f(0, 0); // Or handle this case as appropriate for your game
            }
            //Console.WriteLine("Moving towards player..." + Position.ToString());
        }

        public override void Update(Player player, float deltaTime)
        {
                base.Update(player, deltaTime);
                MoveTowardsPlayer(player, deltaTime);
        }

        public void SetScale(float scale)
        {
            base.animateSpriteComponent.SetScale(scale);
        }

        public void SetPosition(Vector2f pos)
        {
            Position = pos;
            base.animateSpriteComponent.SetPosition(pos);
        }

        public void SetHitBoxDimensions(FloatRect newHitBox)
        {
            base.animateSpriteComponent.HitBoxDimensions = newHitBox;
        }

    }
}
