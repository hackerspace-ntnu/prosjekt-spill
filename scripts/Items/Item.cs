using Godot;

[GlobalClass]
public partial class Item : Resource
{
	[Export] public string itemName { set; get; }
	[Export(PropertyHint.Enum, "Common,Rare,Legendary")]
	public string itemGrade { set; get; }
	[Export] public Texture itemImage { set; get; }
	[Export] public PackedScene itemModel { set; get; }
}
