using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	private CanvasLayer gameOverOverlay;
	[Export] AudioStreamRandomizer playerDeathYell;

	public override void _Ready()
	{
		Visible = false;
		
	}

	private void _on_play_again_button_pressed()
	{
		GetTree().Root.GetNode<GameMaster>("GameMaster").ResetGame();
		GetTree().Root.GetNode<GameMaster>("GameMaster").LoadElevatorScene();
	}

	private void _on_return_to_title_button_pressed()
	{
		GetTree().Paused = false;
		GetTree().Root.GetNode<GameMaster>("GameMaster").ResetGame();
		GetTree().ChangeSceneToFile("res://Level-Nodes/title.tscn");
	}

	public void game_over()
	{
		GetTree().Paused = true;
		Visible = true;
		//GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(playerDeathYell, Transform);
	}
}
