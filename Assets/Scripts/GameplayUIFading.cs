using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIFading : MonoBehaviour
{
    [SerializeField] private GameObject[] primaryFade;
    [SerializeField] private GameObject[] secondaryFade;

    [SerializeField] private TutorialStateManager tutorialStateManager;

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
        }
    }

    private void FadeOutObjects()
    {
        for (int i = 0; i < primaryFade.Length; i++)
        {
            primaryFade[i].GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0f);
        }

        for (int i = 0; i < primaryFade.Length; i++)
        {
            secondaryFade[i].GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0f);
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

        for (int i = 0; i < primaryFade.Length; i++)
        {
            secondaryFade[i].GetComponent<Image>().CrossFadeAlpha(1f, 2f, false);
        }

        hasFadeCompleted = true;
        yield return null;
    }
}
