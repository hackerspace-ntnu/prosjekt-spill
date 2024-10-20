using Godot;

public partial class Fireball : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 10.0f; // Speed of the fireball
	public Vector3 targetPosition;
	private Vector3 direction;  // Direction to move in
	private bool isMoving = true; // To control if the fireball should move

	public override void _Ready()
	{

		// Set the target position 20 meters away
		targetPosition = GlobalTransform.Origin + -GlobalTransform.Basis.Z.Normalized() * 20.0f;
		direction = (targetPosition - GlobalTransform.Origin).Normalized();

		// Connect the body_entered signal from the Area3D
		BodyEntered += OnFireballBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isMoving)
		{
			// Move the spell towards the target
			Position += direction * speed * (float)delta;

			if (Position.DistanceTo(targetPosition) <= 0.1f)
			{
				isMoving = false; // Stop when moving the target
				QueueFree(); // Destroy spell when done/hit target
			}
		}
	}

	private void OnFireballBodyEntered(Node3D body)
	{
		// Stop the fireball when it hits something
		isMoving = false;
		QueueFree();  // Remove the fireball after hitting something
	}
}
