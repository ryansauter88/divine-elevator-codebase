using Godot;
using Godot.Collections;
using System;

public partial class DataManager : Node2D
{
	[Export] public Array<Node2D> spawnPoints1, spawnPoints2, spawnPoints3, spawnPoints4;
	[Export] public Array<EnemyVariation> enemyVars;
	private GameMaster master;
	private CombatStats enemyStats;
	private bool canCloseDoor = false;
	private bool enemiesCleared = false;
	private int spawnNum = 0;

	public override void _Ready()
	{
		master = GetTree().Root.GetNode<GameMaster>("GameMaster");
		base._Ready();
		SetEnemyStats(master.roundNumber);
		BeginRound(master.roundNumber);
		CanvasLayer gameUI = GetNode<CanvasLayer>("GameUI");
		gameUI.GetNode<RichTextLabel>("RoundNumberText").Text = "[font_size=30]Round: " + master.roundNumber + "[/font_size]";
		gameUI.GetNode<RichTextLabel>("NextShopText").Text = "[font_size=30][right]Next Shop: " + master.currentDeity.name + "[/right][/font_size]";
		gameUI.GetNode<RichTextLabel>("CoinText").Text = "[right][color=yellow][font_size=32][b]" + master.player.money + "[/b][/font_size][/color]";
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (enemiesCleared) {
			if (!CheckForEnemies()) {
				GD.Print("passing round");
				GetNode<GameUI>("GameUI").RoundPassed();
				enemiesCleared = false;
			}
		}
	}

	public void SetEnemyStats(float roundNum) {
		enemyStats = new CombatStats(){
			health = roundNum * 15f + 200f,
			damage = roundNum * 1f + 9f,
			speed = roundNum * 2.5f + 80f,
			currentItem = new Item(){
				type = ItemType.sword,
				health = 0,
				damage = 0,
				speed = 0,
				cost = 0,
				name = "",
				flavorText = "",
				image = ResourceLoader.Load<CompressedTexture2D>("res://Art/Weapons/sword.svg"),
			},
		};
		spawnNum = 15 + (int)roundNum;
		GetNode<Timer>("SpawnTimer").WaitTime = 2 - roundNum*1/20;
	}

	public void BeginRound(float roundNum) {
		GetNode<Timer>("SpawnTimer").Start();
		GetNode<Timer>("GameUI/RoundTimerText/RoundTimer").Start(18 + spawnNum * (2 - roundNum*1/20));
	}

	public void SpawnTimerTimeout() {
		spawnNum--;
		if (spawnNum <= 0) {GetNode<Timer>("SpawnTimer").Paused = true;}
		SpawnEnemy();
	}

	public bool CheckForEnemies(){
		var children = GetChildren();
		for (int i = 0; i < children.Count; i++){
			if (children[i].SceneFilePath.Contains("enemy")) {
				return true;
			}
		}
		return false;
	}

	public void SpawnEnemy(){
		enemiesCleared = true;
		var basicEnemy = ResourceLoader.Load<PackedScene>("res://Game-Object-Nodes/Characters/basic_enemy.tscn").Instantiate<BasicEnemyBehavior>();

		var spawnPoint = spawnPoints1[0];
		spawnPoint = GetSpawnPoint(spawnPoint);

		basicEnemy.Position = spawnPoint.Position;

		var randomSprite = enemyVars.PickRandom();
		var enemyParts = randomSprite.RandomEnemySpriteParts();
		
		for (int i = 0; i < enemyParts.Count; i++) {
			basicEnemy.GetChild<Sprite2D>(i + 3).Texture = enemyParts[i];
		}
		basicEnemy.stats = enemyStats;
		AddChild(basicEnemy);
	}

	public Node2D GetSpawnPoint(Node2D spawnPoint){
		var playerPos = PlayerPosition();

		Random random = new Random();

		if (playerPos.X < -70 && playerPos.Y < 970)
		{
			// Spawn at spawn points 2 or 3 (right and bottom)
			int randomIndex = random.Next(spawnPoints1.Count);
			spawnPoint = spawnPoints1[randomIndex];
		}
		else if (playerPos.X > -70 && playerPos.Y < 970)
		{
			// Spawn at spawn points 3 or 4 (left and bottom)
			int randomIndex = random.Next(spawnPoints2.Count);
			spawnPoint = spawnPoints2[randomIndex];
			
		}
		else if (playerPos.X > -70 && playerPos.Y > 970)
		{
			// Spawn at spawn points 1 or 4 (left and top)
			int randomIndex = random.Next(spawnPoints3.Count);
			spawnPoint = spawnPoints3[randomIndex];
		}
		else if (playerPos.X < -70 && playerPos.Y > 970)
		{
			// Spawn at spawn points 1 and 2 (right and top)
			int randomIndex = random.Next(spawnPoints4.Count);
			spawnPoint = spawnPoints4[randomIndex];
		}
		return spawnPoint;
	}

	public void SpawnCoins(Transform2D transform) {
		Random rng = new Random();
		float dropCount = MathF.Floor(rng.NextSingle() + 1.3f + master.roundNumber * 0.2f);
		for (int i = 0; i < dropCount; i++) {
			Coin coinObj = ResourceLoader.Load<PackedScene>("res://Game-Object-Nodes/Misc. Objects/coin.tscn").Instantiate<Coin>();
			coinObj.Transform = new Transform2D(transform.X, transform.Y, transform.Origin + new Vector2(150f * (rng.NextSingle() - 0.5f),150f * (rng.NextSingle() - 0.5f)));
			CallDeferred(MethodName.AddChild,coinObj);//AddChild(coinObj);
		}
	}

	public Vector2 PlayerPosition(){
		Node2D playerNode2D = (Node2D)GetNode("%player");
		return playerNode2D.Position;
	}

	public void ActivateInteractElementForDoor(Node2D area){
		// check for if all enemies are defeated
		if (!CheckForEnemies()) {
			canCloseDoor = true;
			GetNode<Sprite2D>("%PressEnterSign").Visible = true;
			GetNode<PlayerMovement>("%player").canDoor = true;
		}
	}

	public void DeactivateInteractElementForDoor(Node2D area){
		// check for if all enemies are defeated
		if (canCloseDoor) {
			GetNode<Sprite2D>("%PressEnterSign").Visible = false;
			GetNode<PlayerMovement>("%player").canDoor = false;
		}
	}

	public void SetDeathState() {
		GetNode<GameOver>("%GameOverOverlay").game_over();
	}
}
