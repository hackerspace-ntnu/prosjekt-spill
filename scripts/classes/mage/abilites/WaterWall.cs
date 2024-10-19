using Godot;
using System;

public partial class WaterWall : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 8.0f; // Speed of the Water wall
    public Vector3 targetPosition;
	private Vector3 direction; // Direction to move in
	private bool isMoving = true; // To control if the water wall should move

	public override void _Ready()
	{
		// Set the target position x meters away
        targetPosition = GlobalTransform.Origin + -GlobalTransform.Basis.Z * 5.0f;
        direction = (targetPosition - GlobalTransform.Origin).Normalized();

        // Connect the "body_entered" to detect collision
        BodyEntered += OnBodyEntered;
	}

    public override void _PhysicsProcess(double delta)
    {
        if (isMoving)
        {
            // Move the spell towards the target
            Position += direction * speed * (float)delta;

            if (Position.DistanceTo(targetPosition) <= 0.1f){
                isMoving = false; // Stop when reached set target
                QueueFree(); // Destroy spell
            }
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        // Check if entered body is RigidBody3D
        if (body is RigidBody3D rigidBody)
        {
            // Apply force in the direction of the spell
            rigidBody.ApplyCentralImpulse(direction * 10.0f); // Adjustable force
        }
    }

	public void SetDirection(Vector3 dir)
	{
		direction = dir.Normalized();  // Normalize the direction vector
	}
}