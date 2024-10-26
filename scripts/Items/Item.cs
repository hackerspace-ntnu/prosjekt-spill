using Godot;
using Godot.Collections;

[GlobalClass]
[Tool]
public partial class Item : Resource
{
	[ExportGroup("Item properties")]
	[Export] public string itemName { set; get; }
	[Export(PropertyHint.Enum, "Common,Rare,Legendary")]

	public string itemGrade { set; get; }
	
	[ExportGroup("Item visuals")]
	[Export] public Texture itemImage { set; get; }
	[Export] public PackedScene itemModel { set; get; }
	
	// Conditional rendering of inspector elements is difficult. Will see if i can convert to costum plugin later
	[ExportGroup("Class Specificity")]
	private bool _classSpecific;
	[Export]
	public bool ClassSpecific
	{
		get => _classSpecific;
		set
		{
			_classSpecific = value;
			NotifyPropertyListChanged();
		}
	}

	[Export(PropertyHint.Enum, "Soldier,Melee,Mage,Engineer")]
	public string ItemClass { set; get; }

	public override void _ValidateProperty(Dictionary property)
	{
		if(property["name"].AsStringName() == PropertyName.ItemClass && !ClassSpecific) {
			property["usage"] = (int)property["usage"].As<PropertyUsageFlags>() | (int)PropertyUsageFlags.ReadOnly;
		}
	}
}
