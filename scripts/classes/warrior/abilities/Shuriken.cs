using Godot;

public partial class Shuriken : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 30.0f; // Speed of the shurikens

	[Export]
	public float damage = 5.0f; // Damage of each shuriken
	
	private Vector3 direction;  // Direction to move in
	private bool isMoving = true; // To control if the shurikens should move

	public override void _Ready()
	{
		// Connect the body_entered signal from the Area3D
		BodyEntered += OnShurikenBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isMoving)
		{
			// Move the spell towards the target
			Position += direction * speed * (float)delta;

			/*  Check if the shuriken has reached the target and despawn. 
				Unsure if usefull if map will always have walls
			if (Position.DistanceTo(GlobalTransform.Origin + direction * 20.0f) <= 0.1f)
			{
				isMoving = false; // Stop when moving the target
				QueueFree(); // Destroy spell when done/hit target
				GD.Print("Despawned");
			}
			*/
		}
	}

	public void SetDirection(Vector3 newDirection)
	{
		direction = newDirection.Normalized();
	}

	private void OnShurikenBodyEntered(Node3D body)
	{
		// Stop the shuriken when it hits something
		isMoving = false;
		QueueFree(); 

		if (body is BaseCharacter characterBody)
		{
			// Calls takeDamage method on hit character body (To be changed for enemy body)
			characterBody.takeDamage(damage);
		}
	}
}
