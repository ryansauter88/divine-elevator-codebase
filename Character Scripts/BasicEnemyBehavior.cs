using Godot;
using System;

public partial class BasicEnemyBehavior : CharacterBody2D
{
	[Export] public CombatStats stats;
	[Export] PackedScene swordAttack;
	private PlayerMovement player;
	private bool walkFlag = true;
	private Timer timer;

	public override void _Ready()
	{
		player = GetTree().Root.GetNode<PlayerMovement>("Root/%player");
		timer = GetNode<Timer>("AttackWindup");
	}
	public override void _PhysicsProcess(double delta)
	{
		if (walkFlag) {
			Vector2 vecAng = player.GlobalPosition - GlobalPosition;
			Velocity = new Vector2(stats.speed, stats.speed) * vecAng.Normalized();
			MoveAndSlide();
		}
	}

	public void PlayerEnteredDetection(Area2D area) {
		timer.WaitTime = 0.775f - GetTree().Root.GetNode<GameMaster>("GameMaster").roundNumber/50;
		timer.Start();
		walkFlag = false;
	}

	public void PlayerExitedDetection(Area2D area) {
		timer.Paused = true;
		walkFlag = true;
	}

	public void InstantiateAttackObject() {
		AttackComponent atkCmp = (AttackComponent)swordAttack.Instantiate();
		atkCmp.GetNode<Sprite2D>("Sprite2D").Texture = stats.currentItem.image;
		Vector2 currentDirection = player.GlobalPosition - GlobalPosition;
		atkCmp.Transform = new Transform2D(atkCmp.Transform.X, atkCmp.Transform.Y, currentDirection.Normalized() * 150f);
		atkCmp.Rotation = currentDirection.Angle();
		atkCmp.damage = stats.damage + stats.currentItem.damage;
		AddChild(atkCmp);
		if(atkCmp.attackSound != null)
		{
			atkCmp.attackSound.Play();
		}
	}
}
