using Godot;
using System;

public partial class Coin : Area2D
{
	[Export] public AudioStreamRandomizer coinAudio;
	public void Collected(Area2D area) {
		var master = GetTree().Root.GetNode<GameMaster>("GameMaster");
		master.player.money += 1f;
		GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(coinAudio,Transform);
		// update UI
		GetNode<RichTextLabel>("/root/Root/GameUI/CoinText").Text = "[right][color=yellow][font_size=32][b]" + master.player.money + "[/b][/font_size][/color]";
		QueueFree();
	}
}
