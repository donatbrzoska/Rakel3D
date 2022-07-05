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
 * CMD + ALT + LEFT/RIGHT: Springe zu vorheriger/nächster Curserposition
 * 
 * ALT + Mouse: Spaltenweise Mutliline-Markierung
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
 */