using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashAndDisplay : MonoBehaviour
{
    private Slider dashDisplay;
    [SerializeField] private GameObject sliderFill;

    private float collectedCoins = 3;
    [SerializeField] private Color nonFilledColor; //Red for when using dash
    [SerializeField] private Color filledColor = new Color (1f, (100f / 255f), 0f); //Orange for fully charged

    [SerializeField] private int meterStartValue = 3;
    [SerializeField] private int meterMinimum = 0;
    [SerializeField] private int meterMaximum = 15;
    public bool canDash = false;
    [SerializeField] private float dashDuration;

    private void Awake()
    {
        dashDisplay = GetComponent<Slider>();
    }

    private void Start()
    {
        //nonFilledColor = sliderFill.GetComponent<Image>().color;

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
            sliderFill.GetComponent<Image>().color = filledColor;
            canDash = true;
            
        }
        //Failsafe to reset color 
        else
        {
            sliderFill.GetComponent<Image>().color = nonFilledColor;
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
        StartCoroutine(DecreaseCoinCount());
        canDash = false;
        sliderFill.GetComponent<Image>().color = nonFilledColor;
    }

    private IEnumerator DecreaseCoinCount()
    {
        float timePassed = 0;

        while (timePassed < dashDuration)
        {
            timePassed += Time.deltaTime;

            collectedCoins = Mathf.Lerp(collectedCoins, 0, timePassed / dashDuration);
            yield return null;
        }
    }
}
