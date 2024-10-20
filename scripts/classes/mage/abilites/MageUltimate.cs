using Godot;
using System;

public partial class MageUltimate : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene blackholeScene;

	private Timer timer;
	private Marker3D blackholeSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		blackholeSpawnpoint = GetNode<Marker3D>("Staff/BlackholeSpawnpoint");

		//Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
	{
		if (!hasShot)
		{
			isShooting = true;
			
			// Instantiate the blackhole
			Node3D blackhole = (Node3D) blackholeScene.Instantiate();
			
			// Set the blackhole's position to the spawn point (staff or gun)
			blackhole.GlobalTransform = blackholeSpawnpoint.GlobalTransform;

			// Add the blackhole to the scene
			GetTree().Root.AddChild(blackhole);

			// Start the cooldown timer
			hasShot = true;  // Prevent further shooting until the cooldown ends
			timer.Start();   // Start the cooldown timer
		}
	}

	public void CanShoot()
	{
		hasShot = false;
	}

	public void StoppedShooting()
	{
		isShooting = false;
	}
}
