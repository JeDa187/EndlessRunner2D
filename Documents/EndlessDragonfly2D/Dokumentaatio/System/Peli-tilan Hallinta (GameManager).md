# Peli-tilan Hallinta  

## Johdanto:

Tässä dokumentaatiossa esittelen prosessin, jonka avulla loin peli-tilan hallinnan. Tavoitteenani oli varmistaa, että kun sudenkorento menee ruudun ulkopuolelle, peli ilmoittaa pelaajalle pelin päättymisestä ja tarjoaa mahdollisuuden aloittaa peli uudelleen. 

## Työvaiheet:

### GameManager Luokan Luonti:

**Tavoite:** Luoda yhtenäinen järjestelmä pelin tilan hallintaan. 

**Toimenpiteet:** 

1. Loin Unityn Editorissa uuden C# scriptin nimeltä GameManager. 
2. Integroin suunnittelemani GameManager koodin scriptiin. 
3. Lisäsin GameManager scriptin yhteen GameObjectiin Unityn Editorissa. 
4. Suunnittelin ja loin pelin loppumisen ilmoituspaneelin (GameOver) ja liitin sen GameManager skriptin gameOverPanel muuttujaan. 

### Muutokset DragonflyController Luokkaan:

**Tavoite:** Muokata sudenkorennon käyttäytymistä, jotta peli lopettaa kun sudenkorento menee ruudun ulkopuolelle. 

**Toimenpiteet:** 

1. Avattuani DragonflyController koodieditorissa.
2. Integroin suunnittelemani koodin sudenkorennon ulos ruudusta menemisen tarkistukseen ja kutsuin GameManager luokan GameOver metodia. 

### "Play Again" Painikkeen Lisääminen:

**Tavoite:** Tarjota pelaajalle mahdollisuus aloittaa peli uudelleen pelin päätyttyä. 

**Toimenpiteet:** 

1. Suunnittelin ja loin "Play Again" painikkeen Unityn Editorissa ja lisäsin sen GameOver paneeliin. 
2. Liitin painikkeen OnClick tapahtuman GameManager skriptin RestartGame() metodiin. 

## Johtopäätökset:

Integroidessani peli-tilan hallinnan, saavutin toivotun lopputuloksen: pelaajalle näytettiin ilmoitus pelin päätyttyä ja hänelle tarjottiin mahdollisuus aloittaa peli uudelleen. 

## Tunnistetut Ongelmat ja Niiden Ratkaisut:

**Ongelma:** Pelin uudelleenaloituksen yhteydessä sudenkorento ei palautunut alkuperäiseen asentoonsa, eikä peli reagoinut pelaajan syötteisiin. 

**Ratkaisu:** Tein useita muutoksia, joiden avulla ratkaisin tunnistetun ongelman. Toteutin laskurin, joka informoi pelaajaa pelin alkamisesta, ja mukautin sudenkorennon käyttäytymistä ja pelin ajan hallintaa varmistaakseni sujuvan pelikokemuksen. Muutosten myötä pelimekaniikka toimi odotetulla tavalla, ja pelaajalle tarjottiin selkeä ja miellyttävä pelikokemus. 

Tämä dokumentaatio toivottavasti auttaa muita pelinkehittäjiä integroimaan peli-tilan hallinnan omiin projekteihinsa sujuvasti ja tehokkaasti.

# Refaktoroitu Koodi

Tässä dokumentissa esitellään refaktoroitu koodi, jossa on pyritty parantamaan koodin selkeyttä ja lisätty kommentteja tarkoituksenmukaisiin paikkoihin.

# DragonflyController.cs

Tämä on luokka, joka määrittelee sudenkorento-hahmon toiminnan ja liikkeen pelissä.

```csharp
using UnityEngine;

public class DragonflyController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 12f;
    private bool canMove = false;
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    private Renderer rend;

    private void Start()
    {
        // Alusta komponentit
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!canMove) return;

        // Käsittele pelaajan syöte
        HandleInput();
        // Tarkista onko hahmo ulkona ruudulta
        CheckOutOfScreenBounds();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Hyppää ylös
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void CheckOutOfScreenBounds()
    {
        Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min);
        Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max);

        if (viewportPositionMax.y < 0 || viewportPositionMin.y > 1)
        {
            // Lopeta peli jos hahmo menee ruudun ulkopuolelle
            GameManager.Instance.GameOver();
        }
    }

    public void ToggleRigidbodyMovement(bool allowMovement)
    {
        canMove = allowMovement;
        rb.isKinematic = !allowMovement;

        if (!allowMovement)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void ResetDragonfly()
    {
        gameObject.SetActive(true);
        transform.position = originalPosition;
        rb.velocity = Vector2.zero;
    }
}
```

## Metodit

- `Start`: Alustaa komponentit.
- `Update`: Kutsutaan joka ruudunpäivityksen yhteydessä ja käsittelee pelaajan syötteen ja tarkistaa, onko hahmo ruudun ulkopuolella.
- `HandleInput`: Käsittelee pelaajan syötteen ja määrää hahmon hyppäämään, kun hiiren nappia painetaan tai kosketusnäytöllä tapahtuu kosketus.
- `CheckOutOfScreenBounds`: Tarkistaa, onko hahmo mennyt ruudun ulkopuolelle, ja jos on, kutsuu `GameManager.Instance.GameOver()` lopettaakseen pelin.
- `ToggleRigidbodyMovement`: Sallii tai estää hahmon liikkeen.
- `ResetDragonfly`: Asettaa hahmon takaisin alkuperäiseen sijaintiin ja nollaa nopeuden.

