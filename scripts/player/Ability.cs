using Godot;

public partial class Ability : Resource
{
	// This is the texture for the icon in the UI.
	[Export]
	public Texture2D icon { get; set; }

	// This is the animation the player will play when using the ability.
	// Should we have this here? Can be refactored.
	[Export]
	public Animation playerAnimation { get; set; }

	// C# delegate calling all subscribed functions for when ability is activated and deactivated
	public delegate void HandleActivate();
	public event HandleActivate AbilityActivated;
	public event HandleActivate AbilityDeactivated;

	// Calls all subscribed delegates.
	public void ActivatePressed()
	{
		AbilityActivated?.Invoke();
	}

	// Calls all subscribed delegates.
	public void ActivateReleased()
	{
		AbilityDeactivated?.Invoke();
	}
}
