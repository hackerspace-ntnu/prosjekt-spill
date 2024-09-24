using Godot;
using System;

public partial class SoldierPrimary : Node3D
{
	[Export]
	private AbilityHandler abilityHandler;

	[Export]
	private PackedScene bulletScene;

	private Timer timer;
	private Boolean hasShot = false;

	public override void _Ready()
	{
		abilityHandler.primaryAbility.AbilityActivated += Shoot;
		timer = GetNode<Timer>("Timer");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("Primary"))
		{
			abilityHandler.primaryAbility?.Activate();
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
