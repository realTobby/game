using game.Abilities;
using game.Entities;
using game.Entities.Enemies;
using game.Entities.Pickups;
using game.Helpers;
using game.Managers;
using game.Scenes;
using game.UI;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

public class Player : Entity
{
    private Random rnd = new Random();

    private Vector2f precisePosition;
    private float speed = 150f;

    public int MaxXP = 10;
    public int CurrentXP = 0;

    public int Level = 1;

    private UI_PowerupMenu powerupMenu;

    public List<Ability> Abilities { get; private set; } = new List<Ability>();

    public Player(Vector2f initialPosition)
        : base("Entities", "priestess", 4, initialPosition)
    {
        precisePosition = initialPosition;
        //Abilities.Add(new FireballAbility(this, 1.25f, 25f, 5f));

        powerupMenu = new UI_PowerupMenu(new Vector2f(100, 100), GameScene.Instance._viewCamera.view);
        GameScene.Instance._uiManager.AddComponent(powerupMenu);
        SetScale(1.5f);

        Abilities.Add(new OrbitalAbility(this, 5f, 5f, 100f, 3));
    }

    public string GetUIXPString()
    {
        return CurrentXP + "/" + MaxXP;
    }

    public void Update(float deltaTime)
    {
        base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, 16, 16));

        base.Update();
        base.SetPosition(new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y)));

        CheckCollisionWithPickups();

       

    }

    private void CheckCollisionWithPickups()
    {
        foreach (Gem gem in EntityManager.Instance.gemEntities.ToList().Where(x =>x.IsActive))
        {
            if (CheckCollision(gem))
            {
                int xpAmount = gem.Pickup();

                while (xpAmount > 0)
                {
                    
                    if (CurrentXP + xpAmount >= MaxXP)
                    {
                        xpAmount -= (MaxXP - CurrentXP);
                        CurrentXP = 0;
                        MaxXP += 5;
                        Level += 1;

                        UniversalLog.LogInfo("level up!");


                        AbilityFactory af = new AbilityFactory();
                        var newAbility = af.CreateRandomAbility(this);
                        Abilities.Add(newAbility);
                        UniversalLog.LogInfo("Added new ability: " + newAbility.Name);

                        powerupMenu.OpenWindow();
                    }
                    else
                    {
                        CurrentXP += xpAmount;
                        xpAmount = 0;
                    }
                }
            }
        }
    }

    public void Draw(float deltaTime)
    {
        Vector2f roundedPosition = new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y));
        base.SetPosition(roundedPosition);
        base.Draw(deltaTime);
    }

    public void MoveLeft(float deltaTime)
    {
        precisePosition.X -= speed * deltaTime;
    }

    public void MoveRight(float deltaTime)
    {
        precisePosition.X += speed * deltaTime;
    }

    public void MoveUp(float deltaTime)
    {
        precisePosition.Y -= speed * deltaTime;
    }

    public void MoveDown(float deltaTime)
    {
        precisePosition.Y += speed * deltaTime;
    }

    public override void ResetFromPool(Vector2f position)
    {
        UniversalLog.LogInfo("hopefully we do not pool the Player object :)");
    }

    public Vector2f Position
    {
        get { return precisePosition; }
        set { precisePosition = value; }
    }
}
