using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerSprites : Resource
{
	[Export] public Array<CompressedTexture2D> bothDown;
	[Export] public Array<CompressedTexture2D> bothUp;
	[Export] public Array<CompressedTexture2D> LupRdown;
	[Export] public Array<CompressedTexture2D> RupLdown;

	public string directionFacing = "front";

	public CompressedTexture2D SpriteDirection(Vector2 input, Array<CompressedTexture2D> array) {
		CompressedTexture2D newSprite = null;
		string filter = "";
		if (input.X > 0f){filter = "right";}
		else if (input.X < 0f){filter = "left";}
		else if (input.Y > 0f){filter = "front";}
		else if (input.Y < 0f){filter = "back";}

		directionFacing = filter;
		
		for (int i = 0; i < array.Count; i++) {
			if (array[i].LoadPath.Contains(filter)) {
				newSprite = array[i];
			}
		}
		return newSprite;
	}
}
