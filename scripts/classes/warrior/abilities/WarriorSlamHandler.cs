using Godot;

public partial class WarriorSlamHandler : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene slamScene;

	private Timer timer;
	private Marker3D slamSpawnpoint;

	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	public override void _Ready()
	{
		timer = GetNode<Timer>("CastCooldown");
		slamSpawnpoint = GetNode<Marker3D>("SlamSpawnpoint");

		//Connect Timer timeout to canShoot method
		timer.Timeout += CanShoot;
	}

	public void Shoot()
	{
		if (!hasShot)
		{
			isShooting = true;
			
			// Instantiate the slam spell
			WarriorSlam slam = (WarriorSlam) slamScene.Instantiate();
			
			// Set the slam's position to the spawn point
			slam.GlobalTransform = slamSpawnpoint.GlobalTransform;

			// Add the slam spell to the scene
			GetTree().Root.AddChild(slam);

            // Trigger slam effect
            slam.Slam();

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
