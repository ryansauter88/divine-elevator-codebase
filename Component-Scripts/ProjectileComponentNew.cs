using Godot;
using System;

public partial class ProjectileComponentNew : Node2D
{
	[Export] PackedScene arrow_scn;
	[Export] float arrow_speed = 600f;
	[Export] float aps = 5f;
	[Export] float arrow_damage = 30f;

	[Export] private PlayerSprites spriteSet;

	float fire_rate;

	float time_until_fire = 0f;

	public override void _Ready()
	{
		fire_rate = 1 / aps;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("shoot") && time_until_fire  > fire_rate)
		{
			RigidBody2D arrow = arrow_scn.Instantiate<RigidBody2D>();

			string playerDirection = spriteSet.directionFacing;

			if (playerDirection == "front")
			{
				arrow.Rotation = 1.57f;
			}
			else if (playerDirection == "back")
			{
				CanvasItem arrowCanvasItem = (CanvasItem)arrow;
				arrowCanvasItem.ZIndex = -2;
				arrow.Rotation = 4.72f;
			}
			else if(playerDirection == "right")
			{
				arrow.Rotation = 6.29f;
			}
			else if (playerDirection == "left")
			{
				arrow.Rotation = 9.41f;
			}

			arrow.GlobalPosition = GlobalPosition;
			arrow.LinearVelocity = arrow.Transform.X * arrow_speed;

			GetTree().Root.AddChild(arrow);

			time_until_fire = 0f;
		}
		else
		{
			time_until_fire += (float)delta;
		}
	}
}