# GameManager.cs


Tämä on luokka, joka hallinnoi pelin tilaa pelissä.

```csharp
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text countdownTextObject;

    private float countdownTime = 3.0f;
    public static GameManager Instance;

    private void Awake()
    {
        SetupSingleton();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        Time.timeScale = 1;
        StartCoroutine(CountdownToStart());
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator CountdownToStart()
    {
        DragonflyController dragonfly = FindObjectOfType<DragonflyController>();

        dragonfly?.ToggleRigidbodyMovement(false);

        float currentCountdown = countdownTime;

        while (currentCountdown > 0)
        {
            countdownTextObject.text = currentCountdown.ToString("0");
            yield return new WaitForSeconds(1.0f);
            currentCountdown--;
        }

        countdownTextObject.text = "";

        if (dragonfly != null)
        {
            dragonfly.ResetDragonfly();
            dragonfly.ToggleRigidbodyMovement(true);
        }
    }
}
```

## Metodit

- `Awake`: Kutsutaan, kun scripti-objekti alustetaan. Tässä metodissa kutsutaan `SetupSingleton`, joka varmistaa, että `GameManager` on singleton.
- `Start`: Kutsutaan ennen ensimmäistä päivitystä. Tässä metodissa kutsutaan `InitializeGame`, joka alustaa pelin.
- `SetupSingleton`: Varmistaa, että `GameManager` on singleton. Jos on jo olemassa `GameManager`-instanssi, tämä objekti tuhotaan.
- `InitializeGame`: Alustaa pelin asettamalla `timeScale` arvoksi 1 ja aloittaa alkulaskennan.
- `GameOver`: Asettaa `gameOverPanel` aktiiviseksi ja asettaa `timeScale` arvoksi 0, pysäyttäen pelin.
- `RestartGame`: Asettaa `timeScale` arvoksi 1 ja lataa aktiivisen kohtauksen uudelleen, käynnistäen pelin alusta.
- `CountdownToStart`: Suorittaa alkulaskennan ennen pelin alkua ja valmistelee sudenkorennon (`DragonflyController`) peliä varten.
### Huomioita

- `[SerializeField]` määritettä käytetään, jotta muuttujia voidaan asettaa Unityn editorissa, mutta ne eivät ole julkisesti saatavilla muista skripteistä.
- Koodissa on käytetty useita yksityisiä metodeja parantamaan luettavuutta ja ylläpidettävyyttä.
- Kommentteja on lisätty tarkoituksenmukaisiin paikkoihin.

Jatkotyössä voisi vielä harkita eri osien erottamista omiksi komponenteiksi tai palveluiksi, mutta yllä oleva koodi on jo selvempi ja jäsennellympi kuin alkuperäinen.

# Jatkojalostusta
Tässä vaiheessa `GameManager` -luokka käsittelee pelin tilaa ja hallinnoi pelin kulkua. Se huolehtii pelin alkuvalmisteluista, pelin loppumisesta ja uudelleenkäynnistämisestä, sekä pelin ajastamisesta ja ajastetusta alusta.

`DragonflyController` -luokka puolestaan huolehtii hahmon (tässä tapauksessa sudenkorento) liikkeestä ja toiminnasta. Se käsittelee pelaajan syötteet (hyppiminen), hahmon ulottuvuuksien tarkistamisen ja hahmon liikkeen säätelyn.

Lisäsin scripteihin englanninkielisen kommentoinnin.

## Hahmon kuolemisen päivitys 

Haluan muuttaa toimintaa niin että hahmo on pelin ulkopuolella eli kuolee (tuhoutuu) jos puolet hahmosta on mennyt rajojen yli.

### Ratkaisu

```csharp
private void CheckOutOfScreenBounds() 

{ 

    // Convert object bounds to viewport positions 

    Vector2 viewportPositionMin = Camera.main.WorldToViewportPoint(rend.bounds.min); 

    Vector2 viewportPositionMax = Camera.main.WorldToViewportPoint(rend.bounds.max); 

    // Get the height of the object in viewport coordinates 

    float objectHeightInViewport = viewportPositionMax.y - viewportPositionMin.y; 

    // If dragonfly is more than half outside the screen vertically 

    if (viewportPositionMax.y < 0.5f * objectHeightInViewport || viewportPositionMin.y > 1 - 0.5f * objectHeightInViewport) 

    { 

        GameManager.Instance.GameOver(); // End the game 

        Destroy(gameObject); // Destroy the dragonfly game object 

    } 

}
}```

Tässä muutoksessa käytämme `viewportPositionMin` ja `viewportPositionMax` -muuttujia määrittämään hahmon korkeuden näkökulmapisteessä (viewport). Tarkistamme sitten, onko yli puolet hahmosta näytön ulkopuolella, ja jos näin on, peli päättyy. `Destroy(gameObject)` -kutsu tuhoaa `gameObject` -muuttujan viittaaman peliobjektin (tässä tapauksessa sudenkorennon), ja se poistetaan pelimaailmasta. Tämä simuloi hahmon "kuolemaa", kun se menee näytön rajojen ulkopuolelle.