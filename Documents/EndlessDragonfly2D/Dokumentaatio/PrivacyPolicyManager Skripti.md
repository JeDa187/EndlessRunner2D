# PrivacyPolicyManager Skriptin rakentamine

![[privacypolicy.png]]
## Yleiskatsaus:

PrivacyPolicyManager skripti hallitsee käyttäjän toimintaa liittyen pelin yksityisyyskäytäntöön, erityisesti päätöksessä hyväksyä tai hylätä ehdot. Skripti tarkistaa myös käyttäjän toimintaa linkkeihin, jotka on upotettu TextMeshPro tekstikomponenttiin.

## Muuttujat:

- `acceptButton`: Tämä julkinen muuttuja viittaa painikkeeseen, jota käyttäjät klikkaavat hyväksyäkseen yksityisyyskäytännön.
- `declineButton`: Tämä julkinen muuttuja viittaa painikkeeseen, jota käyttäjät klikkaavat hylätäkseen yksityisyyskäytännön.
- `privacyPolicyText`: Viittaus TextMeshPro tekstikomponenttiin, joka näyttää yksityisyyskäytännön. Tähän on sijoitettu klikattavat linkit ulkoisiin lähteisiin, kuten PlayFab ehtoihin.

## Metodit:

1. **Start()**:
    - Alustaa skriptin kun kohtaus latautuu.
    - Liittää `OnAcceptButtonClicked` ja `OnDeclineButtonClicked` metodit vastaaviin painikkeen klikkaustapahtumiin.

2. **Update()**:
    - Kutsutaan kerran per ruudunpäivitys ja tarkistaa käyttäjän syötteen.
    - Kuuntelee hiiren klikkauksia (tarkoitettu selainpeliin).
    - Kuuntelee kosketusnäytön napautuksia (tarkoitettu mobiili-/kosketusnäytölliseen peliin).

3. **CheckForLink(Vector2 position)**:
    - Apuohjelma, joka määrittää, leikkaako käyttäjän klikkaus tai napautus linkkiä privacyPolicyText-tekstissä.
    - Jos leikkaus löytyy, se kutsuu `OnLinkClicked`-metodia relevantilla linkkitiedolla.

4. **OnLinkClicked(TMP_LinkInfo linkInfo)**:
    - Käsittelee tapahtuman, kun yksityisyyskäytännön tekstissä olevaa linkkiä klikataan.
    - Jos klikattu linkki vastaa PlayFab-ehtojen linkkiä, se avaa liittyvän URL:n verkkoselaimessa.

5. **OnAcceptButtonClicked()**:
    - Käsittelee tapahtuman, kun käyttäjä klikkaa "Hyväksy" -painiketta.
    - Asettaa pelaajan suosituksen osoittamaan, että käyttäjä on hyväksynyt yksityisyyskäytännön.
    - Lataa "LoginScene".

6. **OnDeclineButtonClicked()**:
    - Käsittelee tapahtuman, kun käyttäjä klikkaa "Hylkää" -painiketta.
    - Asettaa pelaajan suosituksen osoittamaan, että käyttäjä on hylännyt yksityisyyskäytännön ja haluaa pysyä offline-tilassa.
    - Tallentaa pelaajan suositusmuutokset.
    - Lataa "CharacterSelection" -kohtauksen.

## Toiminnallisuus:

Kohtauksen lataamisen yhteydessä skripti alustaa painikkeen klikkaustapahtumat. Jokaisessa ruudunpäivityksessä skripti kuuntelee sekä hiiren klikkauksia (selainversioille) että kosketusnäytön napautuksia (mobiiliversioille).

Jos käyttäjä klikkaa tai napauttaa linkkiä yksityisyyskäytännön tekstissä, se avaa vastaavan URL:n. Tällä hetkellä ainoa tuettu linkki on PlayFab ehtojen linkki.

Kun käyttäjä päättää hyväksyä tai hylätä yksityisyyskäytännön, skripti tallentaa tämän suosituksen. Päätöksestä riippuen skripti joko jatkaa "LoginScene" -kohtaukseen (jos hyväksytään) tai "CharacterSelection" -kohtaukseen (jos hylätään).
