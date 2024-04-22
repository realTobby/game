using Godot;
using System;

public partial class Player : Sprite2D
{
	public float Speed = 100; // Speed in degrees per second
	public float Radius = 250; // Orbit radius
	private float angle = 0; // Current angle in degrees

	// The center position
	private Vector2 center;

	public override void _Ready()
	{
		center = GetNode<Sprite2D>("Center").Position;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_right"))
		{
			angle -= (float)(Speed * delta);
		}
		if (Input.IsActionPressed("ui_left"))
		{
			angle += (float)(Speed * delta);
		}

		// Convert angle from degrees to radians
		float radians = angle * (Mathf.Pi / 180.0f);
		Position = center + new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * Radius;
	}
}
