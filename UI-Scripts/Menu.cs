using Godot;
using System;

public partial class Menu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("%StartButton").GrabFocus();
	}

	public override void _Process(double delta)
	{

	}

	private void _on_start_button_pressed()
	{
		GetTree().Root.GetNode<GameMaster>("GameMaster").LoadElevatorScene();
	}

	private void _on_tutorial_button_pressed()
	{
		GetTree().Root.GetNode<GameMaster>("GameMaster").LoadTutorialScene();
	}

	private void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
}
