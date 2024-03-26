using game.Entities.Enemies;
using game.Helpers;
using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Abilitites
{
    public class AbilityEntity : Entity
    {
        public string AbilityName = string.Empty;

        public int Damage = -1;
        public bool CanCheckCollision = false;

        //TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f), initialPosition
        public AbilityEntity(string name, Vector2f initialPosition, Texture entityTexture, int rowCount, int columnCount, Time frameDuration) : base(entityTexture, rowCount, columnCount, frameDuration, initialPosition)
        {
            IsActive = true;
            AbilityName = name;
        }

        public override void ResetFromPool(Vector2f position)
        {
            IsActive = true;
            UniversalLog.LogInfo("hier könnte ihre reset AbilityEntity Logik stehen");
        }

        public override void Update()
        {
            if(!IsActive) return;
            base.Update();

            // get all active enemies, check if any collision, let them know they got collided with

            if(CanCheckCollision)
            {
                var enemies = GameManager.Instance.GetEntities(new Type[] { typeof(Enemy) });

                List<Enemy> listOfY = enemies.Cast<Enemy>().ToList();

                foreach (var enemy in listOfY)
                {
                    if (enemy.IsActive == false) continue;
                    if (CheckCollision(enemy))
                    {

                        enemy.AbilityCollision(this);
                    }
                }
            }
        }
    }
}
