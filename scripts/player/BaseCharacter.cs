using Godot;
using System;

public partial class BaseCharacter : CharacterBody3D
{	
	[ExportGroup("Character Attributes")]
	[Export]
	private float speed = 5.0f;
	
	[Export]
	private float jumpVelocity = 4.5f;

	[Export]
	private float sens = 1.5f;

	[Export]
	public AbilityHandler abilityHandler { get; private set; }

	// OnReadys
	private Camera3D camera;
	private bool escape = false;

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("FirstPersonCam");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		// We retrieve the current velocity, since it is readOnly, and thus create a new "instance" of it.
		Vector3 velocity = Velocity;

		// Jump handling. IsOnFloor() is a built in function for testing wether. 
		if (IsOnFloor() && Input.IsActionJustPressed("Jump"))
		{
			velocity.Y = jumpVelocity;
		}

		// Gets the input vectors for the directions on the xz plane.
		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		// Checks for movement
		if (direction.Length() != 0 && IsOnFloor())
		{	
			velocity = new Vector3(direction.X * speed, velocity.Y, direction.Z * speed);
		}
		// Add Quake air strafing when in air.
		else if (!IsOnFloor())
		{
			velocity = HandleAirVelocity(direction, velocity, delta);
			velocity.Y += GetGravity().Y * (float) delta;
		}
		// Default check
		else
		{
			velocity = new Vector3(0.0f, velocity.Y, 0.0f);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		// Input handling mouse movement.
		if (@event is InputEventMouseMotion eventMouseMotion && !escape)
		{
			RotateY(Mathf.DegToRad(-eventMouseMotion.Relative.X * sens));
			camera.RotateX(Mathf.DegToRad(-eventMouseMotion.Relative.Y * sens));

			float clampedX = Mathf.Clamp(camera.RotationDegrees.X, -80, 80);
			camera.RotationDegrees = new Vector3(clampedX, camera.RotationDegrees.Y, camera.RotationDegrees.Z);
		}

		if (Input.IsActionJustPressed("Escape"))
		{
			escape = !escape;
			Input.MouseMode = escape ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}
	}

	private Vector3 HandleAirVelocity(Vector3 wishDirection, Vector3 currentVelocity, double delta)
	{
		float currentSpeed = currentVelocity.Dot(wishDirection);

		// Arbitrary values, can be changed or customized if necessary.
		// TBH idk how this works, look up the quake air strafe movement.
		float addedSpeed = Mathf.Max(2*speed/3 - currentSpeed, 0);
		float acceleratedSpeed = Mathf.Min(2*speed/3 * 10 * (float) delta, addedSpeed);
		return currentVelocity + acceleratedSpeed * wishDirection;
	}
}
