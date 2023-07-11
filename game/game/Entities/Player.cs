using game.Abilities;
using game.Entities;
using SFML.System;

public class Player : Entity
{
    private Vector2f precisePosition;
    private float speed = 150f;

    public List<Ability> Abilities { get; private set; } = new List<Ability>();

    public Player(Vector2f initialPosition)
        : base("Entities", "priestess", 4, initialPosition)
    {
        precisePosition = initialPosition;
        Abilities.Add(new FireballAbility(this, 1.25f, 25f, 5f));
    }

    public void Update(float deltaTime)
    {
        base.Update();
        base.SetPosition(new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y)));
        base.Update();
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
