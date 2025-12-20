using UnityEngine;
using UnityEngine.InputSystem;

//makes sure this script is executed before SettingsMenu to avoid issues with values being read prior to being correctly set
[DefaultExecutionOrder(-1)]
public class ControlSchemeManager : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction WASD;
    private InputAction ArrowKeys;
    private int currentControlScheme;

    private void Awake()
    {
        inputActions.FindActionMap("Player");
        WASD = inputActions.FindAction("Player/LeftRight (WASD)");
        ArrowKeys = inputActions.FindAction("Player/LeftRight (ArrowKeys)");
    }

    private void Start()
    {
        if(WASD == null || ArrowKeys == null)
        {
            Debug.Log("Input Action can't be found");
        }
        if (PlayerPrefs.HasKey("ControlSchemeKey"))
        {
            currentControlScheme = PlayerPrefs.GetInt("ControlSchemeKey");
            SetControlScheme();
        }
        else
        {
            currentControlScheme = 0;
            SetControlScheme();
        }
    }

    private void SetControlScheme()
    {
        switch(currentControlScheme)
        {
            case 0:
                Debug.Log("WASD Controls");
                WASD.Enable();
                ArrowKeys.Disable();
                break;
            case 1:
                Debug.Log("Arrow key controls");
                WASD.Disable();
                ArrowKeys.Enable();
                break;
        }
    }

    public void ChangeControlScheme()
    {
        if (currentControlScheme > 0 || currentControlScheme < 0)
        {
            currentControlScheme = 0;
        }
        else
        {
            currentControlScheme++;
        }
        PlayerPrefs.SetInt("ControlSchemeKey", currentControlScheme);
        SetControlScheme();
    }

    public int GetControlScheme()
    {
        return currentControlScheme;
    }

    public void ResetToDefault()
    {
        currentControlScheme = 0;
        if (PlayerPrefs.HasKey("ControlSchemeKey"))
        {
            PlayerPrefs.SetInt("ControlSchemeKey", 0);
        }
    }
}
