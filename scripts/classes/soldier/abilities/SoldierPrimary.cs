using Godot;
using System;

public partial class SoldierPrimary : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene bulletScene;

	[Export]
	private RayCast3D shootRayCast;

	[Export]
	private PackedScene bulletHole;

	private Timer timer;
	private AnimationPlayer animationPlayer;
	private Marker3D bulletSpawnpoint;
	
	
	// Shoot attributes
	private bool hasShot = false;
	private bool isShooting = false;

	// AimDownSight attributes
	private bool isADS = false;
	private Vector3 mainPosition;
	private Vector3 mainRotation;
	private float spreadMultiplierX = 0.0f;
	private float spreadMultiplierY = 0.0f;

	public override void _Ready()
	{
		mainPosition = this.Position;
		mainRotation = new Vector3(Mathf.DegToRad(2.0f), Mathf.DegToRad(2.0f), 0.0f);
		abilityHandler.primaryAbility.AbilityActivated += Shoot;
		abilityHandler.primaryAbility.AbilityDeactivated += StoppedShooting;
		abilityHandler.secondaryAbility.AbilityActivated += AimDownSight;
		abilityHandler.secondaryAbility.AbilityDeactivated += HipFire;
		timer = GetNode<Timer>("ShootCooldown");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		bulletSpawnpoint = GetNode<Marker3D>("Gun/BulletSpawnPoint");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Time variable to lerp between.
		float t = 2f * (float) delta;

		if (isShooting)
		{
			// Spread multiplier for x and y axis
			spreadMultiplierY = Mathf.Min(spreadMultiplierY + 0.05f * (float) delta, 0.5f);
			spreadMultiplierX = Mathf.Min(spreadMultiplierX + 0.05f * (float) delta, 0.4f);

			// New "random" x and y rotation.
			float randomX = (float) new Random().NextDouble() * spreadMultiplierX;
			double randomY = (new Random().NextDouble() - new Random().NextDouble()) * spreadMultiplierY;

			// Apply the spread to the rotation
			spreadMultiplierY = Mathf.Min(spreadMultiplierY + 0.05f * (float) delta, 0.5f);
			spreadMultiplierX = Mathf.Min(spreadMultiplierX + 0.05f * (float) delta, 0.4f);

			float randomX = (float) new Random().NextDouble() * spreadMultiplierX;
			double randomY = (new Random().NextDouble() - new Random().NextDouble()) * spreadMultiplierY;
			this.Rotation = this.Rotation.Lerp(new Vector3(randomX, (float) randomY, 0.0f), t);
			shootRayCast.Rotation = this.Rotation;
		}
		else 
		{
			// Return to normal rotation
			spreadMultiplierY = Mathf.Max(0.0f, spreadMultiplierY - 0.5f * (float) delta);
			spreadMultiplierX = Mathf.Max(0.0f, spreadMultiplierX - 0.5f * (float) delta);
			this.Rotation = this.Rotation.Lerp(mainRotation, t * 2f);
			shootRayCast.Rotation = new Vector3(this.Rotation.X - Mathf.DegToRad(2.0f), this.Rotation.Y - Mathf.DegToRad(2.0f), 0.0f);
		}

		this.Position = isADS ? this.Position.Lerp(new Vector3(0.0f, -0.1f, 0.0f), t * 10f) : this.Position.Lerp(mainPosition, t * 10f);
		this.Rotation = isADS ? this.Rotation.Lerp(Vector3.Zero, t * 10f) : this.Rotation.Lerp(mainRotation, t * 10f);
	}
	
	/// <summary>
	/// Handles shooting logic and functionality.
	/// Spawns a bullet packedscene and instantiates it with necessary transforms.
	/// Adds the bullet to the root of the scene.
	/// </summary>
	public void Shoot()
	{
		// From manual testing there will only be 2 bullets in the scene at a time.
		if (!hasShot)
		{
			isShooting = true;
			animationPlayer.Play("GunShot");
			
			Node3D bullet = (Node3D) bulletScene.Instantiate();
			bullet.GlobalTransform = bulletSpawnpoint.GlobalTransform;
			Node root = GetTree().Root;
			root.AddChild(bullet);
			hasShot = true;
			timer.Start();

			if (shootRayCast.IsColliding())
			{
				Node3D bulletHoleInstance = (Node3D) bulletHole.Instantiate();
				Vector3 collisionPoint = shootRayCast.GetCollisionPoint();
				Vector3 collisionNormal = shootRayCast.GetCollisionNormal();

				bulletHoleInstance.Position = collisionPoint;

				// Must retrieve the normal so that the decal doesnt warp
				Quaternion rotation = new Quaternion(Vector3.Up, collisionNormal);
				bulletHoleInstance.Rotation = rotation.GetEuler();

				root.AddChild(bulletHoleInstance);
			}
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

	public void AimDownSight()
	{
		isADS = true;
	}

	public void HipFire()
	{
		isADS = false;
	}
}
