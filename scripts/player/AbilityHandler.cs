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
	private Node3D primaryNode;
	[Export]
	private string primaryActivateMethodName = "TargetMethod";
	[Export]
	private string primaryDeactivateMethodName = "TargetMethod";

	[ExportSubgroup("Secondary Main")]
	[Export]
	private NodePath secondaryNodePath;
	private Node3D secondaryNode;
	[Export]
	private string secondaryActivateMethodName = "TargetMethod";
	[Export]
	private string secondaryDeactivateMethodName = "TargetMethod";

	[ExportSubgroup("UtilityOne Main")]
	[Export]
	private NodePath utilityOneNodePath;
	private Node3D utilityOneNode;
	[Export]
	private string utilityOneActivateMethodName = "TargetMethod";
	[Export]
	private string utilityOneDeactivateMethodName = "TargetMethod";

	
	[ExportSubgroup("UtilityTwo Main")]
	[Export]
	private NodePath utilityTwoNodePath;
	private Node3D utilityTwoNode;
	[Export]
	private string utilityTwoActivateMethodName = "TargetMethod";
	[Export]
	private string utilityTwoDeactivateMethodName = "TargetMethod";

	[ExportSubgroup("Ultimate Main")]
	[Export]
	private NodePath ultimateNodePath;
	private Node3D ultimateNode;
	[Export]
	private string ultimateActivateMethodName = "TargetMethod";
	[Export]
	private string ultimateDeactivateMethodName = "TargetMethod";

	public override void _Ready()
	{
		if (primaryNodePath != null)
			primaryNode = GetNode<Node3D>(primaryNodePath);

		if (secondaryNodePath != null)
			secondaryNode = GetNode<Node3D>(secondaryNodePath);

		if (utilityOneNodePath != null)
			utilityOneNode = GetNode<Node3D>(utilityOneNodePath);

		if (utilityTwoNodePath != null)
			utilityTwoNode = GetNode<Node3D>(utilityTwoNodePath);

		if (ultimateNodePath != null)
			ultimateNode = GetNode<Node3D>(ultimateNodePath);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("Primary"))
		{
			primaryAbility.ActivatePressed();
			primaryNode?.Call(primaryActivateMethodName);
		}
		else if (Input.IsActionJustReleased("Primary"))
		{
			// Unsure if this is the only else case, thats why else if.
			primaryAbility.ActivateReleased();
			primaryNode?.Call(primaryDeactivateMethodName);
		}

		if (Input.IsActionPressed("Secondary"))
		{
			secondaryAbility.ActivatePressed();
			secondaryNode?.Call(secondaryActivateMethodName);
		}
		else if (Input.IsActionJustReleased("Secondary"))
		{
			// Unsure if this is the only else case, thats why else if.
			secondaryAbility.ActivateReleased();
			secondaryNode?.Call(secondaryDeactivateMethodName);
		}

		if (Input.IsActionPressed("UtilityOne"))
		{
			utilityOneAbility.ActivatePressed();
			utilityOneNode?.Call(utilityOneActivateMethodName);
		}
		else if (Input.IsActionJustReleased("UtilityOne"))
		{
			// Unsure if this is the only else case, thats why else if.
			utilityOneAbility.ActivateReleased();
			utilityOneNode?.Call(utilityOneDeactivateMethodName);
		}

		if (Input.IsActionPressed("UtilityTwo"))
		{
			utilityOneAbility.ActivatePressed();
			utilityTwoNode?.Call(utilityTwoActivateMethodName);
		}
		else if (Input.IsActionJustReleased("UtilityTwo"))
		{
			// Unsure if this is the only else case, thats why else if.
			utilityTwoAbility.ActivateReleased();
			utilityTwoNode?.Call(utilityTwoDeactivateMethodName);
		}

		if (Input.IsActionPressed("Ultimate"))
		{
			ultimateAbility.ActivatePressed();
			ultimateNode?.Call(ultimateActivateMethodName);
		}
		else if (Input.IsActionJustReleased("Ultimate"))
		{
			// Unsure if this is the only else case, thats why else if.
			ultimateAbility.ActivateReleased();
			ultimateNode?.Call(ultimateDeactivateMethodName);
		}
	}
}
