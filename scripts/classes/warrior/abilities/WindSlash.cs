using Godot;

public partial class WindSlash : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 30.0f; // Speed of the slash

	[Export]
	public float damage = 50.0f; // Damage of spell
	public Vector3 targetPosition;
	private Vector3 direction;  // Direction to move in
	private bool isMoving = true; // To control if the spell should move

	public override void _Ready()
	{

		// Set the target position 30 meters away
		targetPosition = GlobalTransform.Origin + -GlobalTransform.Basis.Z.Normalized() * 30.0f;
		direction = (targetPosition - GlobalTransform.Origin).Normalized();

		// Connect the body_entered signal from the Area3D
		BodyEntered += OnSlashBodyEntered;
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

	private void OnSlashBodyEntered(Node3D body)
	{
		// Stop the slash when it hits something
		isMoving = false;
		QueueFree();  // Remove the slash after hitting something

		if (body is BaseCharacter characterBody)
		{
			// Calls takeDamage method on hit character body
			characterBody.takeDamage(damage);
		}
	}
}
