
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilities;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;
using System;

namespace sfmlgame.Abilities
{
    public class ScytheAbility : Ability
    {
        private Player player;

        private List<ScytheEntity> allScythes = new List<ScytheEntity>();

        public ScytheAbility(Player player, float cooldown)
            : base("Scythe", 1, cooldown)
        {
            this.player = player;
            this.Icon = new SFML.Graphics.Sprite(GameAssets.Instance.TextureLoader.GetTexture("scythe", "Entities/Abilities"));
        }

        public override void Activate()
        {
            // check if a scythe is "parked" at the player
            var parkedScythes = allScythes.Where(x => x.AtPlayer);

            if(parkedScythes.Count() > 0 )
            {
                foreach(var scythe in parkedScythes)
                {
                    Enemy nearestEnemy = Game.Instance.EntityManager.FindNearestEnemy(player.GetPosition());
                    if (nearestEnemy == null)
                        continue;
                    SoundManager.Instance.PlaySliceEffect();

                    scythe.SetPosition(player.GetPosition());
                    scythe.SetTarget(nearestEnemy);
                }
            }
            else
            {
                ScytheEntity? scytheEntity = Game.Instance.EntityManager.CreateAbilityEntity(player.GetPosition(), typeof(ScytheEntity)) as ScytheEntity;
                if (scytheEntity == null) return;
                allScythes.Add(scytheEntity);

                Enemy nearestEnemy = Game.Instance.EntityManager.FindNearestEnemy(player.GetPosition());
                if (nearestEnemy == null)
                    return;
                SoundManager.Instance.PlaySliceEffect();

                scytheEntity.SetPosition(player.GetPosition());
                scytheEntity.SetTarget(nearestEnemy);

            }

            abilityClock.Restart();
        }

        public override void Update()
        {
            // Update logic if needed
        }
    }
}