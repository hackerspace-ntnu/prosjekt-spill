using Godot;
using System;
using System.Collections.Generic;

public partial class BlackHole : Area3D
{
	[Export] public Vector3 origin = Vector3.Zero;  // Origin of the force
	[Export] public float pullStrength = 10.0f;       // The strength of the pulling force
	[Export]public float speed = 5.0f; // Speed that blackhole moves
	public Vector3 targetPosition;
	private Vector3 direction;  // Direction to move in
	private bool isMoving = true; // To control if the blackhole should move
	private CollisionShape3D gravitationalPullRadius; 

	private List<RigidBody3D> affectedBodies = new List<RigidBody3D>(); //List to track affected bodies

	public override void _Ready()
	{
		// Set the target position 20 meters away
		targetPosition = GlobalTransform.Origin + -GlobalTransform.Basis.Z * 20.0f;
		direction = (targetPosition - GlobalTransform.Origin).Normalized();

		// Connect the body entered and exited signals
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;

		// Get the CollisionShape3D for effect radius and disable until target hit
		gravitationalPullRadius = GetNode<CollisionShape3D>("GravitationPullRadius");
		gravitationalPullRadius.SetDeferred("disabled", true); // Disable the CollisionShape3D
	}

	public override void _PhysicsProcess(double delta)
	{
		// Apply gravitational pull to all bodies within list/radius
		foreach (RigidBody3D rigidBody in affectedBodies)
		{
			Vector3 direction = (this.GlobalPosition - rigidBody.GlobalTransform.Origin).Normalized();
			rigidBody.ApplyForce(direction * 10.0f, Vector3.Zero);
		}

		if (isMoving)
		{
			// Move spell towards set target
			Position += direction * speed * (float)delta;

			if (Position.DistanceTo(targetPosition) <= 0.1f)
			{
				ActivateGravitationalPull();
				isMoving = false; // Stop when a target it hit
			}
		}
	}

	// Called when a CollisionShape3D is hit. GravitationalPull activated
	public void OnBodyEntered(Node3D body)
	{
		if (body.HasNode("CollisionShape3D"))
		{
			if (isMoving)
			{
				ActivateGravitationalPull();
				isMoving = false; // Stop moving when hit a CollisionShape3D
			}
		}

		if (body is RigidBody3D rigidBody)
		{
			// Add the entering body to the affected list
			affectedBodies.Add(rigidBody);
		}
	}

	// Called when a body exits the radius
	public void OnBodyExited(Node body)
	{
		if (body is RigidBody3D rigidBody)
		{
			//Remove the body from the affected list
			affectedBodies.Remove(rigidBody);
		}
	}

	//Method to activate gravitational pull
	public void ActivateGravitationalPull()
	{
		gravitationalPullRadius.SetDeferred("disabled", false); //Enable second CollisionShape3D
	}
}
