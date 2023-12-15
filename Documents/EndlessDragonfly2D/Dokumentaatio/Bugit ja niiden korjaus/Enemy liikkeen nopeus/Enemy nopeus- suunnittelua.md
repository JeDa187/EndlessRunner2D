Nyt enemyn nopeus on sidottu cameran nopeuteen sekä jos pelaaja käyttää speedboostia niin se vaikuttaa enemyn nopeuteen. 

Optimaalista olisi jos enemyn nopeus olisi aina hiukan nopeampi kun parallaxin nopeus. tämän toteutus on haastava koska parallaxilla on oma kerroin nopeuteen liittyen. Enemy ei saisi koskaan olla hitaampi kuin parallax koska muuten enemy alkaa laahaamaan parallaxin perässä. näin ollen enemyn nopeus pitäisi olla sidottu parallaxin nopeuteen mutta enemyn pitäisi silti aina kulkea nopeammin kuin parallax koska jos enemy ja parallax menee samaa vauhtia niin se ei näytä hyvältä.

Toteutus: 

nyt kameran nopeus on kirjoitettu infinite parallax scriptiin. Ehkä kameran nopeuteen vaikuttavat asiat tulisi irroittaa omaan luokkaan lähtökohtaisesti, siten kameraan liittyviä asetuksia olisi helpompi hallita.

-->

nyt muutettu enemyn nopeuden määrittymään luokan sisällä. Tästä jatketaan miettimällä miten siihen lisätään kerroin mikä vastaa siitä että enemy liikkuu aina nopeammin kuin parallax

