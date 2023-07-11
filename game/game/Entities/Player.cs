using game.Abilities;
using game.Entities;
using SFML.System;

public class Player : Entity
{
    private Vector2f precisePosition;
    private float speed = 150f;

    public List<Ability> Abilities { get; private set; } = new List<Ability>();

    public List<Entity> AbilityEntities = new List<Entity>();

    public Player(Vector2f initialPosition)
        : base("Entities", "priestess", 4, initialPosition)
    {
        precisePosition = initialPosition;
        Abilities.Add(new FireballAbility(this, 5f, 25f, 5f)); // Example ability
        // Add more abilities as needed
    }

    public void Update(float deltaTime)
    {
        base.Update();
        base.SetPosition(new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y)));
        base.Update();

        foreach(var item in AbilityEntities)
        {
            item.Update();
        }

    }

    public void Draw()
    {
        Vector2f roundedPosition = new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y));
        base.SetPosition(roundedPosition);
        base.Draw();

        foreach(var item in AbilityEntities)
        {
            item.Draw();
        }

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
