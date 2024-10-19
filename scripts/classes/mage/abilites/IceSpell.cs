using Godot;
using System;

public partial class IceSpell : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 8.0f; // Speed of the ice spell
    public Vector3 targetPosition;
	private Vector3 direction; // Direction to move in
	private bool isMoving = true; // To control if the ice spell should move
    
    // Timer for freezing enemies
    private Timer freezeTimer;

    private Timer timer; // Timer for how long spell is affected

	public override void _Ready()
	{
		// Set the target position 20 meters away
        targetPosition = GlobalTransform.Origin + -GlobalTransform.Basis.Z * 20.0f;
        direction = (targetPosition - GlobalTransform.Origin).Normalized();

        // Create and set timer for freeze effect
        freezeTimer = new Timer();
        freezeTimer.WaitTime = 4.0f;  //Timer for freeze effect to last
        freezeTimer.OneShot = true; //Make one-time-use
        AddChild(freezeTimer);

        // Connect the body entered and exited signals
		BodyEntered += OnBodyEntered;

	}

    public override void _PhysicsProcess(double delta)
    {
        if (isMoving)
        {
            // Move the spell towards the target
            Position += direction * speed * (float)delta;

            if (Position.DistanceTo(targetPosition) <= 0.1f){
                isMoving = false; // Stop when moving the target
                QueueFree(); // Destroy spell when done/hit target
            }
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is RigidBody3D)
        {
            GD.Print("Hit RigidBody3D " + body.Name);

            // Apply freeze effect to enemy
            var enemy = body as RigidBody3D;
            FreezeEnemy(enemy);

            // Start timer to unfreeze after set time
            freezeTimer.Start();
        }
    }

    private void FreezeEnemy(RigidBody3D enemy)
    {
        // Assuming enemy has speed og movement
        // Reduce movementspeed
        enemy.LinearDamp = 10.0f;

        // Connect freeze timeOut signal to method that unfreezes enemy
        freezeTimer.Timeout += () => UnfreezeEnemy(enemy);
    }

    private void UnfreezeEnemy(RigidBody3D enemy)
    {
        // Restore enemy speed after freeze timeout
        enemy.LinearDamp = 1.0f;
        GD.Print("Unfrozen enemy " + enemy.Name);
    }

	public void SetDirection(Vector3 dir)
	{
		direction = dir.Normalized();  // Normalize the direction vector
	}
}