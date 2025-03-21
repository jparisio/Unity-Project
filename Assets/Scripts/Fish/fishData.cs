using UnityEngine;

[System.Serializable]
public class FishData
{
    public string fishName;          // Name of the fish
    public FishRarity rarity;        // Rarity type (Common, Rare, Legendary)
    public int difficulty;           // How hard it is to catch (e.g., 1-10)
    public int value;                // The selling price of the fish
    public GameObject fishPrefab;    // Model reference (prefab)

    public FishData(string name, FishRarity rarity, int difficulty, int value, GameObject fishPrefab)
    {
        this.fishName = name;
        this.rarity = rarity;
        this.difficulty = difficulty;
        this.value = value;
        this.fishPrefab = fishPrefab;
    }
}

// Enum for Rarity Levels
public enum FishRarity
{
    Common,
    Uncommon,
    Rare
}
