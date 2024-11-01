using Godot;
using System;
using System.Numerics;

public partial class ShurikenHandler : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene shurikenScene;

	private Timer timer;
	private Marker3D shurikenSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		shurikenSpawnpoint = GetNode<Marker3D>("ShurikenSpawnPoint");

		//Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
	{
        // Sjekk at shurikenSpawnpoint er i scenetreet
        if (!shurikenSpawnpoint.IsInsideTree())
        {
            GD.PrintErr("Shuriken spawn point is not inside the scene tree yet.");
            return;
        }
		if (!hasShot)
		{
			isShooting = true;

            // Angles for shuriken spread (degrees)
            float[] angles = { -10.0f, 0f, 10.0f };
			
            foreach (float angle in angles)
            {
                // Instantiate the shuriken
                Shuriken shuriken = (Shuriken) shurikenScene.Instantiate();

                // Set the shuriken's position to the spawn point (staff or gun)
                shuriken.GlobalTransform = shurikenSpawnpoint.GlobalTransform;

                Godot.Vector3 rotatedDirection = -shurikenSpawnpoint.GlobalTransform.Basis.Z.Normalized()
                .Rotated(Godot.Vector3.Up, Mathf.DegToRad(angle));

                shuriken.SetDirection(rotatedDirection);

                // Add the shuriken to the scene
                GetTree().Root.AddChild(shuriken);

            }

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
