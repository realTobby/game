
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilities;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;

namespace sfmlgame.Abilities
{
    public class ScytheAbility : Ability
    {
        private Player player;

        public ScytheAbility(Player player, float cooldown)
            : base("Scythe", 1, cooldown)
        {
            this.player = player;
            this.Icon = new SFML.Graphics.Sprite(GameAssets.Instance.TextureLoader.GetTexture("scythe", "Entities/Abilities"));
        }

        public override void Activate()
        {
            Enemy nearestEnemy = Game.Instance.EntityManager.FindNearestEnemy(player.GetPosition());
            if (nearestEnemy == null)
                return;

            ScytheEntity? scytheEntity = Game.Instance.EntityManager.CreateAbilityEntity(player.GetPosition(), typeof(ScytheEntity)) as ScytheEntity;
            if (scytheEntity == null) return;

            SoundManager.Instance.PlaySliceEffect();

            scytheEntity.SetPosition(player.GetPosition());
            scytheEntity.SetTarget(nearestEnemy);

            abilityClock.Restart();
        }

        public override void Update()
        {
            // Update logic if needed
        }
    }
}