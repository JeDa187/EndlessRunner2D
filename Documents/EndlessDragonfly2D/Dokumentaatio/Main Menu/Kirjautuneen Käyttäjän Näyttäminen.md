# Kirjautuneen Käyttäjän Näyttäminen

Havaittuani, että pelin päävalikossa olisi tarpeellista näyttää, onko pelaaja kirjautunut sisään vai pelataanko offline-tilassa, päätin lisätä kyseisen toiminnallisuuden.

## Ratkaisu

Ratkaisin ongelman lisäämällä päävalikkoon UI-elementin, joka päivittyy automaattisesti pelaajan kirjautumistilan mukaan.

## Työvaiheet, jotka toteutin

### UI-elementin luominen Unity-editorissa

1. Loin `TextMeshProUGUI`-elementin päävalikon sceneen.
2. Nimesin sen "PlayerStatusText" ja asetin sen yläkulmaan.

### Main Menu -skriptin muokkaaminen

1. Lisäsin viittauksen "PlayerStatusText" UI-elementtiin.
2. Muokkasin `Start()`-metodia tarkistamaan, olenko kirjautunut sisään ja päivitin tekstin sen mukaan.

### Login Scene -skriptin muokkaaminen

1. Kun kirjauduin sisään, tallensin nimeni `PlayerPrefs`iin.
2. Tämän nimen hain myöhemmin päävalikossa ja näytin sen itselleni.

### UI-elementin yhdistäminen koodiin Unity-editorissa

1. Valitsin MainMenu-skriptiä sisältävän `GameObjectin`.
2. Liitin luomani `TextMeshProUGUI`-elementin skriptin "PlayerStatusText"-kohtaan Unity-editorissa.

## Lopputulokset

Kun olin suorittanut kaikki vaiheet, näin päävalikossa nimeni, kun olin kirjautunut sisään. Jos en ollut kirjautunut sisään, näin tekstin "Offline Mode", mikä kertoi minulle, että pelasin offline-tilassa.
