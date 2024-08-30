using Godot;
using System;
using System.Collections;

public partial class AttackComponent : Area2D
{
	public float damage = 1f;
	[Export] public float cooldown = 0f;
	[Export] public ItemType type;
	[Export] public AudioStreamPlayer2D attackSound;
	[Export] public AudioStreamRandomizer hurtSoundClip;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public void DeactivateAttack() {
		QueueFree();
	}
}
