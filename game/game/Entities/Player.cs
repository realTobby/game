using game.Abilities;
using game.Entities;
using game.Entities.Enemies;
using game.Entities.Pickups;
using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

public class Player : Entity
{
    private Random rnd = new Random();

    private Vector2f precisePosition;
    private float speed = 150f;

    public int MaxXP = 10;
    public int CurrentXP = 0;

    public List<Ability> Abilities { get; private set; } = new List<Ability>();

    public Player(Vector2f initialPosition)
        : base("Entities", "priestess", 4, initialPosition)
    {
        precisePosition = initialPosition;
        Abilities.Add(new FireballAbility(this, 1.25f, 25f, 5f));
    }

    public string GetUIXPString()
    {
        return CurrentXP + "/" + MaxXP;
    }

    public void Update(float deltaTime)
    {
        HitBoxDimensions = new FloatRect(Position.X, Position.Y, 16, 16);

        base.Update();
        base.SetPosition(new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y)));

        CheckCollisionWithPickups();

       

    }

    private void CheckCollisionWithPickups()
    {
        foreach (Gem gem in GameManager.Instance.GetEntities(new Type[2] {typeof(Gem), typeof(MaxiGem)}))
        {
            if (CheckCollision(gem))
            {
                //Console.WriteLine("XP GAINED!");
                int xpAmount = gem.Pickup();

                while(xpAmount > 0)
                {
                    if (CurrentXP + xpAmount >= MaxXP)
                    {
                        xpAmount -= (MaxXP - CurrentXP);
                        CurrentXP = 0;
                        MaxXP += 5;

                        if(rnd.Next(100) > 50)
                        {
                            Abilities.Add(new FireballAbility(this, 1.25f, 25f, 5f));
                        }
                        else
                        {
                            Abilities.Add(new ThunderStrikeAbility(5f));
                        }

                        
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

    public Vector2f Position
    {
        get { return precisePosition; }
        set { precisePosition = value; }
    }
}
