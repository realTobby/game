
using SFML.Graphics;
using SFML.System;


namespace sfmlgame.Entities.Abilitites
{
    public class AbilityEntity : Entity
    {
        public string AbilityName = string.Empty;

        //TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f), initialPosition
        public AbilityEntity(string name, Vector2f initialPosition, Texture entityTexture, int rowCount, int columnCount, Time frameDuration) : base(entityTexture, rowCount, columnCount, frameDuration, initialPosition)
        {
            IsActive = true;
            AbilityName = name;
        }

        public AbilityEntity(string name, Vector2f initialPosition, Sprite sprite) : base(sprite, initialPosition)
        {
            IsActive = true;
            AbilityName = name;
        }

        public override void ResetFromPool(Vector2f position)
        {
            IsActive = true;
            
            CanCheckCollision = true;
            SetPosition(position);
            //IsActive = true;
            //UniversalLog.LogInfo("hier könnte ihre reset AbilityEntity Logik stehen");
        }

        public override void Update(Player player, float deltaTime)
        {
            if(!IsActive) return;
            base.Update(player, deltaTime);

            // get all active enemies, check if any collision, let them know they got collided with

            // TODO: reDO collision, maybe via an event based system?
            //if(CanCheckCollision)
            //{
            //    foreach (var enemy in EntityManager.Instance.Enemies.ToList())
            //    {
            //        if (enemy.IsActive == false) continue;
            //        if (CheckCollision(enemy))
            //        {

            //            enemy.AbilityCollision(this);
            //        }
            //    }
            //}
        }
    }
}
