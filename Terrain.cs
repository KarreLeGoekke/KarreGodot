using Godot;
using System;

public partial class Terrain : MeshInstance3D
{
	[Export] CollisionShape3D colShape;
	[Export] float Chunk_Size = 2.0f;
	[Export] float Height_Ratio = 1.0f;
	[Export] float Collision_Shape_Size_Ratio = 0.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		colShape = GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
