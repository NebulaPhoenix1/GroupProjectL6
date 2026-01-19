using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIFading : MonoBehaviour
{
    private GameObject[][] fadeObjects;
    [SerializeField] private GameObject[] primaryFade;
    [SerializeField] private GameObject[] secondaryFade;
    [SerializeField] private GameObject[] tertiaryFade;

    [SerializeField] private TutorialStateManager tutorialStateManager;
    [SerializeField] private TutorialButtons tutorialButtons;

    private bool hasFadeCompleted = false;

    private void Start()
    {
        hasFadeCompleted = false;
    }

    public void StartFadeSequence()
    {
        if (!(hasFadeCompleted && tutorialStateManager.GetIsFirstTutorial()))
        {
            FadeOutObjects();
            StartCoroutine(FadePrimaryObjects());
            tutorialButtons.StartFadeOut();
        }
    }

    private void FadeOutObjects()
    {
        for (int i = 0; i < primaryFade.Length; i++)
        {
            if (primaryFade[i].GetComponent<CanvasRenderer>() == null)
            {
                primaryFade[i].GameObject().AddComponent<CanvasRenderer>(); //any gameobject accessed by this script will need to have a canvas renderer component
            }

            primaryFade[i].GetComponent<CanvasRenderer>().SetAlpha(0f);

            if (primaryFade[i].transform.childCount > 0)
            {
                GameObject[] tempArray = new GameObject[primaryFade[i].transform.childCount];

                for (int j = 0; j < tempArray.Length; j++)
                {
                    if(primaryFade[i].transform.GetChild(j).GameObject().GetComponent<CanvasRenderer>() == null)
                    {
                        primaryFade[i].transform.GetChild(j).GameObject().AddComponent<CanvasRenderer>(); //any gameobject accessed by this script will need to have a canvas renderer component
                    }
                    primaryFade[i].transform.GetChild(j).GameObject().GetComponent<CanvasRenderer>().SetAlpha(0f);
                }
            }
            Debug.Log("Primary Alpha: " + primaryFade[i].GetComponent<CanvasRenderer>().GetAlpha());
        }

        for (int i = 0; i < secondaryFade.Length; i++)
        {
            if (secondaryFade[i].GetComponent<CanvasRenderer>() == null)
            {
                secondaryFade[i].GameObject().AddComponent<CanvasRenderer>();
            }

            secondaryFade[i].GetComponent<CanvasRenderer>().SetAlpha(0f);

            if (secondaryFade[i].transform.childCount > 0)
            {
                GameObject[] tempArray = new GameObject[secondaryFade[i].transform.childCount];

                for (int j = 0; j < tempArray.Length; j++)
                {
                    if (secondaryFade[i].transform.GetChild(j).GameObject().GetComponent<CanvasRenderer>() == null)
                    {
                        secondaryFade[i].transform.GetChild(j).GameObject().AddComponent<CanvasRenderer>();
                    }
                    secondaryFade[i].transform.GetChild(j).GameObject().GetComponent<CanvasRenderer>().SetAlpha(0f);
                }
            }
            Debug.Log("Secondary Alpha: " + secondaryFade[i].GetComponent<CanvasRenderer>().GetAlpha());
        }

        for (int i = 0; i < tertiaryFade.Length; i++)
        {
            tertiaryFade[i].GetComponent<CanvasRenderer>().SetAlpha(0f);

            if (tertiaryFade[i].transform.childCount > 0)
            {
                GameObject[] tempArray = new GameObject[tertiaryFade[i].transform.childCount];

                for (int j = 0; j < tempArray.Length; j++)
                {
                    tertiaryFade[i].transform.GetChild(j).GameObject().GetComponent<CanvasRenderer>().SetAlpha(0f);
                }
            }
            Debug.Log("Tertiary Alpha: " + tertiaryFade[i].GetComponent<CanvasRenderer>().GetAlpha());
        }
    }

    private IEnumerator FadePrimaryObjects()
    {
        yield return new WaitForSeconds(6f);

        for(int i = 0; i < primaryFade.Length; i++)
        {
            primaryFade[i].GetComponent<Image>().CrossFadeAlpha(1f, 2f, false);
        }

        StartCoroutine(FadeSecondaryObjects());
    }

    private IEnumerator FadeSecondaryObjects()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < secondaryFade.Length; i++)
        {
            secondaryFade[i].GetComponent<Image>().CrossFadeAlpha(1f, 2f, false);
        }

        StartCoroutine(FadeTertiaryObjects());
    }

    private IEnumerator FadeTertiaryObjects()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < tertiaryFade.Length; i++)
        {
            tertiaryFade[i].GetComponent<Image>().CrossFadeAlpha(1f, 2f, false);
        }

        hasFadeCompleted = true;
        yield return null;
    }

    public bool GetHasFadeCompleted()
    {
        return hasFadeCompleted;
    }
}
