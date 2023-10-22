using UnityEngine;
using UnityEngine.UI;

public class AutoLoginToggle : MonoBehaviour
{
    public Toggle rememberMeToggle;
    private LoginScene loginScene;  // Viittaus LoginScene-skriptiin

    private void Awake()
    {
        rememberMeToggle.isOn = true;
        loginScene = FindObjectOfType<LoginScene>();
    }

    private void Start()
    {
        bool rememberMe = SecurePlayerPrefs.GetInt("RememberMe", 1) == 1;
        rememberMeToggle.isOn = rememberMe;
    }

    public void OnRememberMeToggleChanged()
    {
        SecurePlayerPrefs.SetInt("RememberMe", rememberMeToggle.isOn ? 1 : 0);

        // Jos Toggle on pois p‰‰lt‰, poista tallennetut kirjautumistiedot
        if (!rememberMeToggle.isOn)
        {
            loginScene.ClearSavedLoginInfo();
        }
    }
}
