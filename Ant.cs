using Godot;
using System;
using System.Numerics;
using Vector3 = Godot.Vector3;

public partial class Ant : CharacterBody3D
{
	public CharacterBody3D target;
	private NavigationAgent3D navAgent;
	private Vector3 velocity = Vector3.Zero;
	private Vector3 targetLocation = Vector3.Zero;
	private float speed = 10.0f;
	public override void _Ready()
	{
		target = GetNode<CharacterBody3D>("../Player");
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
	}

	public void _process(float delta)
	{
		if (target != null)
		{
			navAgent.TargetPosition = target.GlobalTransform.Origin;
			Vector3 nexNavPoint = navAgent.GetNextPathPosition();
			this.velocity = (nexNavPoint - GlobalTransform.Origin).Normalized() * speed;
			MoveAndSlide();
		}
	}

	public void _PhysicsProcess(float delta)
	{
		Vector3 velocity = Velocity;

	}

	public void UpdateTargetLocation(Vector3 targetLocation)
	{
		navAgent.TargetPosition = targetLocation;
	}
}
