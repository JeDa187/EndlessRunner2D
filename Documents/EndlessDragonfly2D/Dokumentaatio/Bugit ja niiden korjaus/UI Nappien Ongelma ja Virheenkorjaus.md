# UI Nappien Ongelma ja Virheenkorjaus 

## Ongelma 

Kun pelaaja napsauttaa hiirellä tai koskettaa näyttöä, pelihahmo hyppää. Ongelma on, että pelihahmo hyppää myös silloin, kun pelaaja napsauttaa tai koskettaa UI-elementtiä, kuten Pause- tai Settings-nappia.

## Ratkaisu 

Ratkaisu on tarkistaa, onko hiiri tai kosketus UI-elementin päällä, kun käsitellään syötteitä pelissä. Jos ne ovat, niin estetään pelihahmoa hyppäämästä.

## Ohjeet 

1. **Lisätään `UnityEngine.EventSystems` -nimiavaruus koodin alkuun**, jotta voimme käyttää `EventSystem` -luokkaa: 

```csharp
using UnityEngine;
using UnityEngine.EventSystems; 
using TMPro;
```

2. **Muokataan `HandleInput` -metodia** tarkistamaan, onko hiiri tai kosketus UI-elementin päällä. Jos se on, palauta heti metodi. Lisätään myös `IsTouchOverUI`-apumetodi kosketuksen tarkistamiseksi. 

```csharp
private void HandleInput() 
{ 
    if (EventSystem.current.IsPointerOverGameObject() || IsTouchOverUI()) 
    { 
        return; 
    } 

    if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
    { 
        rb.velocity = Vector2.up * jumpForce;
    } 
} 

private bool IsTouchOverUI() 
{ 
    if (Input.touchCount > 0) 
    { 
        Touch touch = Input.GetTouch(0);
        int touchID = touch.fingerId;
        return EventSystem.current.IsPointerOverGameObject(touchID);
    }
    return false;
}
```

## Selitys 

Kun pelaaja napsauttaa tai koskettaa näyttöä, `HandleInput`-metodi tarkistaa ensin, onko hiiri tai kosketus UI-elementin päällä. Se käyttää `EventSystem.current.IsPointerOverGameObject()` -metodia hiiren tarkistamiseen ja `IsTouchOverUI`-apumetodia kosketuksen tarkistamiseen.

Jos hiiri tai kosketus on UI-elementin päällä, `HandleInput`-metodi palautuu heti, estäen hahmoa hyppäämästä. Jos ei ole, pelihahmo hyppää.

Tämä ratkaisu estää pelihahmoa hyppäämästä, kun pelaaja napsauttaa tai koskettaa Pause- tai Settings-nappia.