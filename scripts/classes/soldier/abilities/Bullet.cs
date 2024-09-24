using Godot;
using System;

public partial class Bullet : Node3D
{
	public override void _Process(double delta)
	{
		Position += Transform.Basis * new Vector3(0.0f, 0.0f, -15.0f * (float) delta);
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
