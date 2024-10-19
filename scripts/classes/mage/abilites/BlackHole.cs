using Godot;
using System;
using System.Collections.Generic;

public partial class BlackHole : Area3D
{
	[Export] public Vector3 origin = Vector3.Zero;  // Origin of the force
	[Export] public float pullStrength = 10.0f;       // The strength of the pulling force

	private List<RigidBody3D> affectedBodies = new List<RigidBody3D>(); //List to track affected bodies

	public override void _Ready()
	{
		// Connect the body entered and exited signals
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Apply gravitational pull to all bodies within list/radius
		foreach (RigidBody3D rigidBody in affectedBodies)
		{
			Vector3 direction = (this.GlobalPosition - rigidBody.GlobalTransform.Origin).Normalized();

			rigidBody.ApplyForce(direction * 10.0f, Vector3.Zero);
		}
	}

	// Called when a body enters the radius
	public void OnBodyEntered(Node body)
	{
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
}
