using Godot;
using System;
using System.Reflection;

public partial class HealthComponent : Node2D
{
	[Export] CombatStats stats;
	[Export] AudioStreamRandomizer deathSound;

	private float health;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		health = stats.health;
	}

	public void Damage(Attack attack) {

		health = health - attack.damage;

		if (Equals(GetParent().GetType(),new PlayerMovement().GetType())) {
			// update UI
			GetNode<HealthBar>("/root/Root/GameUI/HealthBar").Health = health;
		}

		if (health <= 0f) {
			if (Equals(GetParent().GetType(),new BasicEnemyBehavior().GetType())) {
				DataManager manager = GetTree().Root.GetNode<DataManager>("Root");
				manager.SpawnCoins(GetParent<BasicEnemyBehavior>().Transform);
			}
			else
			{
				if(deathSound != null)
				{
					GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(deathSound,Transform);
				}
			}
			GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(attack.stream,Transform);
			GetParent().QueueFree();
		}
	}
}
