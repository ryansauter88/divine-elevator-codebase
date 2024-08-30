using Godot;
using System;

public partial class Arrow : CharacterBody2D
{
	[Export] public float speed = 400f;
	public override void _Ready()
	{
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += () => QueueFree();
	}
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void SetArrowDamage(float playerDamage) {
		GetNode<AttackComponent>("AttackComponent").damage = playerDamage;
	}
}
