using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[ExportGroup("Movement")]
	[Export] public float Speed { get; set; } = 1.0f;
	[Export] public float Jump_Height { get; set; } = 4.0f;
	[Export] public float Sprint_Multiplier { get; set; } = 1.5f;
	[Export] public float Crouch_Multiplier { get; set; } = 0.5f;
	
	[ExportGroup("Mouse")]
	[Export] public float Sensitivity { get; set; } = 2.0f;

	[ExportGroup("Camera")]
	[Export] public float Main_FOV = 70.0f;
	[Export] public float Sprint_FOV_Difference = 20.0f;

	private Vector2 movementAxis = Vector2.Zero;
	private Vector2 mouseAxis = Vector2.Zero;
	private Vector3 velDir = Vector3.Zero;
	private float speedMultiplier = 1.0f;

	private Camera3D cam;
	private CollisionShape3D physicsMesh;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = GetNode<Camera3D>("Head/Camera");
		physicsMesh = GetNode<CollisionShape3D>("Physics");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		cam.Fov = Main_FOV;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		movementAxis.X = Input.GetAxis("L_Movement", "R_Movement");
		movementAxis.Y = Input.GetAxis("B_Movement", "F_Movement");
	}
	public override void _PhysicsProcess(double delta){
		var aim = GlobalTransform.Basis;
		Vector3 moveDir = aim.X * movementAxis.X + -aim.Z * movementAxis.Y;
		moveDir = moveDir.Normalized() * Speed;

		if (!IsOnFloor()) velDir.Y -= (float)ProjectSettings.GetSetting("physics/3d/default_gravity") * (float)delta;
		else if (Input.IsActionPressed("Jump")) velDir.Y = Jump_Height;
		else velDir.Y = 0;

		velDir.X = Mathf.Lerp(velDir.X, moveDir.X, (float)delta * 5.0f);
		velDir.Z = Mathf.Lerp(velDir.Z, moveDir.Z, (float)delta * 5.0f);

		if (Input.IsActionPressed("Sprint")){
			cam.Fov = Mathf.Lerp(cam.Fov, Main_FOV + Sprint_FOV_Difference, (float)delta * 5.0f);
			speedMultiplier = Mathf.Lerp(speedMultiplier, Sprint_Multiplier, (float)delta * 5.0f);
		}
		else if (Input.IsActionPressed("Crouch")){
			cam.Fov = Mathf.Lerp(cam.Fov, Main_FOV - Sprint_FOV_Difference, (float)delta * 5.0f);
			speedMultiplier = Mathf.Lerp(speedMultiplier, 1 / Sprint_Multiplier, (float)delta * 5.0f);
		}
		else{
			cam.Fov = Mathf.Lerp(cam.Fov, Main_FOV, (float)delta * 5.0f);
			speedMultiplier = Mathf.Lerp(speedMultiplier, 1.0f, (float)delta * 5.0f);
		}

		Velocity = velDir * speedMultiplier;

		MoveAndSlide();
	}
	
	public override void _Input(InputEvent @event){
		if (@event is InputEventMouseMotion eventMouseMotion){
			GD.Print(eventMouseMotion.Relative);
			mouseAxis = eventMouseMotion.Relative;
			cam.RotateX((-mouseAxis.Y / 500) * Sensitivity);
			RotateY((-mouseAxis.X / 250) * Sensitivity);

			float camRot = cam.Rotation.X;
			camRot = Mathf.Clamp(camRot, Mathf.DegToRad(-85f), Mathf.DegToRad(85f));
			cam.Rotation = new Vector3(camRot, 0, 0);
		}
		else{
			mouseAxis = Vector2.Zero;
		}
	}
}
