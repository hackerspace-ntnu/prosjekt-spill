using Godot;

public partial class Ability : Resource
{
	[Export]
	public Texture2D icon { get; set; }

	[Export]
	public Animation playerAnimation { get; set; }

	public delegate void HandleActivate();
	public event HandleActivate AbilityActivated;

	public void Activate()
	{
		AbilityActivated?.Invoke();
	}
}
