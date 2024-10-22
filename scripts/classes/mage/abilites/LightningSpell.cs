using Godot;

public partial class LightningSpell : Area3D // Inherit from Area 3D to detect collision
{
    [Export]
    public float speed = 10.0f;
    [Export]
    public float effectRadius = 10.0f; // Radius for which lightning spell can affect other bodies
    [Export]
    public float delayBeforeFree = 0.5f; // Delay before QueueFree to let effect finish

    public Vector3 targetPosition;
    private CollisionShape3D effectRadiusShape; // Referance to effectRadius collision shape
    private Vector3 direction; // Direction to move in
    private bool isMoving = true; // To control if the lightning spell is moving


    public override void _Ready()
	{
		// Set the target position x meters away
        targetPosition = GlobalTransform.Origin + -GlobalTransform.Basis.Z * 20.0f;
        direction = (targetPosition - GlobalTransform.Origin).Normalized();

        // Connect the "body_entered" to detect collision
        BodyEntered += OnBodyEntered;

        // Get the CollisionShape3D for effect radius and disable until target hit
        effectRadiusShape = GetNode<CollisionShape3D>("EffectRadius");
        effectRadiusShape.SetDeferred("disabled", true); // Disable the CollisionShape3D
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
            // Stop moving the spell
            isMoving = false;

            // Apply force in the direction of the spell
            rigidBody.ApplyCentralImpulse(direction * 10.0f); // Adjustable force CHANGE FOR LIGHTNING SPELL

            effectRadiusShape.SetDeferred("disabled", false); // Enable the EffectRadius if target hit

            // Affect nearby rigidbodies within effectRadius
            AffectNearbyTargets(rigidBody);

            // Start timer to Queuefree after a short delay
            Timer delayTimer = new Timer();
            AddChild(delayTimer);
            delayTimer.WaitTime = delayBeforeFree;
            delayTimer.OneShot = true;
            delayTimer.Timeout += () => QueueFree();
            delayTimer.Start();
        }
    }

    private void AffectNearbyTargets(RigidBody3D currentTarget)
    {
        // Get the global position of the hit target
        Vector3 currentPosition = currentTarget.GlobalTransform.Origin;

        // Get the list of all bodies withing the radius
        foreach (var body in GetOverlappingBodies())
        {
            if (body is RigidBody3D rigidBody && rigidBody != currentTarget)
            {
                // Calculate distance between target and body
                float distance = targetPosition.DistanceTo(rigidBody.GlobalTransform.Origin);

                // If within affect radius, apply force
                if (distance <= effectRadius)
                {
                    rigidBody.ApplyCentralImpulse(direction * 10.0f); // TEMPORARY EFFECT FOR HIT BODIES
                }
            }
        }
    }
}