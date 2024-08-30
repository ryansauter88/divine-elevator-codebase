using Godot;
using Godot.Collections;

[GlobalClass]
public partial class EnemyVariation : Resource
{
	[Export] public Array<CompressedTexture2D> body;
	[Export] public Array<CompressedTexture2D> hairs;
	[Export] public Array<CompressedTexture2D> hairsBehind;
	[Export] public Array<CompressedTexture2D> arms;
	[Export] public Array<CompressedTexture2D> outfitTops;
	[Export] public Array<CompressedTexture2D> outfitBottoms;
	
	[Export] public Array<CompressedTexture2D> eyes;
	[Export] public Array<CompressedTexture2D> eyebrows;
	[Export] public Array<CompressedTexture2D> beards;
	[Export] public Array<CompressedTexture2D> noses;
	[Export] public Array<CompressedTexture2D> mouths;

	[Export] public Array<string> tones;
	[Export] public Array<string> outfitColors;
	[Export] public Array<string> armOrientations;
	
	[Export] public AudioStreamRandomizer deathAudio;

	public Array<CompressedTexture2D> RandomEnemySpriteParts() {
		var tone = tones.PickRandom();
		var outfitColor = outfitColors.PickRandom();
		var armOrientation = armOrientations.PickRandom();

		var enemyParts = new Array<CompressedTexture2D>
		{
			SelectSprite("", hairsBehind),
			SelectSprite(tone, armOrientation, arms),
			SelectSprite(tone, body),
			SelectSprite(tone, hairs),
			SelectSprite("", mouths),
			SelectSprite("", noses),
			SelectSprite("", beards),
			SelectSprite("", eyes),
			SelectSprite("", eyebrows),
			SelectSprite(outfitColor, armOrientation, outfitTops),
			SelectSprite(outfitColor, outfitBottoms),
		};
		return enemyParts;
	}

	private CompressedTexture2D SelectSprite(string filter, Array<CompressedTexture2D> array) {
		var newArray = new  Array<CompressedTexture2D>();
		for (int i = 0; i < array.Count; i++) {
			if (array[i].LoadPath.Contains(filter)) {
				newArray.Add(array[i]);
			}
		}
		return newArray.PickRandom();
	}

	private CompressedTexture2D SelectSprite(string color, string orientation, Array<CompressedTexture2D> array) {
		var newArray = new  Array<CompressedTexture2D>();
		for (int i = 0; i < array.Count; i++) {
			if (array[i].LoadPath.Contains(color) && array[i].LoadPath.Contains(orientation)) {
				newArray.Add(array[i]);
			}
		}
		return newArray.PickRandom();
	}
}
