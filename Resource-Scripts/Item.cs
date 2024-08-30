using Godot;

[GlobalClass]
public partial class Item : Resource {
	[Export] public ItemType type;
	[Export] public float health;
	[Export] public float damage;
	[Export] public float speed;
	[Export] public float cost;
	[Export] public string name;
	[Export] public string flavorText;
	[Export] public CompressedTexture2D image;
}
