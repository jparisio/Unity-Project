using UnityEngine;
using TMPro;

public class MissonController : MonoBehaviour
{
    public static MissonController Instance;
    public int currentLevel = 0;
    public int fishCaught = 0;
    public TextMeshProUGUI fishCaughtText;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (fishCaught >= 5)
        {
            fishCaughtText.text = "Congrats Mission Complete" ;

        }
    }

    public void UpdateFishCaught(int amount)
    {
        fishCaught += amount;
        fishCaughtText.text = "Catch 5 fish: " + fishCaught.ToString()+ "/5" ;
    }


}
