using Godot;
using System;
using System.Collections.Generic;

public partial class BlackHole : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 10.0f; // Speed of the black hole
	public float gravitationalForce = 50.0f; // Strength of the gravitational pull
	public float blackholeHorizonRadius = 5.0f; // Radius of the gravitational pull

	private List<RigidBody3D> affectedBodies = new List<RigidBody3D>();  // List of rigid bodies affected by the black hole

	private Vector3 direction;  // Direction to move in
	private bool isMoving = true; // To control if the blackhole should move

	private Timer timer;        // Timer node to destroy blackhole after some time

	public override void _Ready()
	{
		// Connect the body_entered signal from the Area3D
		BodyEntered += OnBlackholeBodyEntered;
		BodyExited += OnBlackholeBodyExited;

		// Find the Timer node (make sure the Timer is a child of the blackhole)
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;  // Connect the timer's timeout signal
		timer.Start();
	}

	public void SetDirection(Vector3 dir)
	{
		direction = dir.Normalized();  // Normalize the direction vector
	}

	public override void _PhysicsProcess(double delta)
	{

		if (HasOverlappingBodies())
		{
			GD.Print("hello");
		}
		if (isMoving)
		{
			// Move the blackhole in the direction of the raycast
			Position += direction * speed * (float)delta;
		}

		// Pull all objects within radius towards center each frame
		foreach (RigidBody3D body in affectedBodies)
		{
			ApplyGravitationalPull(body, (float)delta);
		}
	}

	private void OnBlackholeBodyEntered(Node3D body)
	{
		// Stop the blackhole when it hits something
		GD.Print("Blackhole hit: ", body.Name);
		isMoving = false;

		// Only applies gravitational pull to RigidBody3D objects
		if (body is RigidBody3D rigidBody)
		{
			affectedBodies.Add(rigidBody);
		}
		GD.Print("Han er i meg");
	}

	private void OnBlackholeBodyExited(Node3D body)
	{
		// Remove body from the "affected" list when the body is no longer within radius
		if (body is RigidBody3D rigidBody)
		{
			affectedBodies.Remove(rigidBody);
		}
		
	}

   private void ApplyGravitationalPull(RigidBody3D body, float delta)
	{
		// Calculate direction from the body to the black hole's center
		Vector3 directionToCenter = GlobalTransform.Origin - body.GlobalTransform.Origin;
		float distance = directionToCenter.Length();

		// Only apply force if the body is within the event horizon radius
		if (distance < blackholeHorizonRadius)
		{
			// Normalize direction and apply gravitational force inversely proportional to the distance
			Vector3 forceDirection = directionToCenter.Normalized();
			float forceStrength = gravitationalForce / (distance * distance);  // Gravity falls off with the square of the distance

			// Apply the impulse to pull the body towards the center
			body.ApplyImpulse(forceDirection * forceStrength * delta);
		}
	}

	private void OnTimerTimeout()
	{
		// Destroy the blackhole when the timer runs out
		GD.Print("Blackhole timed out");
		QueueFree();
	}
}
