using Godot;
using System;

public partial class MagePrimary : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene fireballScene;

	private Timer timer;
	private Marker3D fireballSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		fireballSpawnpoint = GetNode<Marker3D>("Staff/FireballSpawnPoint");

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

			// Add the fireball to the scene
			GetTree().Root.AddChild(fireball);

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
