# Törmäysominaisuuden lisääminen objekteille 

Tässä dokumentissa käydään läpi, kuinka lisätään törmäysominaisuus sekä liikkuvien objektien että hahmon (Dragonfly) välille käyttäen Unitya.

---
## Collider2D Komponentin Lisääminen

1. **Valmistelut:**
   - Varmista, että sekä objektit että hahmo on varustettu `Collider2D`-komponentilla.
   - Tee tämä valitsemalla halutut peliobjektit Unity-editorissa ja lisäämällä niille sopivan kokoisen ja muotoisen `Collider2D`-komponentin Inspector-paneelissa.
   
![[Näyttökuva 2023-08-16 134159.png]]

![[Näyttökuva 2023-08-16 134204.png]]

---
## Törmäysmetodin Lisääminen

2. **Koodi:**
   - Lisää `OnCollisionEnter2D`-metodi `DragonflyController`-luokkaan:
     ```csharp
     private void OnCollisionEnter2D(Collision2D collision) 
     { 
         // Tarkista, onko törmäys tapahtunut objektin kanssa, joka voi tuhota hahmon 
         if (collision.gameObject.CompareTag("Hazard")) 
         { 
             GameManager.Instance.GameOver(); 
             Destroy(gameObject); 
         } 
     }
     ```
     Tässä koodissa `OnCollisionEnter2D`-metodi käynnistyy automaattisesti, kun `Dragonfly`-objektin `Collider2D` törmää toiseen `Collider2D`-objektiin.
   - Tarkista, onko törmäys tapahtunut vaarallisen objektin kanssa tarkistamalla törmänneen objektin tagi.

---
## Vaarallisten Objektien Merkitseminen

3. **Tagin Asettaminen:**
   - Varmista, että vaaralliset objektit on merkitty `Hazard`-tagilla Unity-editorissa.
   - Tee tämä valitsemalla objekti ja asettamalla sen tagiksi `Hazard` Inspector-paneelissa.
   
---
## Lopputulos

- Tämän muutoksen jälkeen `Dragonfly`-hahmo kuolee automaattisesti, kun se törmää vaarallisiin objekteihin pelissä.