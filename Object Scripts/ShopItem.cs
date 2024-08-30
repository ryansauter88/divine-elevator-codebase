using Godot;
using System;

public partial class ShopItem : Panel
{
	public Item itemData = new Item();

	public void ItemSelected() {
		GetTree().Root.GetNode<ShopManager>("Shop").HighlightItem("Highlighted",itemData);
	}

	public void FillDetails(Item item) {
		itemData = item;
		GetNode<TextureRect>("ItemIcon").Texture = item.image;
		GetNode<RichTextLabel>("ItemDescrip").Clear();
		GetNode<RichTextLabel>("ItemDescrip").AppendText("[color=black][font_size=24]" + item.name + "[/font_size]\n\n\n" + item.flavorText);
		GetNode<RichTextLabel>("CostText").Clear();
		GetNode<RichTextLabel>("CostText").AppendText("[right][color=yellow][font_size=22][b]" + item.cost + "[/b][/font_size][/color]");
	}
}
