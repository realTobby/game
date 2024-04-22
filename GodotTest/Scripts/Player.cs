using Godot;
using System;

public partial class Player : Node2D
{
	[Export]
	private NodePath worldGeneratorPath; // Path to the WorldGenerator node in the scene

	private WorldGenerator worldGenerator; // Reference to the WorldGenerator

	public float Speed = 200; // Speed in pixels per second

	public override void _Ready()
	{
		worldGenerator = GetNode<WorldGenerator>(worldGeneratorPath);
		if (worldGenerator == null)
		{
			GD.Print("WorldGenerator not found!");
		}
		else
		{
			GD.Print("WorldGenerator successfully linked!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("ui_right"))
			velocity.X += 1;
		if (Input.IsActionPressed("ui_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			velocity.Y -= 1;

		velocity = velocity * Speed * (float)delta;

		Position += velocity;

		
		if (worldGenerator == null)
		{
			worldGenerator = GetNode<WorldGenerator>(worldGeneratorPath);
			if (worldGenerator == null)
			{
				GD.Print("WorldGenerator not found! == " + worldGeneratorPath);
			}
		}

		// Update the world generator with the new player position
		worldGenerator?.UpdatePlayerPosition(Position);
	}
}
