using Godot;
using System;

public partial class MageUltimate : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene blackholeScene;

	[Export]
	private RayCast3D shootRayCast;

	private Timer timer;
	private Marker3D blackholeSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		blackholeSpawnpoint = GetNode<Marker3D>("Staff/BlackholeSpawnpoint");

		// Ensure the RayCast3D is enabled for accurate collision detection
		shootRayCast.Enabled = true;

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
			
			// Set the blackhole direction based on the RayCast3D or forward direction
			SetBlackholeDirection(blackhole);

			// Add the blackhole to the scene
			GetTree().Root.AddChild(blackhole);

			// Start the cooldown timer
			hasShot = true;  // Prevent further shooting until the cooldown ends
			timer.Start();   // Start the cooldown timer
		}
	}

	private void SetBlackholeDirection(Node3D blackhole)
	{
			// Get the collision point and calculate direction from blackhole spawn to collision point
			Vector3 targetPosition = shootRayCast.GetCollisionPoint();
			Vector3 spawnPosition = blackholeSpawnpoint.GlobalPosition;

			// Calculate the direction from the spawn point to the collision point
			Vector3 direction = (targetPosition - spawnPosition).Normalized();

			// Cast and set direction from blackhole Script
			if (blackhole is BlackHole blackholeScript)
			{
				//blackholeScript.SetDirection(direction);
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
