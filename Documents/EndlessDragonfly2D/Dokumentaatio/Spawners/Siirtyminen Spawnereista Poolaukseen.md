Pelissämme käytimme aiemmin spawnereita objektien, kuten vihollisten ja esteiden, luomiseen. Tämä tarkoitti, että joka kerta kun tarvitsimme uuden objektin, loimme sen tyhjästä ja tuhosimme sen, kun emme enää tarvinneet sitä. Vaikka tämä menetelmä on yksinkertainen ja helppokäyttöinen, se ei ole tehokas, varsinkin kun pelissä on paljon objekteja, jotka luodaan ja tuhotaan jatkuvasti.

Tämä johti suorituskykyongelmiin, kuten pätkiviin taustoihin ja viiveisiin, erityisesti laitteissa, joissa on rajoitettu laskentateho. 

Siirryimme käyttämään objektipoolausjärjestelmää vastataksemme tähän haasteeseen. Poolauksen avulla voimme "kierrättää" jo luotuja objekteja sen sijaan, että loisimme ja tuhoaisimme niitä jatkuvasti. Kun objekti "tuodaan takaisin eloon", se otetaan pois poolista, ja kun se "kuolee", se palautetaan takaisin pooliin uudelleenkäytettäväksi. 

Tämä muutos vähensi huomattavasti CPU:n ja muistin kuormitusta, mikä paransi pelimme suorituskykyä ja varmisti sulavan pelikokemuksen pelaajille. Poolauksen käyttöönotto oli kriittinen askel pelimme optimoinnissa ja suorituskyvyn parantamisessa.

[[Objektipoolausjärjestelmä]]