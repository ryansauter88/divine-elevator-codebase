using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	private Timer timer;
	private Control damage_bar;
	public ProgressBar damage_bar_progress_bar;

	public float health = 0;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
	}

	public override void _Process(double delta)
	{
	}

	public void init_Health(float _health)
	{
		damage_bar = GetNode<Control>("%DamageBar");
		damage_bar_progress_bar = (ProgressBar)damage_bar;
		health = _health;
		MaxValue = health;
		Value = health;
		damage_bar_progress_bar.MaxValue = health;
		damage_bar_progress_bar.Value = health;
	}

	public float Health
	{
		get { return health; }
		set { _set_health(value); }
	}

	public void _set_health(float new_health)
	{
		var prev_health = health;
		health = Math.Min((float)MaxValue, (float)new_health);
		Value = health;

		if (health <= 0)
		{
			QueueFree();
		}
		
		if (health < prev_health)
		{
			timer.Start();
		}
		else
		{
			damage_bar_progress_bar.Value = health;
		}
	}

	private void _on_timer_timeout()
	{
		damage_bar_progress_bar.Value = Value;
	}
}
