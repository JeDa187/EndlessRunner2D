# TURHA TYÖ -> POISTETTU!!!

---

## PlayFab Leaderboardin Integrointi Unityyn 

Ohjeet perustuvat Youtube-videoon [Youtube Video](https://www.youtube.com/watch?v=e2RXDso6fWU&t=277s)

### a. Rekisteröidy PlayFab-palveluun:

- Siirry PlayFabin verkkosivustolle.
- Luo tili.

### b. Luo uusi peli PlayFabissa:

- Kirjaudu sisään PlayFab-kojelautaan.
- Luo uusi peli.

### c. Asenna PlayFab SDK Unityyn:

- Avaa Unity Editor.
- Siirry Unity Asset Storeen.
- Etsi "PlayFab SDK".
- Lataa ja asenna SDK Unity-projektiisi.

## Luodaan Leaderboard PlayFabissa:

- Siirry PlayFab-kojelautaan ja valitse peli.
- Vasemmasta sivupalkista valitse "Leaderboards".
- Luo uusi leaderboard nimeltä "PlatformScore".
- Määritä "Reset Frequency" ja valitse, miten tulokset tallennetaan.

## Lähetä Tulokset PlayFabille Unitysta:

- Luo "PlayFabManager"-scripti Unityssa.
- Luo "SendLeaderboard"-metodi, joka ottaa pistemäärän parametrina.
- Käytä "UpdatePlayerStatisticsRequest" lähettääksesi pistemäärän.
- Testaa peliä Unityssa.

## Hae Leaderboard PlayFabista Unityssa:

- Luo "GetLeaderboard"-metodi.
- Käytä "GetLeaderboardRequest" hakeaksesi tulokset.
- Tulosta tulokset konsoliin tai näytölle.
