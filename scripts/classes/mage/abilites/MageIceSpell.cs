using Godot;
using System;

public partial class MageIceSpell : Node3D
{
	[Export]
	private PackedScene iceSpellScene;  // Packed scene for the ice spell

    [Export]
	private AbilityHandler abilityHandler;

	private Timer timer;
	private Marker3D iceSpellSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		// Get cooldown timer and spell spawn point from the staff
		timer = GetNode<Timer>("CastCooldown");
		iceSpellSpawnpoint = GetNode<Marker3D>("IceSpellSpawnpoint");

		// Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
{
    if (!hasShot)
    {
        isShooting = true;

        // Instantiate the ice spell
        IceSpell iceSpell = (IceSpell)iceSpellScene.Instantiate();

        // Set the ice spell's position to the spawn point (staff)
        iceSpell.GlobalTransform = iceSpellSpawnpoint.GlobalTransform;

        // Add the ice spell to the scene
        GetTree().Root.AddChild(iceSpell);

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
