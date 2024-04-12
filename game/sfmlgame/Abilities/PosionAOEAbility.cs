
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Managers;


namespace sfmlgame.Abilities
{
    //public class PosionAOEAbility : Ability
    //{
    //    private Player player;


    //    public PosionAOEAbility(Player player, float cooldown)
    //        : base("", 1, 2f)
    //    {
    //        this.player = player;

    //        this.Icon = new SFML.Graphics.Sprite(GameAssets.Instance.TextureLoader.GetTexture("fireballIcon", "UI/Abilities"));

    //    }

    //    public int LastFireballCount = 1;

    //    public override void Activate()
    //    {

    //        Enemy nearestEnemy = Game.Instance.EntityManager.FindNearestEnemy(player.GetPosition());
    //        if (nearestEnemy == null)
    //            return;

    //        FireballEntity? fireballEntity = Game.Instance.EntityManager.CreateAbilityEntity(player.GetPosition(), typeof(FireballEntity)) as FireballEntity;
    //        if (fireballEntity == null) return;

    //        SoundManager.Instance.PlayFireProjectile();


    //        fireballEntity.SetPosition(player.GetPosition());
    //        //EntityManager.Instance.AddEntity(new FireballEntity(player.Position, nearestEnemy));

    //        fireballEntity.SetTarget(nearestEnemy);



    //        abilityClock.Restart();

    //        //fireballEntity.IsActive = true;


    //    }

    //    private Vector2f Normalize(Vector2f vector)
    //    {
    //        float magnitude = CalculateMagnitude(vector);
    //        if (magnitude > 0)
    //        {
    //            return vector / magnitude;
    //        }
    //        else
    //        {
    //            return vector;
    //        }
    //    }

    //    private float CalculateMagnitude(Vector2f vector)
    //    {
    //        return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    //    }


    //    public override void Update()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //}
}