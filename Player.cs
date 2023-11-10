using Godot;
using System;

public partial class Player : RigidBody3D
{
	[ExportGroup("Movement")]
	[Export]
	public float Speed { get; set; } = 1.0f;
	
	private Vector2 axis = Vector2.ZERO;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		input.get_axis()
	}
}
