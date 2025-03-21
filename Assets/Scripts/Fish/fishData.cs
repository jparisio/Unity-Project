using UnityEngine;

[System.Serializable]
public class FishData
{
    public string fishName;          // Name
    public FishRarity rarity;        // Rarity (Common, Rare, Legendary)
    public int difficulty;           // Difficulty (1-10)
    public int value;                // Value
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

// Enum for Rarity
public enum FishRarity
{
    Common,
    Uncommon,
    Rare
}
