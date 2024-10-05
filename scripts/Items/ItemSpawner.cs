using Godot;

// Class meant to spawn items in a scene. Only contains testing code for now.
public partial class ItemSpawner : Node3D
{
	[Export] public Resource itemResource;

	public override void _Ready()
	{
		if (itemResource != null)
		{
			Item item = itemResource as Item;

			if (item != null && item.itemModel != null)
			{
				// Spawn instance of testitem
				Node3D itemInstance = (Node3D)item.itemModel.Instantiate();
				itemInstance.GlobalTransform = new Transform3D(Basis.Identity, new Vector3(0, 1, 0));
				AddChild(itemInstance);
			}
		}
	}
}
