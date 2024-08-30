using Godot;
using Godot.Collections;
using System;

public partial class HurtboxComponent : Area2D
{
	[Export] HealthComponent healthComp;
	[Export] public AudioStreamPlayer2D hurtSound;

	public void CheckForAttackComponents(Area2D area) {
		AttackComponent atkCmp = (AttackComponent)area;
		Attack hit = new Attack()
		{
			stream = atkCmp.hurtSoundClip,
			damage = atkCmp.damage,
			attackPosition = Transform
		};
		if((hurtSound != null) && (atkCmp.hurtSoundClip != null))
			{
				GetTree().Root.GetNode<SoundSingleton>("SoundSingleton").CreateOneShotSound(atkCmp.hurtSoundClip,Transform);
				//hurtSound.Stream = atkCmp.hurtSoundClip;
				//hurtSound.Play();
			}
		healthComp.Damage(hit);
	}
}
