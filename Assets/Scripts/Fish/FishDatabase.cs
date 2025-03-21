using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FishDatabase", menuName = "Fishing/Fish Database")]
public class FishDatabase : ScriptableObject
{
    public List<FishData> fishList = new List<FishData>();

    public FishData GetRandomFish()
    {
        return fishList[Random.Range(0, fishList.Count)];
    }
}
