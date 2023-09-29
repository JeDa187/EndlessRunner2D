# 2D Sateen Hiukkasefektin Luonti Unityssa

Tässä kerron, kuinka loin 2D sateen hiukkasefektin käyttäen tätä videotutoriaalia [EASY 2D Rain Particle in Unity (Particle System Tutorial)](https://www.youtube.com/watch?v=k1kGVmS-bJQ) ja tein siitä prefabin myöhempää käyttöä varten.

## 1. Hiukkassysteemin luominen

Minä aloitin luomalla uuden hiukkassysteemin Unityssa. Hierarkiassa, valitsin "Effects" ja sitten "Particle System". Nimesin luomani partikkelisysteemin "Rain" ja nollasin sen sijainnin.

## 2. Hiukkassysteemin asetukset

Seuraavaksi ryhdyin määrittämään partikkelisysteemin asetuksia. Asetin "Start Speed" arvoksi 0, jotta hiukkaset eivät liikkuisi alussa. Asetin "Start Size" arvoksi 1. Halusin, että sade putoaa, joten asetin "Velocity over Lifetime" arvot -15 ja -20 Y-akselille.

## 3. Muoto ja koko

Määritin partikkelisysteemin muodon valitsemalla "Shape" ja asettamalla sen arvoksi "Box". Säädin X-akselin kokoa, jotta se kattaisi koko pelinäytön. Halusin, että sadepisarat olisivat erikokoisia, joten käytin "Size over Lifetime" -asetusta. Valitsin "Separate Axes" ja asetin X-akselille arvot 0.01 ja 0.03 sekä Y-akselille arvot 0.2 ja 0.4.

## 4. Emissio ja materiaali

Halusin sateen olevan tiheä, joten asetin "Emission" arvoksi 200. Seuraavaksi loin uuden materiaalin Unityssa. Asetin sen tyypiksi "Particles" ja "Standard Unlit". Valitsin "Fade" sekoitusmoodin ja lisäsin luomani materiaalin "Rain" partikkelisysteemin.

## 5. Renderöinti ja sijainti

Lopuksi säädin pisaroiden sijaintia "Sorting Layer" -asetuksella, jotta ne näkyisivät oikein suhteessa muihin spriteihin pelissäni.

## 6. Prefabin luominen

Kun olin tyytyväinen sateen partikkelisysteemiin, päätin tehdä siitä prefabin. Valitsin "Rain" partikkelisysteemin hierarkiassa ja vedin sen prefabs kansioon, jolloin siitä tuli prefab. Nyt voin käyttää tätä prefabia ilman, että minun tarvitsee luoda sateen efektiä uudelleen.

## Yhteenveto

Näin loin 2D sateen partikkelisysteemin Unityssa ja tein siitä prefabin. Tämä efekti on nyt valmis integroitavaksi mihin tahansa ja sitä voidaan mukauttaa tarpeen mukaan.
