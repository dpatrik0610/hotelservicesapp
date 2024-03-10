# Frontend fejlesztés menete
Ennek a dokumentumnak a célja, hogy minél részletesebben leírja a
menetét, gondolatait és folyamatát a frontend alkalmazás fejlesztésének, mely
a végén a backenddel fog együttdolgozni.
## High level design
- Elsőnek a cél az volt, hogy minél kevesebb munkával minél lenyűgözőbb alkamazást alkossunk, tehát csak a backend kommunikációval küszködjünk mégsem a dizájn megalkotásával
- Későbbiekben kiderült, hogy jobban járok, ha a nagyon egyszerű dolgokat vagy magamtól megcsinálom, vagy külső forrásból szerzem a nagyobb dolgokat pedig kifejezetten magamtól alkotom meg
- Utóbbira azért került sor, mivel az erősen struktúrált elemek mint például egy header, vagy egy welcome page sajnos el fogok veszni abban, hogy milyen részt hol kell módosítani ahhoz, hogy ugyanazt az eredményt érjem el, mint amit szeretnék. Sajnos az a nagy hátránya a saját munkának, hogy nincsen megírva magától, hogy mi hova kerül majd, hanem nekem kell meghatározni low-level szinten
- Az aranyközépút tehát úgy tűnik az lesz, ha egyaránt használok külső forrásból származó template-eket az alacsony nehézségű problémákra, valamint önálló írást a nagyobb problémákra.
- Külső forrás a bootstrap és a primeNG a legtöbb esetben
## A welcome page
- A welcome page megírásához úgy kezdtem hozzá, hogy először leszedtem egy template megoldást, ami tartalmazta a legalapabb headert, valamint egy basic CSS megoldást úgy, hogy volt egy háttér, meg egy pár preview picture arra az esetre, hogy legyen egy "véleményező rész, valamint egy pár előkép a szállodáról
- Itt történt meg az első döntés, amikor bármit hozzá szerettem volna adni extraként az oldalhoz, egyszerűen teljesen más eredménye lett és sok ID volt használva a CSS-ben, ami azt eredményzte, hogy elvesztem abban, hogy hol és mit kéne hozzáadnom hová, ahhoz, hogy eredményes legyek.
- Itt született meg a döntés is, hogy aktívabban elkezdem a Bootstrap keretrendszert használni a problémákhoz, pontosabban olyanokhoz, ami azt igényli, hogy el legyenek valahova helyezve elemek valami mellé, valam közé, utána, az elejére stb...
- Szerencsére a bootstrap tartalmaz olyan szükséges header elementeket, amik  hozzájárultak később a header komponens létrehozásához
## A header
- Mint ahogy a welcome page-ben is írtam, rengeteg segítséget kaptam a bootstrap keretrendszertől ahhoz, hogy ezerszer könnyebben meg tudjam írni ezt a részét az alkalmazásnak.
- Természetesen közel sincs még kész, mire oda eljutunk az egy hosszadalmas idő lesz
- A navbar-brand eleméhez egy teljesen egyszerű cursive betűtípussal megírt "Gyobos" szót rendeltem, ami nem más mint a Dobos és a Gyenes szó merge-elése.
- A home, About, Contact fülek egy-egy routerlink, melyek különböző komponensekhez küldenek át
