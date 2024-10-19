using Godot;
using System;

public partial class MageWaterWall : Node3D
{
	[Export]
	private PackedScene waterWallScene;  // Packed scene for the water spell

    [Export]
	private AbilityHandler abilityHandler;

	private Timer timer;
	private Marker3D waterWallSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		// Get cooldown timer and spell spawn point from the staff
		timer = GetNode<Timer>("CastCooldown");
		waterWallSpawnpoint = GetNode<Marker3D>("WaterWallSpawnpoint");

		// Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
{
    if (!hasShot)
    {
        isShooting = true;

        // Instantiate the water spell
        WaterWall waterWall = (WaterWall)waterWallScene.Instantiate();

        // Set the water spell's position to the spawn point (staff)
        waterWall.GlobalTransform = waterWallSpawnpoint.GlobalTransform;

        // Add the water spell to the scene
        GetTree().Root.AddChild(waterWall);

        // Start the cooldown timer
        hasShot = true;  // Prevent further shooting until the cooldown ends
        timer.Start();   // Start the cooldown timer
    }
}


	public void CanShoot()
	{
		hasShot = false;  // Allow shooting again after cooldown
	}

	public void StoppedShooting()
	{
		isShooting = false;
	}
}