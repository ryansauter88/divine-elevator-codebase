using Godot;
using System;

public partial class SoundSingleton : Node2D

{
	public void CreateOneShotSound(AudioStream audioStream, Transform2D transform) 
	{

	// instantiate an object, something like: var soundObj = (TYPE)packedScene.Instantiate()
		var soundObjectScene = ResourceLoader.Load<PackedScene>("res://Game-Object-Nodes/sound_object.tscn").Instantiate() as SoundObject;
		AddChild(soundObjectScene);
		soundObjectScene.Stream = audioStream;
		soundObjectScene.Transform = transform;
		soundObjectScene.Play();



		
		//SoundObject soundObject = soundObjectScene.;
		//soundObject.Stream = audioStream;
		//soundObject.Transform = transform;
		//AddChild(soundObjectScene);
		//soundObject.Play();



	// input the information [i assume you'll know what to do here]
	// set the position: soundObj.Transform = transform;
	// add it as a child of this node: AddChild(soundObj);
}
}
