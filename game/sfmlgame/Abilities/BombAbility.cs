using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Abilities
{
    //public class BombAbility : Ability
    //{

    //    public BombAbility(int damage, float cooldown) : base("Bomb", damage, cooldown)
    //    {
    //        this.Icon = new SFML.Graphics.Sprite(GameAssets.Instance.TextureLoader.GetTexture("bomb", "Entities/Abilities"));
    //    }

    //    public override void Activate()
    //    {
    //        // spawn bomb outside of screen
    //        // make the bomb drop from the top of the screen towards the position of the mouse at the time it was activated
    //        var mousePos = Mouse.GetPosition();

    //        UniversalLog.LogInfo("Mouse Position: " + mousePos.ToString());

    //        // convert to worldPos
    //        var targetWorldPos = Game.Instance.ConvertViewToWorldPosition(mousePos);

    //        UniversalLog.LogInfo("Target Strike Position: " + targetWorldPos.ToString());

    //        var spawnPos = new Vector2f(targetWorldPos.X, targetWorldPos.Y - 500f);

    //        UniversalLog.LogInfo("Spawn Position Position: " + spawnPos.ToString());

    //        Game.Instance.EntityManager.CreateBombEntity(spawnPos, targetWorldPos);

    //    }

    //}
}
