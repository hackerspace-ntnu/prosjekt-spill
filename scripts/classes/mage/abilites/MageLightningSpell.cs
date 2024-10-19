using Godot;
using System;

public partial class MageLightningSpell : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene lightningSpellScene;

	private Timer timer;
	private Marker3D lightningSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		lightningSpawnpoint = GetNode<Marker3D>("LightningSpawnpoint");

		//Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
	{
		if (!hasShot)
		{
			isShooting = true;
			
			// Instantiate the lightning spell
			Node3D lightningSpell = (Node3D) lightningSpellScene.Instantiate();
			
			// Set the lightning's position to the spawn point (staff or gun)
			lightningSpell.GlobalTransform = lightningSpawnpoint.GlobalTransform;

			// Add the lightning spell to the scene
			GetTree().Root.AddChild(lightningSpell);

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
