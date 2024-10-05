using Godot;
using System;

public partial class AbilityHandler : Node3D
{
	[ExportGroup("Character Abilities")]
	[Export]
	public Ability primaryAbility { get; private set; }

	[Export]
	public Ability secondaryAbility { get; private set; }

	[Export]
	public Ability utilityOneAbility { get; private set; }

	[Export]
	public Ability utilityTwoAbility { get; private set; }

	[Export]
	public Ability ultimateAbility { get; private set; }

	[ExportGroup("Main abilities")]
	[ExportSubgroup("Primary Main")]
	[Export]
	private NodePath primaryNodePath;
	[Export]
	private string primaryActivateMethodName = "TargetMethod";
	[Export]
	private string primaryDeactivateMethodName = "TargetMethod";

	[ExportSubgroup("Secondary Main")]
	[Export]
	private NodePath secondaryNodePath;
	[Export]
	private string secondaryActivateMethodName = "TargetMethod";
	[Export]
	private string secondaryDeactivateMethodName = "TargetMethod";

	[ExportSubgroup("UtilityOne Main")]
	[Export]
	private NodePath utilityOneNodePath;
	[Export]
	private string utilityOneActivateMethodName = "TargetMethod";
	[Export]
	private string utilityOneDeactivateMethodName = "TargetMethod";

	
	[ExportSubgroup("UtilityTwo Main")]
	[Export]
	private NodePath utilityTwoNodePath;
	[Export]
	private string utilityTwoActivateMethodName = "TargetMethod";
	[Export]
	private string utilityTwoDeactivateMethodName = "TargetMethod";

	[ExportSubgroup("Ultimate Main")]
	[Export]
	private NodePath ultimateNodePath;
	[Export]
	private string ultimateActivateMethodName = "TargetMethod";
	[Export]
	private string ultimateDeactivateMethodName = "TargetMethod";

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("Primary"))
		{
			primaryAbility.ActivatePressed();
		}
		else if (Input.IsActionJustReleased("Primary"))
		{
			// Unsure if this is the only else case, thats why else if.
			primaryAbility.ActivateReleased();
		}

		if (Input.IsActionPressed("Secondary"))
		{
			secondaryAbility.ActivatePressed();
		}
		else if (Input.IsActionJustReleased("Secondary"))
		{
			// Unsure if this is the only else case, thats why else if.
			secondaryAbility.ActivateReleased();
		}

		if (Input.IsActionPressed("UtilityOne"))
		{
			utilityOneAbility.ActivatePressed();
		}
		else if (Input.IsActionJustReleased("UtilityOne"))
		{
			// Unsure if this is the only else case, thats why else if.
			utilityOneAbility.ActivateReleased();
		}

		if (Input.IsActionPressed("UtilityTwo"))
		{
			utilityOneAbility.ActivatePressed();
		}
		else if (Input.IsActionJustReleased("UtilityTwo"))
		{
			// Unsure if this is the only else case, thats why else if.
			utilityTwoAbility.ActivateReleased();
		}

		if (Input.IsActionPressed("Ultimate"))
		{
			ultimateAbility.ActivatePressed();
		}
		else if (Input.IsActionJustReleased("Ultimate"))
		{
			// Unsure if this is the only else case, thats why else if.
			ultimateAbility.ActivateReleased();
		}
	}
}
