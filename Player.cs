using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	//Use the [Export] to reference something in the inspector
	//[Export]
	
	private PackedScene scene = GD.Load<PackedScene>("res://bullet.tscn");

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("RotateRight", "RotateLeft", "Forward", "Backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		direction.Y = velocity.Y;
		if (direction != Vector3.Zero)
		{
			this.RotateY(Mathf.DegToRad(inputDir.X * 2.5f));
			velocity = new Vector3(direction.X * Speed, velocity.Y, direction.Z * Speed);
		}
		else
		{
			velocity = new Vector3(0.0f, velocity.Y, 0.0f);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			Node3D bullet_instance = (Node3D) scene.Instantiate();
			bullet_instance.Transform = this.Transform;
			bullet_instance.Position = this.Position;
			//AddChild(bullet_instance); // Add as child under this node
			Node root = GetTree().Root;
			root.AddChild(bullet_instance);
		}
	}
	
	
}
