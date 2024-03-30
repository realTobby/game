using game.Entities.Enemies;

using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;


namespace sfmlgame.Entities.Abilitites
{
    public class FireballEntity : AbilityEntity
    {
        public Clock killTimer = new Clock();

        public List<Enemy> previousTargets = new List<Enemy>();

        public Enemy target;

        public FireballEntity(Vector2f initialPosition, Enemy targetEnemy) : base("Fireball", initialPosition, GameAssets.Instance.TextureLoader.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f))
        {
            CanCheckCollision = true;

            SetTarget(targetEnemy);

            SetPosition(initialPosition);

            Damage = 1;
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;

            base.Draw(renderTexture, deltaTime);
        }

        public void SetTarget(Enemy enemyTarget)
        {
            target = enemyTarget;
            killTimer.Restart();
            IsActive = true;
        }

        public override void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            if(killTimer.ElapsedTime > Time.FromSeconds(6))
            {
                //EntityManager.Instance.RemoveEntity(this);
                IsActive = false;
            }

            base.Update(player, deltaTime);

            if(target != null)
            {
                var direction = target.Position - Position;
                var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));
                Position += normalizedDirection * 4f;
                SetPosition(Position);

                base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, HitBoxDimensions.Width, HitBoxDimensions.Height));

                if (target == null) IsActive = false;

                if (target.IsActive == false) IsActive = false;
            }

        }

        public override void CollidedWith(Entity collision)
        {
            IsActive = false;
        }
    }
}
