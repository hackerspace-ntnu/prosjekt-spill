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
	
	
	private bool hasShot = false;
	private bool isShooting = false;
	private float spreadMultiplierX = 0.0f;
	private float spreadMultiplierY = 0.0f;

	public override void _Ready()
	{
		abilityHandler.primaryAbility.AbilityActivated += Shoot;
		timer = GetNode<Timer>("ShootCooldown");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		bulletSpawnpoint = GetNode<Marker3D>("Gun/BulletSpawnPoint");
	}

	public override void _PhysicsProcess(double delta)
	{
		float t = 2f * (float) delta;

		if (Input.IsActionPressed("Primary"))
		{
			isShooting = true;
			abilityHandler.primaryAbility?.Activate();

			spreadMultiplierY = Mathf.Min(spreadMultiplierY + 0.24f * (float) delta, 1.5f);
			spreadMultiplierX = Mathf.Min(spreadMultiplierX + 0.24f * (float) delta, 0.4f);

			float randomX = (float) new Random().NextDouble() * spreadMultiplierX;
			double randomY = (new Random().NextDouble() - new Random().NextDouble()) * spreadMultiplierY;
			this.Rotation = this.Rotation.Lerp(new Vector3(randomX, (float) randomY, 0.0f), t);
			shootRayCast.Rotation = this.Rotation;
		}

		if (Input.IsActionJustReleased("Primary"))
			isShooting = false;

		if (!isShooting)
		{
			spreadMultiplierY = Mathf.Max(0.0f, spreadMultiplierY - 0.5f * (float) delta);
			spreadMultiplierX = Mathf.Max(0.0f, spreadMultiplierX - 0.5f * (float) delta);
			this.Rotation = this.Rotation.Lerp(new Vector3(Mathf.DegToRad(2.0f), Mathf.DegToRad(2.0f), 0.0f), t * 2f);
			shootRayCast.Rotation = new Vector3(this.Rotation.X - Mathf.DegToRad(2.0f), this.Rotation.Y - Mathf.DegToRad(2.0f), 0.0f);
		}

		// This section is used for debugging amount
		// int count = 0;
		// Node root = GetTree().Root;
		// foreach(Node child in root.GetChildren())
		// {
		// 	if (child is BulletHole)
		// 		count++;
		// }

		// GD.Print("BulletHole scenes: ", count);
	}

	public void Shoot()
	{
		// From manual testing there will only be 2 bullets in the scene at a time.
		if (!hasShot)
		{
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
}
