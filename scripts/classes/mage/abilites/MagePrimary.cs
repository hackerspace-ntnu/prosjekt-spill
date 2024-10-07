using Godot;
using System;

public partial class MagePrimary : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene fireballScene;

	[Export]
	private RayCast3D shootRayCast;

	private Timer timer;
	private Marker3D fireballSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		fireballSpawnpoint = GetNode<Marker3D>("Staff/FireballSpawnPoint");

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
			
			// Instantiate the fireball
			Node3D fireball = (Node3D) fireballScene.Instantiate();
			
			// Set the fireball's position to the spawn point (staff or gun)
			fireball.GlobalTransform = fireballSpawnpoint.GlobalTransform;
			
			// Set the fireball direction based on the RayCast3D or forward direction
			SetFireballDirection(fireball);

			// Add the fireball to the scene
			GetTree().Root.AddChild(fireball);

			// Start the cooldown timer
			hasShot = true;  // Prevent further shooting until the cooldown ends
			timer.Start();   // Start the cooldown timer
		}
	}

	private void SetFireballDirection(Node3D fireball)
	{
			// Get the collision point and calculate direction from fireball spawn to collision point
			Vector3 targetPosition = shootRayCast.GetCollisionPoint();
			Vector3 spawnPosition = fireballSpawnpoint.GlobalPosition;

			// Calculate the direction from the spawn point to the collision point
			Vector3 direction = (targetPosition - spawnPosition).Normalized();

			// Assuming your Fireball script has a method like SetDirection, cast it and set the direction
			if (fireball is Fireball fireballScript)
			{
				fireballScript.SetDirection(direction);
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
