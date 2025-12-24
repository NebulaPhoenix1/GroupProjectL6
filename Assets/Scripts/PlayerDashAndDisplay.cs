using UnityEngine;
using UnityEngine.UI;

public class PlayerDashAndDisplay : MonoBehaviour
{
    private Slider dashDisplay;

    private int collectedCoins;

    public int meterStartValue = 0;
    public int meterMinimum = 0;
    public int meterMaximum = 15;

    private void Awake()
    {
        dashDisplay = GetComponent<Slider>();
    }

    private void Start()
    {
        dashDisplay.minValue = meterMinimum;
        dashDisplay.maxValue = meterMaximum;
        dashDisplay.value = meterStartValue;
    }

    private void Update()
    {
        if (dashDisplay)
        {
            dashDisplay.value = collectedCoins;
        }
        else
        {
            Debug.Log("Can't find display object");
        }
    }

    public void IncrementCollectedCoins()
    {
        if (collectedCoins <= meterMaximum)
        {
            collectedCoins++;
        }
    }
}
