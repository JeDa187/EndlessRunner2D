using UnityEngine;
using TMPro;

public class PasswordVisibilityToggle : MonoBehaviour
{
    public TMP_InputField passwordInputField;
    private bool passwordVisible = false;

    public void TogglePasswordVisibility()
    {
        passwordVisible = !passwordVisible;

        if (passwordVisible)
        {
            passwordInputField.inputType = TMP_InputField.InputType.Standard;
        }
        else
        {
            passwordInputField.inputType = TMP_InputField.InputType.Password;
        }

        // Pakota input-kentän tekstin päivitys
        passwordInputField.ForceLabelUpdate();
    }
}
