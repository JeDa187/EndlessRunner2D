
### **Selvitys Parallax-koodin Muutoksista:**

Kun vertaamme alkuperäistä ja uutta parallax-koodia, on selvää, että muutokset keskittyvät koodin modularisointiin, dynaamiseen nopeuden säätöön ja koodin selkeyteen. 

Alkuperäisessä koodissa kaikki toiminnallisuus oli pakattu yhteen luokkaan, kun taas uudessa koodissa käytettiin sisäistä `ParallaxLayer` luokkaa, mikä mahdollistaa joustavamman ja helpommin hallittavan koodirakenteen. Tämä ei ainoastaan tee koodista helpommin ymmärrettävää, vaan myös mahdollistaa joustavamman laajennettavuuden tulevaisuudessa.

Lisäksi koodissa otettiin käyttöön dynaaminen kiihtyvyys `accelerationFactor`, joka antaa taustoille elävämmän tunteen ja tekee liikkeestä sujuvampaa.

Koodin siisteydessä tehtiin myös parannuksia. Tarpeettomat koodin osat poistettiin, mikä tekee koodista yksinkertaisemman ja helpomman ylläpitää.

Yhteenvetona voidaan todeta, että tehdyt muutokset parantavat koodin yleistä arkkitehtuuria ja toiminnallisuutta sekä tekevät siitä helpommin ymmärrettävän ja ylläpidettävän.

--- 
## **Parallax Koodin Muutokset:**

### **1. Rakenne:**
**Alkuperäinen:**  
Käytettiin yhtä monoliittista luokkaa kaikille toiminnoille.

```csharp
public float[] LayerScrollSpeeds = new float[7];
public GameObject[] Layers = new GameObject[7];
```

**Uusi:**  
Koodi modularisoitiin käyttämällä sisäistä `ParallaxLayer` luokkaa.

```csharp
[System.Serializable]
public class ParallaxLayer
{
    public float scrollSpeed;
    public Transform parentObject;
    ...
}
public ParallaxLayer[] parallaxLayers;
```

### **2. Kiihtyvyys:**
**Alkuperäinen:**  
Staattinen nopeuden kasvu `speedIncreaseRate`.

```csharp
for (int i = 0; i < LayerScrollSpeeds.Length; i++)
{
    LayerScrollSpeeds[i] += speedIncreaseRate * Time.deltaTime;
}
```

**Uusi:**  
Dynaaminen kiihtyvyys `accelerationFactor`.

```csharp
scrollSpeed += acceleration * Time.deltaTime;
```

### **3. Kerrosten Käsittely:**
**Alkuperäinen:**  
Seitsemän erillistä kerrosta, jotka liikkuvat kameran mukana.

```csharp
Layers[i].transform.position = new Vector2(...);
```

**Uusi:**  
Kerrokset on jaettu kolmeen lapseen (`childSprites`), jotka liikkuvat ryhminä.

```csharp
for (int i = 0; i < childSprites.Length; i++)
{
    childSprites[i].position += new Vector3(...);
}
```

### **4. Koodin Siisteys:**
**Alkuperäinen:**  
Joitakin funktioita oli kommentoitu pois ja koodissa oli ylimääräisiä muuttujia.

```csharp
//public void SetLayerSpeed(...)
```

**Uusi:**  
Tarpeettomat osat poistettu ja koodi keskittyy vain olennaiseen.

```csharp
public void Initialize()
{
    ...
}
```

### **5. Kameran Liike:**
**Alkuperäinen:**  
Monimutkainen kameran liike ja kerrosten alkuperäisten sijaintien huomioiminen.

```csharp
mainCamera.position += CameraSpeed * Time.deltaTime * Vector3.right;
```

**Uusi:**  
Yksinkertaistettu kameran liike.

```csharp
mainCamera.position += cameraSpeed * Time.deltaTime * Vector3.right;
```

## **Yhteenveto:**
Muutokset tehtiin parantamaan koodin luettavuutta, ylläpidettävyyttä ja modulaarisuutta. Uusi rakenne on objektiorientoituneempi ja helpompi ymmärtää.

---
### Testaushavainnot

Kun pelaaja käytti speedboostia, tausta nopeutui odotettua enemmän joka kerta, kun boostia käytettiin. Tämä johtui siitä, että sekä kameran nopeus että taustakerrosten kiihtyvyys kasvoivat jatkuvasti.

### Ratkaisu

Jotta saavutettaisiin tasainen kiihtyvyys ilman kiihtyvyyden kiihtymistä, muutoksia tehtiin seuraavasti:

1. Poistettiin kiihtyvyyden kiihtyminen taustakerroksilta.
2. Lisättiin tasainen kiihtyvyys vain kameran nopeuteen.

### Muutokset koodissa:

1. Poistettu `acceleration`-parametri `ParallaxLayer`-luokan `Scroll`-metodista.
2. Lisätty kameraan tasainen kiihtyvyys `Update`-metodissa käyttämällä `accelerationFactor`-muuttujaa.
3. Taustakerrokset liikkuvat nyt suhteessa kameran nopeuteen ilman omaa kiihtyvyyttään.

### Lopullinen testaus

Kun muutokset tehtiin, peliä testattiin uudelleen. Havainnot:

1. Kameran ja taustan nopeus kasvaa nyt tasaisesti ajan myötä.
2. Kun pelaaja käyttää speedboostia, taustan nopeus ei enää kiihdy odotettua enemmän.
3. Pelikokemus on nyt ennustettavampi ja sopusoinnussa pelin muiden ominaisuuksien kanssa.

### Johtopäätökset

On tärkeää varmistaa, että kaikki pelin osa-alueet toimivat yhdessä sujuvasti. Kiihtyvyyden hallinta oli keskeinen osa tätä pelimekaniikkaa, ja nyt se on säädetty toimimaan optimaalisesti.