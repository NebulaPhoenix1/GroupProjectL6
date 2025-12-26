using UnityEngine;
using UnityEngine.UI;

public class PlayerDashAndDisplay : MonoBehaviour
{
    private Slider dashDisplay;

    private int collectedCoins;

    [SerializeField] private int meterStartValue = 0;
    [SerializeField] private int meterMinimum = 0;
    [SerializeField] private int meterMaximum = 15;
    public bool canDash = false;

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

        if (collectedCoins >= meterMaximum)
        {
            canDash = true;
        }
    }

    public void IncrementCollectedCoins()
    {
        if (collectedCoins < meterMaximum)
        {
            collectedCoins++;
        }
    }

    public void OnPlayerDash()
    {
        collectedCoins = 0;
        canDash = false;
    }
}
