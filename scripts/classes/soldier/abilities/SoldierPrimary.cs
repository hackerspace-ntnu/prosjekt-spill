using Godot;
using System;

public partial class SoldierPrimary : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene bulletScene;

	private Timer timer;
	private bool hasShot = false;
	private bool isShooting = false;
	private float spreadMultiplierX = 0.0f;
	private float spreadMultiplierY = 0.0f;

	public override void _Ready()
	{
		abilityHandler.primaryAbility.AbilityActivated += Shoot;
		timer = GetNode<Timer>("Timer");
	}

	public override void _PhysicsProcess(double delta)
	{
		float t = 2f * (float) delta;

		if (Input.IsActionPressed("Primary"))
		{
			isShooting = true;
			abilityHandler.primaryAbility?.Activate();

			spreadMultiplierY = Mathf.Min(spreadMultiplierY + 0.14f * (float) delta, 7.5f);
			spreadMultiplierX = Mathf.Min(spreadMultiplierX + 0.14f * (float) delta, 0.4f);

			float randomX = (float) new Random().NextDouble() * spreadMultiplierX;
			double randomY = (new Random().NextDouble() - new Random().NextDouble()) * spreadMultiplierY;
			this.Rotation = this.Rotation.Lerp(new Vector3(randomX, (float) randomY, 0.0f), t);
		}

		if (Input.IsActionJustReleased("Primary"))
			isShooting = false;

		if (!isShooting)
		{
			spreadMultiplierY = Mathf.Max(0.0f, spreadMultiplierY - 0.5f * (float) delta);
			spreadMultiplierX = Mathf.Max(0.0f, spreadMultiplierX - 0.5f * (float) delta);
			this.Rotation = this.Rotation.Lerp(Vector3.Zero, t * 2f);
		}

		
	}

	public void Shoot()
	{
		if (!hasShot)
		{
			Node3D bullet = (Node3D) bulletScene.Instantiate();
			bullet.GlobalTransform = this.GlobalTransform;
			Node root = GetTree().Root;
			root.AddChild(bullet);
			hasShot = true;
			timer.Start();
		}
	}

	public void CanShoot()
	{
		hasShot = false;
	}
}
