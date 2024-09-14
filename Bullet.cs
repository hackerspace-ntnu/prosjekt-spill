using Godot;
using System;

public partial class Bullet : Node3D
{
	private const float SPEED = 13.0f;
	
	[Export]
	private Timer timer;
	
	[Export]
	private float tobiasKultNr = 4.0f;
	
	public override void _Ready()
	{
		timer.Start();
	}
	
	public override void _Process(double delta)
	{
		Position += Transform.Basis * new Vector3(0.0f, 0.0f, -SPEED * (float) delta);
	}

	public void OnTimerTimeout()
	{
		GD.Print("Hei");
		QueueFree();
	}
}
