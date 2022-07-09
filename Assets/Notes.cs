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
 * GUI
 * - Rakel-Ausrichtung und Größe
 *   + "Preview"
 * - Farben
 * - Resolution
 * 
 * Farbmenge über BumpMap / NormalMap
 * 
 * Bidirektionaler Farbaustausch
 * - Farbschichten auf Rakel
 *   - Form wie Maske im initial State
 * - Mapping von Maskenkoordinaten auf Farbschichten in ApplyMask
 * 
 * Maske optimieren
 * - größere Rakel ziemlich langsam
 * 
 * Haken am Rand beim Ziehen
 * - Berechnungen optimieren -> bringt nicht wirklich was, wahrscheinlich wird Update() einfach gar nicht oft genug aufgerufen
 * - Implementierung so anpassen, dass der Rakel "Pixel für Pixel" übers Bild gezogen wird
 *   - wird Anwendung weniger flüssig machen
 * 
 * Tests
 * - OptimizedRakel
 * 
 * Bugs
 * - RakelWidth 0 macht trotzdem eine Linie
 * - Wertebereiche bei InputFields nicht definierbar
 *
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
 * 14.06.2022
 * - Baut man ein Objekt als komplettes Model oder macht man alle nötigen Objekte
 *   aus denen das bestehen würden nach außen zum Controller sichtbar?
 *   
 * - Es ist extrem schwer, sich vorher ein Interface nach außen zu überlegen
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
 * 26.06.2022
 * Arrays aus Farben vergleichen ist anstrengend ...
 * 
 * 30.06.2022
 * - Stand der Dinge
 *  - Maske ist irgendwie noch an x-Achse gespiegelt
 *  - Zeichnen sehr rechenaufwändig
 *      - Maske cachen
 *      - Maske nur anwenden, wenn die Position sich geändert hat
 *      - notfalls andere Repräsentation wählen, damit Anwendung der Maske effizienter wird
 *      
 * 01.07.2022
 * - 
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
 * 05.07.2022
 * - Neue Implementierung für die Maske
 * 1. Koordinaten der Maske im InitialState berechnen
 * 2. Koordinaten um 0,0 rotieren
 * 3. Koordinaten an Stelle verschieben
 * 4. ApplyMask macht dann was für alle Koordinaten
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
 * 09.07.2022
 * Test After hat Nachteile
 * - Test richtet sich evtl. nur nach dem was man implementiert hat, nicht nach dem was man möchte
 *   (Reapply, Recalculate Mask Beispiel)
 */