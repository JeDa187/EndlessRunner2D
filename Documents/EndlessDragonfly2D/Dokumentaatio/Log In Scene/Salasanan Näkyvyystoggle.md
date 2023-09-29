# Salasanan Näkyvyysjärjestelmä

Tämä järjestelmä on suunniteltu auttamaan käyttäjiä tarkistamaan napilla syöttämänsä salasana varmistaakseen, että he ovat syöttäneet oikean salasanan.

## Työvaiheet

### UI-Elementtien Luominen

- Luo Unity Editorissa uusi Button-komponentti ja TMP_InputField-komponentti.

### Skriptin Lisääminen

1. Luo uusi C#-skripti nimeltä PasswordVisibilityToggle.
2. Liitä se GameObjectiin, johon olet lisännyt Button-komponentin.

### Skriptin Koodaaminen

```csharp
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
            passwordInputField.inputType = TMP_InputField.InputType.AutoCorrect; 
        } 
        else 
        { 
            passwordInputField.inputType = TMP_InputField.InputType.Password; 
        } 

        passwordInputField.ForceLabelUpdate(); 
    } 
}
```
### Skriptin ja Buttonin Konfigurointi

- Liitä `passwordInputField`-muuttujaan TMP_InputField-komponentti.
- Aseta Button-komponentin OnClick()-tapahtuma kutsumaan `PasswordVisibilityToggle.TogglePasswordVisibility()`-metodia.