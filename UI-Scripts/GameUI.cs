using Godot;
using System;

public partial class GameUI : CanvasLayer
{
	[Export] Timer timer;
	private RichTextLabel timerText;
	private GameMaster master;
	private bool isCountingDown = true;
	private bool passedRound = false;
	private string timerTextBBC = "[b][font_size=60][center]";
	private string timerTextGreen = "[b][font_size=60][center][color=green]";
	private string timerTextRed = "[b][font_size=60][center][color=red]";

	public override void _Ready() {
		timerText = GetNode<RichTextLabel>("RoundTimerText");
		master = GetTree().Root.GetNode<GameMaster>("GameMaster");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (isCountingDown) {
			string timeString = FormatTime((int)timer.TimeLeft);
			timerText.Text = timerTextBBC + timeString;
		}
	}

	public void RoundFailed() {
		if (isCountingDown) {	
			isCountingDown = false;
			timer.Paused = true;
			string timeString = FormatTime((int)timer.TimeLeft);
			timerText.Text = timerTextRed + timeString;
			master.RoundTimerAffinityAdjust(passedRound);}
	}

	public void RoundPassed() {
		if (isCountingDown) {	
			isCountingDown = false;
			timer.Paused = true;
			string timeString = FormatTime((int)timer.TimeLeft);
			timerText.Text = timerTextGreen + timeString;
			passedRound = true;
			master.RoundTimerAffinityAdjust(passedRound);}
	}

	private string FormatTime(int time) {
		var mins = time / 60;
		time -= mins * 60;
		var secs = time;
		return mins.ToString() + ":" + secs.ToString();
	}
}
