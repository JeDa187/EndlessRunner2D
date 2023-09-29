## SecurePlayerPrefs Toiminnallisuuden Integroiminen Peliimme 

### Johdanto 

Huolista pelitietoturvan suhteen ja varmistaaksemme, etteivät pelaajan scoret ja asetukset ole helposti muokattavissa, päätin integroida SecurePlayerPrefs -järjestelmän. Tämä järjestelmä käyttää salauksen ja purkamisen metodeja turvatakseen pelaajan scoren ja asetusten tallentamisen. 

### 1. SecurePlayerPrefs Luokan Luominen 

Ensin suunnittelin erillisen luokan nimeltä SecurePlayerPrefs. Tämä luokka tarjoaa salaus- ja purkamismetodit käyttäen TripleDES- ja MD5-hashausta, tarjoten turvallisen tavan tallentaa pelaajan pisteet ja asetukset. 

### 2. Salaisen Avaimen Asettaminen 

SecurePlayerPrefs-luokassa on secretKey-muuttuja. Varmistin, että annoin sillä pelillemme ainutlaatuisen arvon, joka takaa tietojemme salauksen turvallisuuden. 

### 3. Vakiintuneiden PlayerPrefs-Kutsujen Korvaaminen 

Seuraavaksi kävin läpi pelikoodimme etsien kohtia, joissa käytimme Unityn sisäänrakennettua PlayerPrefsia. Jokaista esiintymää varten korvasin: 

- `PlayerPrefs.SetString(key, value)` koodin `SecurePlayerPrefs.SetString(key, value)` -koodilla. 
- `PlayerPrefs.GetString(key, defaultValue)` koodin `SecurePlayerPrefs.GetString(key, defaultValue)` -koodilla. 

Ja niin edelleen muille datatyypeille, kuten kokonaisluvuille. 

Koodin tarkastelun yhteydessä huomattiin, että SecurePlayerPrefs-luokasta puuttui joitakin kriittisiä metodeja, jotka ovat meillä käytössä ja tarpeellisia pelin toiminnan kannalta. 

### DeleteKey-metodi 

Ensimmäinen puuttuva metodi oli DeleteKey, joka mahdollistaa avaimen poiston PlayerPrefs-tietokannasta. Tämä on erityisen tärkeää, kun haluamme poistaa vanhentuneet tai tarpeettomat tiedot. Tässä on toteutus: 

```csharp
public static void DeleteKey(string key)
{
    if (PlayerPrefs.HasKey(key))
    {
        PlayerPrefs.DeleteKey(key);
    }
}
```

Tämä metodi tarkistaa ensin, onko avain olemassa, ja jos on, se poistaa sen. Koska olemme salanneet avaimen arvon, mutta itse avainta ei ole salattu, voimme käyttää perus `PlayerPrefs.DeleteKey`-metodia. 

### Save-metodi 

Toinen puuttuva metodi oli Save. Vaikka Unityn sisäänrakennettu PlayerPrefs tallentaa muutokset automaattisesti tietyissä tilanteissa, voi olla tilanteita, joissa haluamme varmistaa, että tiedot tallennetaan heti. Tässä on toteutus: 

```csharp
public static void Save()
{
    PlayerPrefs.Save();
}
```

Lisäämällä nämä kaksi metodia SecurePlayerPrefs-luokkaan, varmistimme, että meillä on tarkoitukseemme täydellinen ja turvallinen työkalu pelaajatietojen käsittelyyn. Se ei ainoastaan tarjoa salattuja arvoja, vaan myös kattavan sarjan toimintoja PlayerPrefs-toimintojen hallintaan. 


### 4. Salauksen ja Salauksen Purkamisen Osat

Seuraavaksi on esitelty SecurePlayerPrefs-luokan salauksen ja salauksen purkamisen osat, jotka ovat kriittisiä turvallisen tietojen tallennuksen kannalta.

#### Salauksen Osa:

```csharp
private static string Encrypt(string toEncrypt, string key)
{
    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
    byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
    tdes.Key = keyArray;
    tdes.Mode = CipherMode.ECB;
    tdes.Padding = PaddingMode.PKCS7;
    ICryptoTransform cTransform = tdes.CreateEncryptor();
    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
}
```

#### Salauksen Purkamisen Osa:

```csharp
private static string Decrypt(string toDecrypt, string key)
{
    byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
    byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
    tdes.Key = keyArray;
    tdes.Mode = CipherMode.ECB;
    tdes.Padding = PaddingMode.PKCS7;
    ICryptoTransform cTransform = tdes.CreateDecryptor();
    byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
    return UTF8Encoding.UTF8.GetString(resultArray);
}
```

Nämä osat varmistavat, että pelimme pelaajatiedot ovat suojattuja ja turvallisia, suojaten pelaajien yksityisyyttä ja pelikokemusta.

### 5. Testaus 

Integroinnin jälkeen testasin eri skenaarioita pelissä: 

- Asetusten tallentaminen ja lataaminen. 
- Ennätyspisteiden asettaminen ja niiden noutaminen. 
- Pelin konfiguraatioiden muuttaminen ja tarkistaminen, säilyvätkö ne istuntojen välillä. 

Tämä varmisti, että salaus- ja purkuprosessit toimivat saumattomasti eivätkä aiheuta huomattavaa viivettä pelissä.
