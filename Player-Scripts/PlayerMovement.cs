using Godot;
using Godot.Collections;
using System;
using System.Runtime.InteropServices;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	private CombatStats stats;
	[Export]
	private PlayerSprites spriteSet;
	[Export]
	private Array<PackedScene> attackScenes;
	[Export]
	private PackedScene arrow;
	[Export] public AudioStreamRandomizer bellSound;
	private PackedScene currentAtk;
	private HealthBar healthbar;
	private Array<CompressedTexture2D> activeSpriteArray;
	private Vector2 currentVelocity;
	private Vector2 moveInput;
	private Vector2 currentDirection;
	private bool atkPress = false;
	private bool atkCooldown = false;
	public bool canDoor = false;
	public bool interactPress = false;
	public float health = 0;

	public override void _Ready()
	{
		base._Ready();
		activeSpriteArray = spriteSet.bothDown;
		for (int i = 0; i < attackScenes.Count; i++) {
			var atk = attackScenes[i].Instantiate<AttackComponent>();
			if (atk.type == stats.currentItem.type) {
				currentAtk = attackScenes[i];
			}
		}

		health = stats.health;
		healthbar = GetNode<HealthBar>("%HealthBar");
		healthbar.init_Health(health);
	}
	public override void _PhysicsProcess(double Delta)
	{
		base._PhysicsProcess(Delta);

		handleInput();

		if (interactPress) 
		{
			GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(bellSound, Transform);
			GetTree().Root.GetNode<GameMaster>("GameMaster").LoadShopScene();
		}

		if (moveInput != Vector2.Zero) {
			GetNode<Sprite2D>((NodePath)"PlayerSprite").Texture = spriteSet.SpriteDirection(moveInput,activeSpriteArray);
			currentDirection = moveInput;
		}
		else {
			if (spriteSet.directionFacing == "front") {currentDirection = Vector2.Down;}
			else if (spriteSet.directionFacing == "back") {currentDirection = Vector2.Up;}
			else if (spriteSet.directionFacing == "right") {currentDirection = Vector2.Right;}
			else if (spriteSet.directionFacing == "left") {currentDirection = Vector2.Left;}
			GetNode<Sprite2D>((NodePath)"PlayerSprite").Texture = spriteSet.SpriteDirection(currentDirection,activeSpriteArray);
		}

		if (atkPress && !atkCooldown) {InstantiateAttackObject();}
		Velocity = currentVelocity;
		MoveAndSlide();
	}

	private void handleInput()
	{
		moveInput = Input.GetVector("left", "right", "up", "down");
		currentVelocity = moveInput * (stats.speed + stats.currentItem.speed);

		atkPress = Input.IsActionJustPressed("attack");

		if (canDoor) {interactPress = Input.IsActionJustPressed("enter");}
	}

	private void InstantiateAttackObject() {
		atkCooldown = true;
		SetSpriteArray("right up");
		var timer = GetNode<Timer>((NodePath)"/root/Root/player/AttackCooldown");
		AttackComponent atkCmp = (AttackComponent)currentAtk.Instantiate();
		timer.WaitTime = atkCmp.cooldown;
		timer.Start();
		atkCmp.GetNode<Sprite2D>("Sprite2D").Texture = stats.currentItem.image;
		atkCmp.Transform = new Transform2D(atkCmp.Transform.X, atkCmp.Transform.Y, currentDirection * 150f);
		atkCmp.Rotation = currentDirection.Angle();
		if (atkCmp.type == ItemType.bow) {
			var arrowLive = arrow.Instantiate<Arrow>();
			arrowLive.Velocity = arrowLive.speed * currentDirection;
			arrowLive.GlobalPosition = GlobalPosition;
			arrowLive.Rotation = currentDirection.Angle();
			arrowLive.SetArrowDamage(stats.damage + stats.currentItem.damage);
			GetTree().Root.AddChild(arrowLive);
		}
		atkCmp.damage = stats.damage + stats.currentItem.damage;
		AddChild(atkCmp);
		if(atkCmp.attackSound != null)
		{
			atkCmp.attackSound.Play();
		}
	}

	public void CooldownFinish() {
		atkCooldown = false;
		SetSpriteArray("both down");
	}

	private void SetSpriteArray(string arrayName) {
		if (arrayName == "right up"){activeSpriteArray = spriteSet.RupLdown;}
		else if (arrayName == "both down"){activeSpriteArray = spriteSet.bothDown;}
	}
}
