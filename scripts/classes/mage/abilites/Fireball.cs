using Godot;
using System;

public partial class Fireball : Area3D  // Inherit from Area3D to detect collisions
{
	[Export]
	public float speed = 20.0f; // Speed of the fireball

	private Vector3 direction;  // Direction to move in
	private bool isMoving = true; // To control if the fireball should move

	private Timer timer;        // Timer node to destroy fireball after some time

	public override void _Ready()
	{
		// Connect the body_entered signal from the Area3D
		BodyEntered += OnFireballBodyEntered;

		// Find the Timer node (make sure the Timer is a child of the fireball)
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;  // Connect the timer's timeout signal
		timer.Start();
	}

	public void SetDirection(Vector3 dir)
	{
		direction = dir.Normalized();  // Normalize the direction vector
	}

	public override void _Process(double delta)
	{
		if (isMoving)
		{
			// Move the fireball in the direction of the raycast
			Position += direction * speed * (float)delta;
		}
	}

	private void OnFireballBodyEntered(Node3D body)
	{
		// Stop the fireball when it hits something
		GD.Print("Fireball hit: ", body.Name);
		isMoving = false;
		QueueFree();  // Remove the fireball after hitting something
	}

	private void OnTimerTimeout()
	{
		// Destroy the fireball when the timer runs out
		GD.Print("Fireball timed out");
		QueueFree();
	}
}
