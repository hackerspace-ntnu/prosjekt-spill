using Godot;
using System;

public partial class BulletHole : Decal
{
	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
