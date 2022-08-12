using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/* TODO
 * Mischung mit Hintergrundfarbe des Canvas?
 * 
 * GUI
 * - Rakel-Ausrichtung und Größe
 *   + "Preview"
 * - Farben
 * - Clear-Button
 * 
 * Generell was zu den Modi überlegen
 * - Nur übers Bild ziehen
 * - Farbe auf den Rakel auftragen
 *   + Menge
 *   
 * Rakel-Neigung
 * - aktuell ist die Neigung im Prinzip 0°, der Rakel liegt also immer flach auf der Leiwand auf
 * - Maske muss dann entsprechend von RakelPosition wegverschoben werden
 * 
 * Farbmenge über BumpMap / NormalMap
 * 
 * Bidirektionaler Farbaustausch
 * - Farbschichten auf Rakel
 *   - Form wie Maske im initial State
 * - Mapping von Maskenkoordinaten auf Farbschichten in ApplyMask
 * 
 * Haken am Rand beim Ziehen
 * - Berechnungen optimieren -> bringt nicht wirklich was, wahrscheinlich wird Update() einfach gar nicht oft genug aufgerufen
 * - Implementierung so anpassen, dass der Rakel "Pixel für Pixel" übers Bild gezogen wird
 *   - wird Anwendung weniger flüssig machen
 * 
 * Bugs
 * - RakelWidth 0 macht trotzdem eine Linie
 * - Wertebereiche bei InputFields nicht definierbar
 *
 * Code Quality:
 * - OilPaintEngine aufräumen
 * - RakelNormal -> Angle
 * - Tests für Rakel im initial state
     - TODO wrong usage (Apply before set values)
 * - Tests für Mocks
 * - Tests für Apply-Calls
 * - Tests für Masks mit z.B. 70° Rotation
 * - Tests für MaskApplicator CoordinateMapping:
 *   - TODO unlucky cases
 */



/* Koordinatensysteme 
 * 
 * Array-Koordinatensystem: Origin oben links und x,y vertauscht!!! (wie Rotation um 90° nach rechts)
 * Texture2D-Koordinatensystem: Origin unten links (weil 180° rotiert)
 * Screen-Koordinatensystem: Origin unten links
 * 
 * Transformationen
 * Screen -> 
 */



/* Visual Studio Tastenkombinationen
 * 
 * Q: Nach links und rechts bewegen
 * ALT: Rotieren
 *
 * Command + D/Linksklick: Springe zu Deklaration
 * CMD + CTRL + LEFT/RIGHT: Springe zu vorheriger/nächster Curserposition
 * 
 * ALT + Mouse: Spaltenweise Mutliline-Markierung
 * CTRL + ALT + Mouse: Multicurser
 */



/* Bugsources
 * 
 * 2 innere For-Schleifen
 * - die zweite modifiziert die Zählvariable der äußeren For-Schleife (i statt k) ...
 * 
 * Das ausgegebene Array ist gar nicht das Ergebnis-Array im Test, sondern ein Zwischenstand aber noch VORM Rechteck füllen.
 * Der tatsächliche Fehler war, dass ich das erwartete Ergebnis falsch definiert hatte (Maske um zwei Spalten nach links verschoben, durch vorheriges Copy-Paste)
 *
 * Vector3 kann man nicht mit == vergleichen ... (auch wenn es nicht nullable ist)
 * 
 * Copy and Paste, Controller benutzt falschen Setter und tut damit natürlich was anderes als erwartet
 * 
 * Falsche API vermutet
 * - Vector2.Angle liefert immer den kleinsten Winkel und geht damit von 0-180, nicht von 0-360
 */



/* IC Dokumentation
 * 
 * Arrange, Act, Assert
 * 
 * Erfahrungen mit TDD -> erlebte Situationen
 * - Interface vs Vererbung für Mocks
 * 
 * Test-Szenarien
 * - Cache + Test-First-Situation ...
 * - 
 * 
 * Rolle von Unit vs. Integrationstests
 * -> Integrationstests prüfen nur Aufrufe, decken also nicht alle Szenarien ab, die mit Unit-Tests behandelt werden
 */



