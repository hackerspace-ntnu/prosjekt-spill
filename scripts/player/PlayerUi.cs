using Godot;
using System;

public partial class PlayerUi : CanvasLayer
{
	
	[Export]
	private ProgressBar HPbar;
	
	public void takingDamage(float damage)
	{
		GD.Print(damage);
		HPbar.Value -= damage;
	}

	public void updateMaxHealth(float maxHealth)
	{
		HPbar.MaxValue = maxHealth;
		HPbar.Value = maxHealth;
	}
}
