using game.Controllers;
using game.Entities;
using game.Entities.Abilitites;
using game.Entities.Enemies;
using game.Managers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Abilities
{
    public class FireballAbility : Ability
    {
        private Player player;
        private float circleSpeed;
        private float circleRadius;

        public FireballAbility(Player player, float cooldown, float circleSpeed, float circleRadius)
            : base("Fireball", 1, cooldown)
        {
            this.player = player;
            this.circleSpeed = circleSpeed;
            this.circleRadius = circleRadius;
        }

        public override void Activate()
        {
            Enemy nearestEnemy = GameScene.Instance.FindNearestEnemy(player.Position, GameManager.Instance.GetEntities(new Type[] { typeof(Enemy) }).Cast<Enemy>().ToList() );
            if (nearestEnemy == null)
                return;
            EntityManager.Instance.AddEntity(new FireballEntity(player.Position, nearestEnemy));



            abilityClock.Restart();
        }

        private Vector2f Normalize(Vector2f vector)
        {
            float magnitude = CalculateMagnitude(vector);
            if (magnitude > 0)
            {
                return vector / magnitude;
            }
            else
            {
                return vector;
            }
        }

        private float CalculateMagnitude(Vector2f vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }


        public override void Update()
        {
            //throw new NotImplementedException();
        }

    }
}
