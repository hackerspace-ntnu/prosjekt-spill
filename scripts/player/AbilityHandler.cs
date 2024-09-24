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

	
}
