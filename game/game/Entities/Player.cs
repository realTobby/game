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
        foreach (Gem gem in GameManager.Instance.GetEntities(typeof(Gem)))
        {
            if (CheckCollision(gem))
            {
                Console.WriteLine("XP GAINED!");
                gem.Pickup();
                CurrentXP++;
                if (CurrentXP >= MaxXP)
                {
                    SoundManager.Instance.PlayLevelUp();
                    CurrentXP = 0;
                    MaxXP += 5;
                    Abilities.Add(new FireballAbility(this, 1.25f, 25f, 5f));
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
