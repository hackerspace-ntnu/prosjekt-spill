using Godot;

public partial class WarriorDash : Node3D
{
	[Export]
	private float damage = 40.0f;

	[Export]
	private float speed = 100.0f; // Speed for character to dash with

	[Export]
	private float distance =  10.0f; // Distance for character to dash

	private Timer timer;
	private Vector3 dashDirection;
	private float dashDistanceCovered = 0.0f;

	private CharacterBody3D warrior; // Refer to  the character body
	private Camera3D camera; // Refer to camera for dash directon
	private RayCast3D raycast;  // Raycast for collision detection

	// Shoot attributes
	private bool hasDashed = false;
	private bool isDashing = false;

	public override void _Ready()
	{
		// Get referance to the character body
		warrior = GetParent().GetParent() as CharacterBody3D;
		camera = GetParent() as Camera3D;

		// Set up raycast for collision detection
		raycast = GetNode<RayCast3D>("RayCast3D");
		raycast.Enabled = true; //  Enable raycast

		// Connect timer to CanDash method
		timer = GetNode<Timer>("CastCooldown");
		timer.Timeout += CanDash;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isDashing && warrior != null)
		{
			// Calculate distance to move this frame
			float dashStep = speed *  (float)delta;

			// Update raycast for direction to match dash direction
			raycast.TargetPosition = dashDirection * dashStep;

			// TODO: Collision checks and apply damage

			// Stop if reached set distance or wall/ground
			if (raycast.IsColliding())
			{
				// Stop dash if hit wall
				StoppedDashing();
			} 
			else 
			{
				Vector3 velocity = dashDirection * dashStep;
				dashDistanceCovered += velocity.Length();

				warrior.GlobalTransform = new Transform3D(
					warrior.GlobalTransform.Basis,
					warrior.GlobalTransform.Origin + velocity
				);

					//Stop dashing if reached set  distance
					if (dashDistanceCovered >= distance)
					{
						StoppedDashing();
					}
			}
		}
	}

	public void Dash() 
	{
		// Check if the character can dash
		if (!hasDashed && warrior != null) 
		{
			// Start dash and set directon forward
			isDashing = true;
			hasDashed = true;
			dashDirection = -camera.GlobalTransform.Basis.Z.Normalized();

			// Start cooldown timer
			timer.Start();
		}
	}

	public void CanDash()
	{
		hasDashed = false;
	}

	public void StoppedDashing() 
	{
		isDashing = false;
		dashDistanceCovered = 0.0f;

		warrior.Velocity = Vector3.Zero;
	}
}
