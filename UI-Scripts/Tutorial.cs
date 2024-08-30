using Godot;
using Godot.Collections;
using System;

public partial class Tutorial : Control
{
	[Export] public Array<Control> pages;
	private int i = 0;
	private int pageNum = 1;
	private RichTextLabel pageNumText;

	public override void _Ready()
	{
		pageNumText = GetNode<RichTextLabel>("%PageNum");
	}

	public override void _Process(double delta)
	{
	}

	private void _on_next_button_pressed()
	{
		if (i < 4)
		{
			i += 1;
			pages[i - 1].Visible = false;
			pages[i].Visible = true;

			pageNum++;
			pageNumText.Text = "[center][color=#000000]" + pageNum.ToString();
		}
	}

	private void _on_previous_button_pressed()
	{
		if (i > 0)
		{
			i -= 1;
			pages[i + 1].Visible = false;
			pages[i].Visible = true;

			pageNum--;
			pageNumText.Text = "[center][color=#000000]" + pageNum.ToString();
		}

	}

	private void _on_back_pressed()
	{
		GetTree().ChangeSceneToFile("res://Level-Nodes/title.tscn");
	}
}