/* Log
 * 
 * - Input:
 *   - Maus-Position in Canvas-Space (float)
 *   - Textur-Objekt(e)
 * 
 * - Output:
 *   - bemalte(s) Textur-Objekt(e)
 *   
 * - Probleme:
 *   - man weiß einfach nicht was im Code tatsächlich passiert, wenn komplexe
 *     Vorgänge hinter ein einzelnes Interface gepackt werden
 *   - Debuggen ist extrem nervig, weil jedes Frame Update aufgerufen wird und
 *     alles mögliche gemacht wird
 *       - generell ist das real world szenario schwer nachvollziehbar
 *   - es ist nicht mal klar, ob es der Model-Code ist, der nicht funktioniert,
 *     oder ob an den Input-Parametern was nicht stimmt
 * 
 * - TDD Limitierung:
 *   - Funktion mit komplexem Algorithmus
 *      - Aufteilung in verschiedene private Funktionen
 *        -> die sollte man ja theoretisch nicht wirklich testen
 *           bzw. kann man ja auch nicht, weil sie eigentlich private sind
 * 
 * 
 * 14.06.2022
 * - Baut man ein Objekt als komplettes Model oder macht man alle nötigen Objekte
 *   aus denen das bestehen würden nach außen zum Controller sichtbar?
 *   
 * - Es ist extrem schwer, sich vorher ein Interface nach außen zu überlegen
 * 
 * 
 * 16.06.2022
 * // TODO return list of coordinates instead of 2D array
 * // -> more robust tests in case of changes
 * // -> array tight fit dimension problem is gone
 * 
 * Private Methods vs Static Helper Methods
 * - Private Methods -> Klassen haben alles was sie brauchen
 * - Static Helper Methods -> Testability, Hilfe beim Implementieren / Debuggen
 * 
 * Listen von Koordinaten vergleichen: Reihenfolge der Koordinaten relevant .......
 * 
 * 
 * 23.06.2022
 * Arrays als Speicher für Daten in Koordinatensystemen benutzen
 * - bei normalem Indexing arr[x,y], werden die Daten quasi um 90° nach rechts rotiert gespeichert
 * - also das normale Koordinatensystem mit Origin unten links wird um 90° nach rotiert gedreht
 * - Nutzung auf diese Art wichtig, um CPU-Cache zu benutzen
 * Nun ist die Frage wie die Maske eigentlich aussehen soll -> Ermitteln: Wie wird sie benutzt?
 * - Es wird durch alle Zeilen des Arrays [>#<, _] iteriert und geschaut, ob die Elemente gesetzt [_, >#<] sind oder nicht
 * -> also evtl. ist es doch besser die Maske richtig rum gedreht zu speichern
 * -> Lesezugriffe sind damit auch sicher sequentiell
 * -> Was heißt das?
 *  - Bresenham muss das Array so beschreiben können, dass Origin unten links ist (Koordinaten in Bresenham entsprechend transformieren)
 *      - Mapping von realen Koordinaten auf Array-Koordinaten
 *      - hängt ab von Größe des realen Koordinatensystems und Größe des Arrays
 *  - Scanline kann wieder für jede Zeile durchgeführt werden
 * 
 * aufgehört bei:
 * - Rechteck funktioniert
 * - Rotation kommt als nächstes
 * 
 * 
 * 26.06.2022
 * Arrays aus Farben vergleichen ist anstrengend ...
 * 
 * 
 * 30.06.2022
 * - Stand der Dinge
 *  - Maske ist irgendwie noch an x-Achse gespiegelt
 *  - Zeichnen sehr rechenaufwändig
 *      - Maske cachen
 *      - Maske nur anwenden, wenn die Position sich geändert hat
 *      - notfalls andere Repräsentation wählen, damit Anwendung der Maske effizienter wird
 *      
 *      
 * 01.07.2022
 * - 
 * 
 * 
 * 04.07.2022
 * - Maske optimieren, so dass das Ergebnis eine Sparse-Repräsentation ist wodurch das Apply um ein vielfaches schneller stattfinden kann
 *   - Scanline benötigt Zeilen -> Möglichkeiten für Datenstrukturen:
 *     - Array mit so vielen Zeilen wie es y-Werte gibt und zwei Spalten (x-Anfang, x-Ende)
 *       - es muss herausgefunden werden, wie viele y-Werte es gibt (Array-Erstellung)
 *         -> scheint machbar, ansonsten List verwenden?
 *       - es muss herausgefunden werden, wie die Werte der Maske auf den TextureSpace gemappt werden können
 *          - y-Koordinaten verraten wie weit wir vom Center entfernt sind
 *     - Dictionary mit einem Key für jede Zeile, Zeilen sind Arrays mit zwei Werten
 * - evtl. is es auch sinnvoller zuerst den bidirektionalen Farbaustausch zu implementieren, damit man dann weiß
 *   in welcher Form die Maske vorliegen soll
 *   
 *   
 * 05.07.2022
 * - Neue Implementierung für die Maske
 * 1. Koordinaten der Maske im InitialState berechnen
 * 2. Koordinaten um 0,0 rotieren
 * 3. Koordinaten an Stelle verschieben
 * 4. ApplyMask macht dann was für alle Koordinaten
 * 
 * 
 * 07.07.2022
 * -> es entstehen Löcher, siehe Grid.keynote
 * - es braucht also mehr Schritte
 * -> ApplyToCanvas macht dann:
 * 1. rotiertes Rechteck ausrechnen, auf dem Farben aufgetragen werden
 * -> aligned rectangle muss effizient berechnet werden
 * (2. Für jeden Pixel dieses Rechtecks auf den Farbspeicher in initialer Rotation mappen)
 * (-> Pixelkoordinaten zurückrotieren)
 * 3. Textur an Position <Rakelposition + Pixelkoordinate aligned rectangle> entspricht dann der zurückrotiert(Pixelkoordinate aligned rectangle)
 * -> Farbe auftragen / Farbe mitnehmen
 * 
 * Es gibt also die Komponenten:
 * - RectangleFootprint(Calculator)
 * - ColorReservoir + PickupMap
 * - RectangleFootprintMapper (auf ColorReservoir), evtl. reicht hier auch eine Funktion
 * 
 * Efficient Aligned Rectangle
 * - Repräsentation als 2D Array
 *   - So viele Zeilen wie y-Koordinaten
 *   - 2 Spalten, [0] x-Anfang, [1] x-Ende
 *   - Bresenham in dieses Array wird interessant
 * -> enthaltenene Optimierungen:
 *   - Effiziente Speichernutzung (nur Anfangs- und Endkoordinaten)
 *   - Einmalige Speicherallokation (Array, keine Vektoren)
 *   - Einsparung von Rechenschritten (nur Anfangs- und Endkoordinaten bedeutet, dass das Fill auf später verschoben werden kann, um es direkt mit einem weiteren Schritt zu verbinden)
 * 
 * aufgehört bei:
 * - Zeit für Berechnung und Anwendung der Maske messen
 * - Es scheint irgendeinen Bug zu geben, weil die Maske stets neu berechnet wird
 *   - Irgendwer setzt zwischendurch die Normale auf 0,0
 *   - Nein, ganz am Anfang ist sie 0,0 und dann bleibt sie bei 0,0 und natürlich ist dann Normal nie PreviousNormal ....
 *   - Hätte man einen Test gehabt, wär das vermutlich schon aufgefallen
 *
 *
 * 08.07.2022
 * Integrationstests (Rakel) vs Unittests (BasicMaskApplicator, ...)
 * - Integrationstests:
 *   + es ist egal, wie sich die Komponenten verändern, nur das Ergebnis wird geprüft
 *   - Bei Veränderung der Komponenten muss die Zusammensetzung des Rakels in den Tests angepasst werden
 *   - Man müsste für alle Variationen von Komponenten Tests haben
 * - Unittests
 *   + Komponenten lassen sich gut entwickeln
 *   - Bei Veränderungen der Komponenten (Rakelzusammensetzung) ist nicht unbedingt garantiert, dass noch alles funktioniert
 *   
 * Möglichkeiten für neues Mask-Interface:
 * - Array mit Zeilen aus Start + Ende
 *   + wenig Speicherbedarf + noch weiter optimierbar, so dass nur noch Zahlen verwendet werden im Array
 *   + Fill Operation fällt weg
 *   - Bresenham hierdrauf wird interessant (hoffentlich überhaupt effizient implementierbar)
 * - Liste aus allen Koordinaten
 *   + relativ einfach implementierbar
 *   - Fill Operation
 *   - mehr Speicherbedarf
 *   
 *   
 * aufgehört bei:
 * - evtl. wär Liste aus Koordinaten doch besser gewesen?
 *   -> Was ist effizienter: Vergleiche für x-Anfang und x-Ende oder viele new() Aufrufe für die Vektoren?
 * - FacedUp Case funktioniert nicht
 *   - angle ausgeben, Koordinaten ausgeben
 *   
 *   
 * 09.07.2022
 * OptimizedRakel erstmal fertig, buchstäblich 100x schneller
 * 
 * Test-After hat Nachteile
 * - Test richtet sich evtl. nur nach dem was man implementiert hat, nicht nach dem was man möchte
 *   (Reapply, Recalculate Mask Beispiel)
 *   
 * Brainstorming zu Farbaustausch:
 * - zwei Reservoirs auf dem Rakel -> Color[,] für die Farbe + int[,] für die Menge
 *   - PickupReservoir
 *   - ApplicationReservoir
 * - zunächst nur eine Farbschicht auf dem Canvas
 *   - langfristig:
 *     - Schichten: Farben
 *     - Schichent: Farbmengen
 *     - Schicht: NormalMap
 *     - alles in ein Color[] Array zwischenspeichern und am Ende jeweils übertragen
 *       - könnte Performanceprobleme geben
 *       - evtl. nur modifizierte Bereiche übertragen
 * - Farbreservoir alle -> Ausfaden muss modelliert werden
 *   - HSV? RGB?
 *     - bei RGB immer auf alle Farbkanäle noch was drauf addieren könnte funktionieren
 * - Wie war das noch mal mit der Farbmischung beim Auftrag?
 *   - TODO
 * 
 * - Design
 *   - FarbReservoirs im Rakel gespeichert
 *   - Applicator
 *      - bekommt ColorExchanger
 *        - ColorExchanger
 *           - bekommt Reservoirs
 *           - macht Mapping von Texturkoordinaten auf Reservoirs
 *           - macht Farbaustausch
 *     ODER
 *     - bekommt Reservoir
 *       - mit Interface
 *          - PickupFromPixel
 *          - ApplicateToPixel
 *       - hat ColorExchanger
 * 
 * 
 * ??.07.2022
 * Es wär eigentlich gut, wenn MaskApplicator nur erstmal die Mask auf Texture anwendet mit paintReservoir
 * der eigentliche Farbaustausch sollte woanders stattfinden, damit
 * - MaskApplicator testbar bleibt
 * - die ganze Farbaustausch-Logik nicht in den MaskApplicator Tests für jedes Szenario redundant getestet wird
 * >> PaintTransferManager/Operator
 *    - FromPickupMapToCanvas() -> Loop 2
 *    - FromCanvasToPickupMap() -> Loop 1
 * >> oder das ist die Aufgabe von PaintReservoir
 *    - Color Emit(res_x, res_y)
 *    - vod Pickup(res_x, res_y, canvas_color)
 *
 * MaskToReservoirMapper und ReservoirToMaskMapper werden auch benötigt
 * - p_x, p_y MapCanvasToPickupMap(c_x, c_y, mask_position, rakel_rotation)
 * - c_x, c_y MapPickupMapToCanvas(p_x, p_y, mask_position, rakel_rotation)
 * -> Mapping in MaskApplicator oder in PaintReservoir?
 *   - sollte PaintReservoir irgendwas über eine Maske wissen? Vermutlich nicht
 *
 * Tests:
 * - PaintReservoir
 *   - Pickup
 *   - Emit
 * - MaskApplicator
 *   - Pickup und Emit müssen auf den richtigen Koordinaten aufgerufen werden
 *   -> ?? Evtl. doch noch extra Abstraktion einführen?
 *     -> Hat Funktion DoPoint() und das macht dann das Mapping + Pickup + Emit?
 *     -> Das Problem mit DoPoint ist, dass es zwei Arten von DoPoint geben muss!!
 *     -> Zwischenlayer für beide Arten von DoPoint?
 *       -> hätte den Vorteil, dass das Testing für MaskApplicator nicht komplexer wird als es jetzt ist
 *       -> Andererseits gibt es dann für den Mechanismus als ganzes keinen Test mehr, ein Redesign wäre somit stets ein Risiko
 *     -> viele Unit Tests, trotzdem ein Integration Test mit vertretbarem Aufwand?
 *       -> Allerdings wird die Logik unter MaskApplicator vermutlich noch seeehr häufig angepasst werden
 *         -> Die Wartung dieser Integrationtests wäre einfach nur anstrengend
 *
 * - Mapper
 *   - ..
 *   - ..
 *
 * Eine Frage bleibt noch: Wer setzt die Farben in die Textur, und wer holt sie von dort?
 * Applicator oder Reservoir?
 * 
 * 
 * 27.07.2022
 * Überlegtes Design nicht super sinnvoll
 * - Die Koordinatentransformationen sollte evtl. der MaskApplicator machen, weil er sowieso die MaskPosition kennt
 *   - Außerdem hört es sich nicht sinnvoll an, dass das Reservoir eine Maske und ihre Position, sowie Rotation kennt
 * 
 * Tests für neue Version vom MaskApplicator schreiben (nun mit Farb-Reservoirs) sehr anstrengend
 * -> Was soll hier tatsächlich getestet werden? Nur genau das, was der MaskApplicator tut
 * -> Aber das ist teils schwer in Isolation zu testen, weil das Ergebnis nur mit den echten Komponenten rauskommt
 *   -> Ansonsten könnte ich ja auch einfach in den ColorMixerMock schreiben, dass er die vom Test gewünschte Farbe zurückgibt
 *   -> Oder ich mocke halt alle Komponenten und prüfe die Parameter für die Aufrufe genauer
 *     -> Dann ist die Frage ob es nicht einfacher ist, gleich alles durchzurechnen
 * -> Das muss ich dann ja aber für alle TestCases machen, oder aber nur für den einzelnen Pixel und bei den anderen überlege ich mir noch was einfacheres
 *   -> aber was ist einfach genug und dennoch sinnvoll zu testen?
 *   
 * String Log für Mocks für Unit Tests
 * - Vorteile:
 *   - Reihenfolge und Anzahl der Calls kann genau geprüft werden
 * - Nachteil:
 *   - Reihenfolge muss genau bestimmt werden! Dadurch leidet bei Arrays die Lesbarkeit der Tests
 *   
 * Next Steps:
 * - DONE PaintReservoir implementieren + testen
 * - OilPaintTexture erweitern + testen
 * - PaintTransfer Integration Tests implementieren
 * - TestRakel anpassen / löschen / kopieren?
 * - Kommentare aufräumen
 * 
 * 
 * 28.07.2022
 * OilPaintTexture erweitern + testen
 * - Abstraktion hinzufügen:
 *   - OilPaintSurface mit Interface
 *     - AddPaint
 *     - GetPaint
 *   - hat einen Member CustomTexture2D mit Interface
 *     - SetPixelFast
 *     - GetPixelFast
 *     - Apply
 *   - später wird das ganze sowieso noch erweitert, um mehrere Farbschichten zu simulieren
 *     - es wird dann also ein Zwischenarray geben, auf Basis dessen eine CustomTexture2D errechnet/modifiziert/geupdated wird
 *     - die Farbschichten könnten auch ein Member von OilPaintSurface sein
 *     - CustomTexture2D wird eine Abhängigkeit von OilPaintSurface sein, da das Texture2D-Objekt einmal als Textur für das material definiert wird
 *   - Oder einfach alles in OilPaintTexture machen? Welchen Vorteil hätte ein extra Objekt nur für SetPixelFast, GetPixelFast und Apply?
 *   - Aufgaben:
 *      - Farbmischung
 *      - Speicherung von Farbschichten
 *        + Übertragung in renderbare single-layered Farbschicht
 *      - Farbinitialisierung auf weiß
 *      - Apply Passthrough
 *      
 *      - SetPixelFast, GetPixelFast -> OOB Prüfung, Indizes ausrechnen
 *   -> OOB Prüfung wäre einfacher zu testen
 *     - wenn man das von außen durch OilPaintSurface wöllte, müsste man was tun?
 *     - AddPaint und GetPaint auf ungültigen Koordinaten aufrufen aber dann müsste man auch
 *       wieder genau wissen was man für die entsprechenden Aufrufe in Texture2D erwarten muss,
 *       d.h. für die Tests der OOB Prüfung muss man Implementierungsdetails in der Farbmischung kennen
 *       
 *   - Sollte OilPaintSurface ein Texture2D Objekt bekommen oder es selbst erstellen?
 *       
 * Testing:
 * - Was spricht dagegen einen Mock vom echten Objekt erben zu lassen? Evtl. ist
 *   es dann erforderlich auch die Konstruktorparameter des echten Objekts entgegenzunehmen
 *   - Wenn man aber ein Interface benutzt, dann kann man das neue Objekt nicht um öffentliche Methoden
 *     erweitern, da das Interface diese in C# offenbar erfordern würde !!?? Nvm, habe nur den Typen falsch hingeschrieben ... (OilPaintSurface statt OilPaintSurfaceMock)
 * - Probleme mit Interfaces für Mocks:
 *   - Genutzte Attribute mit Getter/Setter der Klasse werden unbenutzbar
 *   -> stimmt nicht, einfach den Getter im Interface definieren und beides noch mal in der Klasse mit gewünschten Rechten hinschreiben
 *   - Teilweise muss Verhalten gestubbed werden, z.B. für Getter, weil das Object Under Test diese bei der Initialisierung nutzt
 *   -> evtl. ist das mit der Initialisierung im Konstruktor auch einfach schlecht gelöst
 * 
 * Next Steps:
 * - DONE OilPaintTexture erweitern + testen
 *   - GetPixelFast Tests
 *   - OilPaintSurface Tests
 * - DONE Shared Mocks in extra Ordner schieben
 * - PaintTransfer Integration Tests implementieren
 * - TestRakel anpassen / löschen / kopieren?
 * - Rakel: SetColor -> FillColor
 * - Kommentare aufräumen
 * - Volume Implementierung
 * - Canvas Snapshot Buffer
 * 
 * 
 * 29.07.2022
 * Next Steps:
 * - ==DONE Neue Komponenten für PaintTransfer edge cases
 * - PaintTransfer Integration Tests implementieren
 * - TestRakel anpassen
 * - Rakel: SetColor -> FillColor
 * - Kommentare aufräumen
 * - Volume Implementierung
 * - Canvas Snapshot Buffer
 * 
 * Testing:
 * - Mocking erlaubt zwar das isolierte Testen einer Komponente
 * - Wird diese jedoch erweitert, müssen die Mocks wieder angepasst werden
 * - Die Komponente allerdings nicht isoliert zu testen (sondern quasi Integrationstests zu machen),
 *   bedeutet wiederum auch ein ständiges Anpassen der Tests solange die Komponente erweitert wird
 *   -> denn das Endergebnis verändert sich damit ja ständig
 * 
 * 
 * 10.08.2022
 * - Testing:
 *   - Test first hilft dabei zu merken, was die bestehenden Komponenten evtl. sogar schon können
 *      - OOB GetPaint ist z.B. schon erledigt, weil FastTexture2D bei OOB bereits NO_PAINT_COLOR zurückgibt
 * 
 * Next Steps:
 * - Volumen Implementierung für Farbreservoir
 *   - UI für Rakel Farbauffüllung
 *      - Predefined Colors
 *      - Colorpicker später
 *   - Rakel: UpdateColor löschen
 *   - Rakel: Fill Reservoir implementieren
 *   - PickupReservoir: Add / Set?
 *      - Add macht mehr Sinn, weil evtl. manchmal auch nur Farbe aufgenommen und keine abgegeben wird
 *      - Aber Add macht nur dann Sinn, wenn mehrere Farbschichten unterstützt werden ...
 *        (sonst wird ja bei jedem neuen Add die Farbe überschrieben)
 * - Volumen Implementierung für OilPaintSurface <--> Farbschichten Implementierung
 * - TestRakel anpassen
 * - ? PaintTransfer TestClass löschen
 * - Kommentare aufräumen
 * - OptimizedRakel -> Rakel
 * - Alle Rakelkomponenten in einen Ordner packen
 * - Anpressdruck beim über die Leinwand ziehen
 * - Canvas Snapshot Buffer
 * - Farbe ausfaden lassen wenn nur noch wenig Volume
 * - mehrere verschiedene Farbschichten auf den Rakel auftragen können
 * 
 * Aufgehört bei:
 * - Testing:
 *   - Integrationstests sind wichtig
 *     - Rakel: UpdateLength und UpdateWidth sollten auch das Reservoir neu anlegen ...
 *     
 * Sollte man Length und Width überhaupt updaten könnnen? Evtl. sollte eher ein neuer Rakel erstellt werden
 * -> Problem hiermit ist aber, dass es evtl. beim Benutzen anstrengend ist, weil man immer beachten muss,
 *    dass nach Length und Width Update noch mal Farbe aufgetragen werden muss
 * -> Wenn man das Reservoir behalten möchte, wird dies aber insbesondere bei schon verwendetem Rakel schwer
 *    nachvollziehbar, wo welche Farbe hinübertragen wurde
 * 
 * 
 * 12.08.2022
 * Probleme sind:
 * - Es kommt zu einer schnellen Verdünnung der Farbe, evtl. geht irgendwo welche verloren
 * - Ein leerer Rakel sollte auch Farbe über die Leinwand ziehen können
 * - Aufgenommene Farbe wird noch im selben Pixel wieder abgegeben
 *   -> Reihenfolge ändern bringt nichts, denn dann würde die abgegebene Farbe noch im selben Pixel wiederaufgenommen
 *   -> Canvas Snapshot Buffer
 *     - Umsetzung bei "rotierender Pickupmap"?
 *       - O wird nach jedem Imprint geupdated
 *         - jedoch nur für alle gerade geänderten Pixel, die nicht unter der neuen Maske liegen
 *     - Alternative Idee: Während Stroke haben alle veränderten Pixel eine "TTL" und erst nach deren Ablauf kann wieder Farbe abgegeben werden
 * 
 * Next Steps:
 * - Canvas Snapshot Buffer
 * - Durch die Farbmischung bei der Abgabe aus dem Reservoir geht immer die Hälfte der Farbe verloren
 * - Bug: Bei mehreren Klicks auf entfernten Flächen wird beim vierten Mal die Farbe halbiert
 * - PickupReservoir: Add
 * - Volumen Implementierung für OilPaintSurface <--> Farbschichten Implementierung
 * - Anpressdruck beim über die Leinwand ziehen
 * - Farbe ausfaden lassen wenn nur noch wenig Volume
 * - mehrere verschiedene Farbschichten auf den Rakel auftragen können
 * - Colorpicker (https://www.youtube.com/watch?v=Ng3P_1nc8YE)
 */