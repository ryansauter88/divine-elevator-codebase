using Godot;

[GlobalClass]
public partial class DeityInfo : Resource {
    [Export] public float affinity = 0f;
    [Export] public float healthMultiplier = 1f;
    [Export] public float damageMultiplier = 1f;
    [Export] public float speedMultiplier = 1f;
    [Export] public string name;
    [Export] public Item axe;
    [Export] public Item bow;
    [Export] public Item shield;
    [Export] public Item spear;
    [Export] public Item sword;
    [Export] public CompressedTexture2D shopBg;
    [Export] public CompressedTexture2D shopCounter;
    [Export] public CompressedTexture2D godSprite;
    [Export] public AudioStreamOggVorbis greetingAudio;
    [Export] public AudioStreamOggVorbis purchaseAudio;
    [Export] public AudioStreamOggVorbis leaveAudio;
}