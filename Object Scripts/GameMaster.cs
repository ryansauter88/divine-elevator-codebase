using Godot;
using Godot.Collections;

public partial class GameMaster : Node2D
{
	// define array of deity resource
	private Array<DeityInfo> deities = new Array<DeityInfo>();
	public DeityInfo currentDeity = new DeityInfo();
	public float roundNumber = 0f;
	public CombatStats player;
	private Item beginnerItem;
	private PackedScene shopLevel;
	private PackedScene elevatorLevel;
	private float shopAffinityAdjust = 5f;
	private float roundAffinityAdjust = 3f;

	public override void _Ready()
	{
		base._Ready();
        deities.Add(ResourceLoader.Load<DeityInfo>("res://Custom-Resources/Deities/deity_marduk.tres"));
        deities.Add(ResourceLoader.Load<DeityInfo>("res://Custom-Resources/Deities/deity_nabu.tres"));
        deities.Add(ResourceLoader.Load<DeityInfo>("res://Custom-Resources/Deities/deity_nergal.tres"));
        player = ResourceLoader.Load<CombatStats>("res://Custom-Resources/player_stats.tres");
        beginnerItem = ResourceLoader.Load<Item>("res://Custom-Resources/basic_item.tres");
        elevatorLevel = ResourceLoader.Load<PackedScene>("res://Level-Nodes/node_2d.tscn");
        shopLevel = ResourceLoader.Load<PackedScene>("res://Level-Nodes/shop.tscn");
        SelectDeity();
    }
    private void SelectDeity() {
        // pick random from array of deity resources, using affinity (from deity info) as weights
        Array<int> selector = new Array<int>();
        for (int i = 0; i < deities.Count; i++){
            float weight = 50f + 2f * deities[i].affinity;
            for (int x = 0; x < weight; x++) {
                selector.Add(i);
            }
        }
        currentDeity = deities[selector.PickRandom()];
    }
    public void LoadShopScene() {
        // similar to below, set a flag and adjust deity affinity accordingly
        GetTree().ChangeSceneToPacked(shopLevel);
    }
    public void LoadElevatorScene() {
        if (GetTree().CurrentScene.Name == "Shop") {
            if (GetTree().Root.GetNode<ShopManager>("Shop").purchaseCheck) {
                currentDeity.affinity += shopAffinityAdjust;
            } else {{currentDeity.affinity -= shopAffinityAdjust;}}
        }
        roundNumber += 1;
        SelectDeity();
        GetTree().ChangeSceneToPacked(elevatorLevel);
    }

    public void LoadTutorialScene() {
        GetTree().ChangeSceneToFile("res://Level-Nodes/tutorial.tscn");
    }

    
    public void RoundTimerAffinityAdjust(bool passedRound) {
        if (passedRound) {currentDeity.affinity += roundAffinityAdjust;}
        else {currentDeity.affinity -= roundAffinityAdjust;}
    }


    public void ResetGame() {
        // set player stats back to original
        player = new CombatStats(){
            currentItem = beginnerItem,
            money = 0,
            health = 100,
            damage = 30,
            speed = 125
        };
        // set round number back to 1
        roundNumber = 0;
        // reset deity affinities
        for (int i = 0; i < deities.Count; i++){
            deities[i].affinity = 0;
        }
    }
}
