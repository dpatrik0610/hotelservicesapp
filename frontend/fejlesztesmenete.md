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
- A routerlink célja a SPA(SinglePageApplication) kihasználása, hiszen ha tipikus href-ekkel dolgoznánk, egy teljesen új index fájlt kéne létrehozni és értékét veszítené az SPA fogalma
- A Login fül úgyszinén egy routerlink, melyre kattintva egy gombkikapcsolási logika aktiválódik, valamint a login oldalra routol az alkalmazás
- A megoldásom a Login oldalra nem más, mint az, hogy az alkalmazás egy routeként kezeli mind a signint, mint a signupot
- Ezzel a probléma az lehet, hogy megtévesztő lehet az, ha az URL végén "Login" szó szerepel azonban a felhasználó éppen regisztrációs fázisban van, ugyanakkor visszarouteolni ugyanarra a komponensre naiv lehet, ugyanis nehéz animációkat végezni abból a célból, hogy az alkalmazás szebben és reaktívabban működjön, hiszen egy routeing alatt a legtöbb cache, valamint beállítás elveszik.
- A legvalószínűbb megoldás az lesz, hogy a böngésző URL-jét átnevezem, valamint marad a komponens egy routeként kezelése, hogy szebben lehessen kezelni a signin/signup oldal átváltását.

## Login, Sign Up
- A login doboz egyébként eleinte úgyszintén egy újrafelhasznált mások által létrehozott doboz lett volna, ugyanakkor úgy gondoltam, hogy kihasználom a Bootstrap adta lehetőségeket és használom annak az előre meghatározott media querykkel ellátott osztályait.
- Az usename input egy "primeNG-sített" input doboz, a jelszó pedig önmagában egy primeNG elem.
- Ennek a hátránya az volt, hogy különböző szélességű volt a kettő, tehát különböző méretdefiníciókat kellett használni a kettőhöz
- Ennek tervben van a javítása
- A betűtípus eleinte egy "Cedarville Cursive" betűtípus lett volna, azonban kiderült, hogy a headeren kívül az csak funkcióbeni hátrányt nyújt, hiszen csökkenti az olvashatóságot valamint a professzionalizmus hatását
- Betűszínként egy olyan színt választottam, mely minél jobban illik a háttér valamint oldal témájához, ugynakkor nem túl nyomulós, de mégis professzinális hatást kelt.
- Adtam hozzá egy erősítő árnyékolást, mely ismét a professzionalizmus hatását kelti, valamint kifejezést ad annak, hogy mi is a célja a jelenlegi route meglétének
- A login és signin buttonok folyamatosan változnak, ezek elhelyezéseit és méreteit úgyszintén bootstrappel oldottam meg, úgy, ahogy a színezéseit is.
- A PrimeNG password mező alapértelmezetten tartalmaz egy szem gombot, mely attól függően dinamikusan változik, hogy éppen kívánja-e megjeleníteni a jelszavát éppen beíró felhasználó magát a jelszót avagy sem
- Ez a mező alapértelmezetten tartalmazott volna egy segítő dialógus dobozt, melyben szerepel egy sáv, ami tartalmazza, hogy milyen erősségű a felhasználó által beadott jelszó, azonban ez a funkció értéktelen egy meglévő felhasználóval rendelkező látogató esetében, sokkal inkább a későbbiekben jön majd elő, mégpedig a regisztrációs felületen.
- A felület programtikusan tartalmazza a formot, melyet az Angular beépített ReactiveFormsModule-jával hoztam létre
- A választás oka az, hogy sokkal testreszabhatóbb és több hozzáférés van hozzá, mint egy Template alapú formkezelésnek
- A felület tartalmaz két validátort, melynek nincs más elvárása, mint az, hogy egyik box (username és password) se legyen üres, hiszen máskülönben a felhasználónak nem lesz lehetősége bejelentkezni, valamint ezen túl több validátort Informatikai Biztonági okokból sincs értelme hozzáadni
- Amennyiben a validátoroknak nem felel meg a látogató, a bejelentkezés gomb kikapcsolt állapotban van
- Amennyiben a felhasználó megérinti a két inputot, valamint nem felel meg a validátoroknak a bemenet, abban az esetben megjelenik egy kisegítő dialógus a doboz tetején a nagy Login felirat fölött, hogy a megfelelő inputot adja meg a dobozokba, ezen felül a piros keretet kapnak a hibás inputok. Ennek oka, hogy tájékoztatást kapjon a felhasználó arról, hogy miért nem tud bejelentkezni.
