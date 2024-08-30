using Godot;
using Godot.Collections;
using System;

public partial class ShopManager : Node2D
{
	private CombatStats player;
	private GameMaster master = new GameMaster();
	private Random rng;
	private Array<Item> currentShop;
	public Item highlightedItem;
	public bool purchaseCheck = false;
	[Export] public AudioStreamRandomizer purchaseSound;

	public override void _Ready()
	{
		base._Ready();
		player = ResourceLoader.Load<CombatStats>("res://Custom-Resources/player_stats.tres");
		master = GetTree().Root.GetNode<GameMaster>("GameMaster");
		rng = new Random();
		GetNode<TextureButton>("%PurchaseButton").GrabFocus();
		LoadDeityShop(master.currentDeity);
	}
	public void LoadDeityShop(DeityInfo deity) {
		// receive deity resource from game master
		// load background sprite
		GetNode<TextureRect>("ShopUIContainer").Texture = deity.shopBg;

		// load deity sprite
		GetNode<TextureRect>("ShopUIContainer/DeitySprite").Texture = deity.godSprite;
		GetNode<TextureRect>("ShopUIContainer/StoreCounter").Texture = deity.shopCounter;
        GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(master.currentDeity.greetingAudio, Transform);


        // load player information into panel beside the highlight panel (current base stats, current item)
        HighlightItem("Current", player.currentItem);

		// show current player money
		GetNode<RichTextLabel>("ShopUIContainer/MainColumn2/RightColumnBackground/CoinText").Text = "[center][color=yellow][font_size=64][b]" + player.money + "[/b][/font_size][/color]";
		GetNode<RichTextLabel>("ShopUIContainer/AffinityText").Text = "[center][color=lightgreen][font_size=96][b]" + master.currentDeity.affinity + "[/b][/font_size][/color]";
		// generate shop items (method) based on round number, deity info (affinity in there)
		currentShop = GenerateShopItems(master.currentDeity, master.roundNumber);
		// put shop items in correct spots
		FillShop(currentShop);
	}

	// change to return item array, add args for deity resource / round number
	private Array<Item> GenerateShopItems(DeityInfo deity, float roundNum) {
		// deity info - items already partially created in editor (name, flavorText) 
		Array<Item> items = new Array<Item>(){deity.axe, deity.bow, deity.shield, deity.spear, deity.sword};
		// for loop through array generating values for items 
		for (int i = 0; i < items.Count; i++){
			items[i].health = Mathf.Floor((1f - rng.Next() % 3 / 10f) * (10f + 0.5f * deity.affinity + roundNum * 6f * deity.healthMultiplier));
			items[i].damage = Mathf.Floor((1f - rng.Next() % 3 / 10f) * (3f + 0.2f * deity.affinity + roundNum  * deity.damageMultiplier));
			items[i].speed = Mathf.Floor((1f - rng.Next() % 3 / 10f) * (5f + 0.75f * deity.affinity + roundNum * 2.5f * deity.speedMultiplier));
			items[i].cost = Mathf.Floor((1f - rng.Next() % 3 / 10f) * 0.8f * (items[i].health / 2f + items[i].damage * 1.5f + items[i].speed));
		}
		return items;
	}

	public void HighlightItem(string panelName, Item item) {
		highlightedItem = item;
		GetNode<RichTextLabel>("%" + panelName + "ItemDescrip").Clear();
		GetNode<RichTextLabel>("%" + panelName + "ItemDescrip").AppendText("[center][b][font_size=40]" + item.name + "[/font_size][/b]\n\n[i][font_size=22]" + item.flavorText);
		GetNode<RichTextLabel>("%" + panelName + "HealthStat").Clear();
		GetNode<RichTextLabel>("%" + panelName + "HealthStat").AppendText("[center][font_size=64][b]"+ item.health);
		GetNode<RichTextLabel>("%" + panelName + "DamageStat").Clear();
		GetNode<RichTextLabel>("%" + panelName + "DamageStat").AppendText("[center][font_size=64][b]"+ item.damage);
		GetNode<RichTextLabel>("%" + panelName + "SpeedStat").Clear();
		GetNode<RichTextLabel>("%" + panelName + "SpeedStat").AppendText("[center][font_size=64][b]"+ item.speed);
		GetNode<RichTextLabel>("%" + panelName + "CostText").Clear();
		GetNode<RichTextLabel>("%" + panelName + "CostText").AppendText("[right][color=yellow][font_size=32][b]"+ item.cost + "[/b][/font_size][/color]");
		GetNode<TextureRect>("%" + panelName + "ItemIcon").Texture = item.image;
	}

	private void FillShop(Array<Item> items) {
		for (int i = 0; i < items.Count; i++) {
			ShopItem shopObj = ResourceLoader.Load<PackedScene>("res://Game-Object-Nodes/shop_item.tscn").Instantiate<ShopItem>();
			GetNode("%ItemsBox").AddChild(shopObj);
			shopObj.FillDetails(items[i]);
		}  
	}

	public void PurchaseHighlight() {
		if (highlightedItem.cost <= player.money){
            GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(purchaseSound, Transform);
            player.money -= highlightedItem.cost;
			player.currentItem = highlightedItem;
			HighlightItem("Current", player.currentItem);
			GetNode<RichTextLabel>("ShopUIContainer/MainColumn2/RightColumnBackground/CoinText").Text = "[center][color=yellow][font_size=64][b]" + player.money + "[/b][/font_size][/color]";
			purchaseCheck = true;
		}
	}

	public void LeaveShop() {
        GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(master.currentDeity.leaveAudio, Transform);
        GetTree().Root.GetNode<GameMaster>("GameMaster").LoadElevatorScene();
	}
}
