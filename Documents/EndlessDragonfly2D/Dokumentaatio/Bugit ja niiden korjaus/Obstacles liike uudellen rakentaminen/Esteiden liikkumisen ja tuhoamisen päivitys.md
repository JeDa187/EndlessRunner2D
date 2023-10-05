
**Tausta**:
Projektissamme oli `ObstacleMovement` niminen skripti, joka vastasi esteiden liikkumisesta sekä niiden tuhoamisesta, kun ne saavuttivat tietyn sijainnin. Halusimme tehdä joitakin optimointeja ja muutoksia siihen, miten esteet käyttäytyvät pelissä. Tämän vuoksi päätettiin erottaa liikkumiseen ja tuhoamiseen liittyvät toiminnot kahteen erilliseen komponenttiin.

**Tavoite**:
Jakaa `ObstacleMovement`-skriptin toiminnallisuus siten, että:
1. Liikkumiseen liittyvät osat poistetaan.
2. Tuhoamiseen liittyvä toiminnallisuus säilytetään.

**Työvaiheet**:

1. **Skriptin analysointi**:
   Alkuperäinen `ObstacleMovement` näytti seuraavalta:
```csharp
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private float speed;
    private float destructionXPosition;

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < destructionXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDestructionXPosition(float position)
    {
        destructionXPosition = position;
    }
}
```

2. **Liikkumiseen liittyvien osien poisto**:
   
Poistettu koodi:
```csharp
private float speed;

public void SetSpeed(float newSpeed)
{
    speed = newSpeed;
}

transform.Translate(Vector2.left * speed * Time.deltaTime);
```

3. **Skriptin nimen muuttaminen**:
   Koska skripti ei enää käsitellyt liikkumista, päätin nimetä sen uudelleen kuvaavammin. Nimi muutettiin `ObstacleDestruction`-nimiseksi ja päivitetty koodi näyttää seuraavalta:
```csharp
using UnityEngine;

public class ObstacleDestruction : MonoBehaviour
{
    private float destructionXPosition;

    private void Update()
    {
        if (transform.position.x < destructionXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestructionXPosition(float position)
    {
        destructionXPosition = position;
    }
}
```

**Johtopäätökset**:
Skriptin jakaminen kahteen osaan tekee siitä modulaarisemman ja helpomman ylläpitää.