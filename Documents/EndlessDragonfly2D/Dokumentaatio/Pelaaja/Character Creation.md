# Hahmon Luominen ja Lento

## Vaihe 1: Hahmon Luominen

1. Halusin aloittaa puhtaalta pöydältä, joten loin uuden kohtauksen (Scene) valitsemalla "File" > "New Scene".
2. Toimitin sudenkorennon kuvan projektiin raahaamalla PNG-kuvan Assets-kansioon.
3. Raahasin sudenkorennon kuvan hierarkiaan, luoden GameObjectin Test_Character.
4. Muokkasin sudenkorennon kokoa Inspector-ikkunassa sopivaksi.

## Vaihe 2: Hahmolle Fysiikka

1. Lisäsin RigidBody2D-komponentin sudenkorentoon, tuoden sille painovoiman ja fysiikan.
2. Säädin painovoiman voimakkuutta "Gravity Scale" -arvolla, jotta se tuntui luonnolliselta.

## Vaihe 3: Lentomekaniikan Luominen

1. Loin uuden C# Scriptin ja nimesin sen "DragonflyController".
2. Avattuani scriptin, kirjoitin seuraavan koodin:
   ```csharp
   using UnityEngine;

   public class DragonflyController : MonoBehaviour
   {
       public float jumpForce = 5f;
       private Rigidbody2D rb;

       private void Start()
       {
           rb = GetComponent<Rigidbody2D>();
       }

       private void Update()
       {
           if (Input.GetMouseButtonDown(0))
           {
               rb.velocity = Vector2.up * jumpForce;
           }
       }
   }
   ```
3. Liitin scriptin sudenkorennon GameObjectiin.
4. Muokkasin "Jump Force" -arvoa Inspector-ikkunassa saadakseni halutun lentomekaniikan.

## Vaihe 4: Hahmon Testaus

1. Käynnistin pelin Unityn yläosassa olevasta "Play"-painikkeesta.
2. Klikkasin hiirellä, jotta sudenkorento lensi, ja testasin sen toimivuuden.

## Vaihe 5: Kehitys Sekä Kosketusnäytöllä Että Hiirellä

1. Halusin, että sudenkorentohahmo reagoi sekä kosketukseen mobiililaitteilla että hiiren klikkaukseen tietokoneella. Jotta tämä toimisi, tein seuraavat muutokset "DragonflyController" -scriptiin.
2. Päivitin "Update"-metodin seuraavasti:
   ```csharp
   private void Update()
   {
       if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
       {
           rb.velocity = Vector2.up * jumpForce;
       }
   }
   ```

## Vaihe 6: Rajojen Määrittely

1. Halusin muokata niin, että hahmo ei voi mennä pelirajojen ulkopuolelle. Tätä varten tein seuraavat muutokset koodiin:
   ```csharp
   using UnityEngine;

   public class DragonflyController : MonoBehaviour
   {
       public float jumpForce = 5f;
       private Rigidbody2D rb;
       private Vector2 originalPosition; // Alkuperäinen sijainti
       private Renderer rend; // Hahmon Renderer

       private void Start()
       {
           rb = GetComponent<Rigidbody2D>();
           originalPosition = transform.position; // Tallenna alkuperäinen sijainti
           rend = GetComponent<Renderer>(); // Hae Renderer
       }

       private void Update()
       {
           if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
           {
               rb.velocity = Vector2.up * jumpForce;
           }

           // Muuta maailman koordinaatit näytön koordinaatteihin
           Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
           Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

           // Tarkista, onko näytön rajojen ulkopuolella
           if (viewportPositionMax.y < 0 || viewportPositionMin.y > 1)
           {
               // Palauta alkuperäiseen sijaintiin
               transform.position = originalPosition;
               rb.velocity = Vector2.zero; // Nollaa nopeus
           }
       }
   }
   ```
   Tässä koodissa rend.bounds.min ja rend.bounds.max antavat hahmon reunat maailman koordinaateissa. Tarkistamalla nämä arvot voimme nähdä, onko hahmo kokonaan näytön rajojen ulkopuolella, ja toimia sen mukaisesti.
## Vaihe 7: Rajojen Tarkistus ja Korjaus

Tämä vaihe selittää, miten estetään hahmo menemästä pelirajojen ulkopuolelle, joka on mainittu Vaiheessa 6.

### Selitys:

1. **Rajojen Määrittely:**
   - Käytetään `Camera.main.WorldToViewportPoint` -metodia muuttamaan hahmon maailmakoordinaatit näytön koordinaateiksi.
   - `rend.bounds.min` ja `rend.bounds.max` antavat hahmon alimman ja ylimmän pisteen maailman koordinaateissa.
   
2. **Rajojen Tarkistus:**
   - Tarkistetaan, onko hahmo kokonaan näytön rajojen ulkopuolella.
   - `viewportPositionMax.y < 0` tarkoittaa, että hahmo on näytön yläreunan ulkopuolella.
   - `viewportPositionMin.y > 1` tarkoittaa, että hahmo on näytön alareunan ulkopuolella.
   
3. **Korjaus:**
   - Jos hahmo menee rajojen ulkopuolelle, sen sijainti palautetaan alkuperäiseen sijaintiin.
   - `transform.position = originalPosition` asettaa hahmon takaisin alkuperäiseen sijaintiinsa.
   - `rb.velocity = Vector2.zero` nollaa hahmon nopeuden, estäen sen liikkumisen.
   
### Tarkoitus:

- **Estää Hahmon Meneminen Rajojen Ulkopuolelle:** Tämä koodi estää hahmon menemisen näytön rajojen ulkopuolelle. Jos hahmo yrittää mennä ulos näytön alueelta, se palautetaan alkuperäiseen sijaintiinsa.
- **Parantaa Pelikokemusta:** Tämä mekaniikka parantaa pelikokemusta estämällä hahmon katoamisen ja varmistamalla, että peli toimii odotetusti.
  
Näiden toimenpiteiden avulla hahmo pysyy pelialueella.