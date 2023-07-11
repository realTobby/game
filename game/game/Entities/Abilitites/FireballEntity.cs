using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Abilitites
{
    public class FireballEntity : Entity
    {
        public Enemy target;

        public FireballEntity(Vector2f initialPosition, Enemy targetEnemy) : base(TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f), initialPosition)
        {
            target = targetEnemy;

            SetPosition(initialPosition);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();


            if (target != null)
            {
                //Console.WriteLine("Moving fireball");
                var direction = target.Position - Position;
                var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));
                Position += normalizedDirection * 2f;
                SetPosition(Position);
            }
        }
    }
}
