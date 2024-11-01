using Godot;
using System;

public partial class WarriorSlashSpellHandler : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene slashScene;

	private Timer timer;
	private Marker3D slashSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		slashSpawnpoint = GetNode<Marker3D>("SlashSpawnpoint");

		//Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
	{
		if (!hasShot)
		{
			isShooting = true;
			
			// Instantiate the slash spell
			Node3D slash = (Node3D) slashScene.Instantiate();
			
			// Set the slash's position to the spawn point
			slash.GlobalTransform = slashSpawnpoint.GlobalTransform;

			// Add the slash spell to the scene
			GetTree().Root.AddChild(slash);

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
