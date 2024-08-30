using Godot;

[GlobalClass]
public partial class CombatStats : Resource
{
	[Export] public Item currentItem;
	public Item effect;
	[Export] public float money = 0f;
	[Export] public float health = 100f;
	[Export] public float damage = 30f;
	[Export] public float speed = 5f;
}
