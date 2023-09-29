# InventoryManager Null Reference Ongelma ja Ratkaisu 

## Ongelma

`InputHandling`-skriptissä oli ongelma `InventoryManager`-komponentin alustamisessa. Virheilmoitus viittasi siihen, että `InventoryManager` ei ollut löydettävissä tai sitä ei ollut alustettu oikein.

## Analyysi

`InputHandling`-skriptissä yritettiin alustaa `InventoryManager` kahteen kertaan: 

- Ensiksi yrittämällä hakea se samasta `GameObjectista` `GetComponent<InventoryManager>()`-metodilla
- Sitten `Singleton`-instanssina `InventoryManager.Instance`-kautta. 

Tämä saattoi aiheuttaa ristiriitaisuuksia tai ongelmia, jos `InventoryManager` ei ollut alustettu ennen `InputHandling`-in `Awake()`-metodin suorittamista.

## Ratkaisu

1. **Poista rivi** `inventoryManager = GetComponent<InventoryManager>();` `Awake()`-metodista.
2. **Muuta skriptien suoritusjärjestystä Unityn asetuksissa**, siten että `InventoryManager` suoritetaan ennen `InputHandling`-ia.

Nämä toimenpiteet varmistavat, että `InventoryManager` alustetaan oikein ennen `InputHandling`-in alustamista, ja ongelma ratkaistaan.

## Lisäohje

Jos `InventoryManager` on `Singleton`, varmista, että sen `Awake()`-metodia kutsutaan ennen `InputHandling`-in `Awake()`-metodia. Voit asettaa skriptien suoritusjärjestyksen Unityssä seuraavasti:

1. Mene kohtaan `Edit` > `Project Settings` > `Script Execution Order`.
2. Lisää `InventoryManager` ja `InputHandling` ja aseta niiden järjestys siten, että `InventoryManager` suoritetaan ennen `InputHandling`-ia.
