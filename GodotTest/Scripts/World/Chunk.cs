using Godot;
using System;

public class Chunk
{
    public Vector2I Origin { get; set; }
    public bool IsActive { get; set; }



    // Constructor to initialize the chunk
    public Chunk(Vector2I origin)
    {
        Origin = origin;
        IsActive = false;
    }
}

