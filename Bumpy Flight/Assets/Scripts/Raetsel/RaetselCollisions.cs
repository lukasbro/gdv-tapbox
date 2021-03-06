﻿/* 
 * RaetselCollisions.cs
 *
 * Script für die Umsetzung der Rätsel und Hindernissen im Nebenlevel "Höhle".
 * Das Script ist Triggern der entsprechenden Rätsel / Hindernisse im Nebenlevel zugewiesen.
 *
 * Funktionen:
 *  	OnTriggerEnter(Collider col)    IEnumerator WaitAndSpawn()      score ()
 *      IEnumerator LightsAndFall()     openDoors()                     checkCorrectness()
 *      IEnumerator WorkAndWait(string name)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaetselCollisions : MonoBehaviour 
{
	private GameObject 		saeule1;			// Säule1, die Weg versperrt (falls Wall vorhanden)
	private GameObject 		saeule2;			// Säule2, die Weg versperrt (falls Wall vorhanden)
	private int[] 			riddle;				// Zufällig gefülltes Array für TouchRockRiddle
	private int[] 			order;				// orderList aus Raetsel.cs als Array
    private int             didScore;           // Sperrvariable für Score
    private Raetsel 		raetselScript;		// Script Raetsel
	private LevelManager 	levelManagerScript;	// Script LevelManager
    private GameObject      pointsText;         // Text, der bei Erfolg angezeigt wird
	
    void Awake()
    {
        raetselScript 		    = GameObject.Find("LevelManager").GetComponent<Raetsel>();
		levelManagerScript 	    = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        raetselScript.sperre    = 0;
        didScore                = 0;

        if (raetselScript.rand == 0) 
        {
            riddle = raetselScript.riddle;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        /*
	     *	Rätsel 0 TouchRockRiddle
	     *  
         *  Player muss drei Steinplatten in richtiger Reihenfolge berühren, damit sich die nachfolgende Wand öffnet.
         *  Die Reihenfolge, in der die Berührung der Steine stattfinden muss, wird zuvor zufällig in Raetsel.cs festgelegt.
	     *	Berührt der Player eine Steinplatte, ändert sich die Lampe des Steins von weiß zu gelb. Hat der Spieler alle drei Steinplatten berührt, wird geprüft ob die Reihenfolge korrekt ist.
         *  Ist die Reihenfolge inkorrekt ändern sich die Lampen von gelb zu rot und der Player muss es erneut versuchen (siehe checkCorrectness()).
         *  Bei korrekter Reihenfolge werden alle Lampen grün und es wird ein Weg durch die Wand geöffnet sowie Punkte für das erfolgreiche Absolvieren vergeben (siehe openDoors()).
 	     *	
	     *	Zugehörige Funktionen: 
         *      checkCorrectness()
         *      openDoors()
	     */
        if (raetselScript.rand == 0)
        {
            if (col.gameObject.tag == "player2" && gameObject.name == "detect1")
            {
                if (raetselScript.orderList.Count < 3 && raetselScript.set1 == 0)
                {
                    raetselScript.orderList.Add(0);
                    raetselScript.set1 = 1;
                    GameObject.Find("RedLamp1").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("WhiteLamp1").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("YellowLamp1").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
                }

                checkCorrectness();

                if (raetselScript.orderList.Count == 1)
                {
                    GameObject.Find("RedLamp2").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("RedLamp3").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("WhiteLamp2").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("WhiteLamp3").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
                }
            }

            else if (col.gameObject.tag == "player2" && gameObject.name == "detect2")
            {
                if (raetselScript.orderList.Count < 3 && raetselScript.set2 == 0)
                {
                    raetselScript.orderList.Add(1);
                    raetselScript.set2 = 1;
                    GameObject.Find("RedLamp2").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("WhiteLamp2").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("YellowLamp2").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
                }

                checkCorrectness();

                if (raetselScript.orderList.Count == 1)
                {
                    GameObject.Find("RedLamp1").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("RedLamp3").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("WhiteLamp1").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("WhiteLamp3").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
                }
            }

            else if (col.gameObject.tag == "player2" && gameObject.name == "detect3")
            {
                if (raetselScript.orderList.Count < 3 && raetselScript.set3 == 0)
                {
                    raetselScript.orderList.Add(2);
                    raetselScript.set3 = 1;
                    GameObject.Find("RedLamp3").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("WhiteLamp3").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("YellowLamp3").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
                }

                checkCorrectness();

                if (raetselScript.orderList.Count == 1)
                {
                    GameObject.Find("RedLamp1").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("RedLamp2").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("WhiteLamp1").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("WhiteLamp2").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
                }
            }
        }

        /*
	     *	Rätsel / Hindernis 1 FireInTheCave
	     *  
	     *	Player muss einen Abgrund, aus dem Flammen nach oben schlagen, über zwei nach oben und unten fahrende Steinplatten (siehe Raetsel.cs) überwinden.
	     *	Bei Berührung der Flammen (bzw. der Collision Plane) verliert der Player ein Leben und wird erneut vor dem Hindernis gespawnt (oder Game Over).
         *  Für das erfolgreiche Absolvieren werden Punkte vergeben.
	     */
		else if (raetselScript.rand == 1) 
		{
			if (col.gameObject.tag == "player2" && gameObject.name == "ColFirePlane")
            {
					levelManagerScript.RespawnPlayer();				
            }
            else if (col.gameObject.tag == "player2" && gameObject.name == "ScorePlane" && didScore == 0)
            {
                didScore = 1;
                score();
            }
		}

        /*
	     *	Rätsel / Hindernis 2 BetterRunFast
	     *  
	     *	Player muss einen Abgrund, aus dem Steinspitzen herausragen, über eine lange Steinplatte überwinden.
	     *	Bei Berührung der langen Steinplatte bricht diese (von der entsprechenden Stelle der Berührung aus) in ihre Einzelteile herunter in den Abgrund (siehe WorkAndWait(string name)).
         *  Bei Berührung der Steinspitzen (bzw. der Collision Plane) verliert der Player ein Leben und wird erneut vor dem Hindernis gespawnt (oder Game Over).
         *  Für das erfolgreiche Absolvieren werden Punkte vergeben.
 	     *	
	     *	Zugehörige Funktionen: 
	     *      WorkAndWait(string name)
	     */
		else if (raetselScript.rand == 2) 
		{
            if (col.gameObject.tag == "player2" && raetselScript.sperre == 0)
            {
                StartCoroutine(WorkAndWait(gameObject.name));
                raetselScript.sperre = 1;
		    }
            else if (col.gameObject.tag == "player2" && gameObject.name == "ColRocksPlane")
            {
                if (levelManagerScript.health > 1) 
                {
                    levelManagerScript.RespawnPlayer();
                    StartCoroutine(WaitAndSpawn());
                }
                else if (levelManagerScript.health == 1) {
                    levelManagerScript.RespawnPlayer();
                }   
            }
            else if (col.gameObject.tag == "player2" && gameObject.name == "ScorePlane" && didScore == 0)
            {
                didScore = 1;
                score();
            }
		}

        /*
	     *	Rätsel / Hindernis 3 FallingStones
	     *  
	     *	Der Player muss einen Stein in einem Abgrund berühren, damit sich die nachfolgende Wand öffnet.
	     *	Bei Berührung der Steinplatte schaltet die Lampe des Steins wie eine Ampel von grün über gelb auf rot (siehe LightsAndFall()). 
         *  Der Player muss sich nun vor von der Decke fallenden gefährlichen Steinen in Acht nehmen, während sich der Weg durch die Wand öffnet (siehe openDoors()).
         *  Bei Berührung der herunterfallenden (bzw. -gefallenen) Steinen verliert der Player ein Leben und wird erneut vor dem Hindernis gespawnt (oder Game Over).
         *  Für das erfolgreiche Absolvieren werden Punkte vergeben.
 	     *	
	     *	Zugehörige Funktionen: 
         *      LightsAndFall()
         *      openDoors()
	     */
        else if (raetselScript.rand == 3) 
		{            
            if (col.gameObject.tag == "player2" && gameObject.name == "detect1" && raetselScript.sperre == 0)
            {
                raetselScript.sperre = 1;
                StartCoroutine(LightsAndFall());
            }
            else if (col.gameObject.tag == "player2" && gameObject.tag == "rock3" && raetselScript.isHitting == 0)
            {
                raetselScript.isHitting = 1;
                if (levelManagerScript.health > 1) 
                {
                    levelManagerScript.RespawnPlayer();
                    StartCoroutine(WaitAndSpawn());
                }
                else if (levelManagerScript.health == 1) {
                    levelManagerScript.RespawnPlayer();
                } 
                raetselScript.isHitting = 0;
            }
            else if (col.gameObject.tag == "player2" && gameObject.name == "ScorePlane" && didScore == 0)
            {
                didScore = 1;
                score();
            }
        }

        /*
	     *	Rätsel / Hindernis 4 UpDownRock
	     *  
	     *	Der Player muss Steine mit gefährlichen Spitzen, die sich hoch und runter bewegen (siehe Raetsel.cs), nacheinander überwinden.
         *  Bei Berührung der Spitze eines solchen Steins verliert der Player ein Leben und wird erneut vor dem Hindernis gespawnt (oder Game Over).
         *  Für das erfolgreiche Absolvieren werden Punkte vergeben.
	     */
        else if (raetselScript.rand == 4) 
		{    
            if (col.gameObject.tag == "player2" && gameObject.name == "Trigger")
            {
                levelManagerScript.RespawnPlayer();
            }
            else if (col.gameObject.tag == "player2" && gameObject.name == "ScorePlane" && didScore == 0)
            {
                didScore = 1;
                score();
            }
        }
    }

    // Coroutine sorgt für eine kurze Verzögerung bevor das Rätsel / Hindernis zurückgesetzt wird
    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        raetselScript.spawnStonesAgain();
    }

    // Vergibt Punkte
    private void score ()
    {
        pointsText = Instantiate(Resources.Load("Punkte-Text"),
                                    GameObject.FindGameObjectWithTag("player2").transform.position,
                                    Quaternion.identity) as GameObject;
        pointsText.AddComponent<UIPoints>();
        GameObject.Find("LevelManager").GetComponent<UIController>().AddToScore(100);
        pointsText.GetComponent<UIPoints>().Points(100, GameObject.FindGameObjectWithTag("player2").transform.position);
    }

    /*
     *  Gehört zu Rätsel / Hindernis 3 FallingStones
     *	
     *	Coroutine schaltet die Lampe des Steins wie eine Ampel von grün über gelb auf rot. 
     *  Lässt zudem gefährliche Steine von der Decke fallen.
     *
     *	Zugehörige Funktionen: 
     *      openDoors()
	 */
    IEnumerator LightsAndFall()
    {
        GameObject.Find("WhiteLamp1").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("RedLamp1").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();

        yield return new WaitForSeconds(0.5f);

        GameObject.Find("RedLamp1").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("YellowLamp1").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();

        GameObject.Find("FallingStone1").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone2").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone3").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone4").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone5").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone6").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone7").GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("FallingStone8").GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(0.5f);

        GameObject.Find("YellowLamp1").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("GreenLamp1").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("Player").GetComponent<AudioFX>().bing.Play();
        GameObject.Find("Player").GetComponent<AudioFX>().stones.Play();

        openDoors();
    }

    /*
     *  Gehört zu Rätsel / Hindernis 3 FallingStones (LightsAndFall()) und Rätsel 0 TouchRockRiddle
     *	
     *	Öffnet einen Weg durch die Wand. Setzt bei Rätsel 0 die Lampen auf grün.
	 */
    void openDoors()
    {
        saeule1 = GameObject.Find("Säule1");
        saeule2 = GameObject.Find("Säule2");

        Vector3 newpos1 = saeule1.transform.position;
        newpos1.y -= 5.0f;
        saeule1.transform.position = newpos1;

        Vector3 newpos2 = saeule2.transform.position;
        newpos2.y -= 5.0f;
        saeule2.transform.position = newpos2;

        if (raetselScript.rand == 0)
        {
            GameObject.Find("Player").GetComponent<AudioFX>().solved.Play();
            score();

            GameObject.Find("YellowLamp1").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("YellowLamp2").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("YellowLamp3").GetComponent<MeshRenderer>().enabled = false;

            GameObject.Find("GreenLamp1").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("GreenLamp2").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("GreenLamp3").GetComponent<MeshRenderer>().enabled = true;

            raetselScript.sperre = 1;
        }
    }

    /*
     *  Gehört zu Rätsel 0 TouchRockRiddle
     *	
     *	Prüft ob die Reihenfolge, in der die Berührung der Steine stattfinden muss, mit der zuvor zufällig in Raetsel.cs festgelegten Reihenfolge übereinstimmt.
     *  Ist die Reihenfolge inkorrekt ändern sich die Lampen von gelb zu rot und der Player muss es erneut versuchen. 
     *  Bei korrekter Reihenfolge werden alle Lampen grün und es wird ein Weg durch die Wand geöffnet sowie Punkte für das erfolgreiche Absolvieren vergeben (siehe openDoors()).
     *	
     *  Zugehörige Funktionen: 
     *      openDoors()
	 */
    void checkCorrectness()
    {
        if (raetselScript.orderList.Count > 2)
        {
            order = raetselScript.orderList.ToArray();

            if (riddle[0] != order[0] || riddle[1] != order[1] || riddle[2] != order[2])
            {
                for (int i = 2; i >= 0; i--)
                {
                    raetselScript.orderList.RemoveAt(i);
                }

                raetselScript.set1 = 0;
                raetselScript.set2 = 0;
                raetselScript.set3 = 0;

                GameObject.Find("YellowLamp1").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("YellowLamp2").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("YellowLamp3").GetComponent<MeshRenderer>().enabled = false;

                GameObject.Find("WhiteLamp1").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("WhiteLamp2").GetComponent<MeshRenderer>().enabled = false;
                GameObject.Find("WhiteLamp3").GetComponent<MeshRenderer>().enabled = false;

                GameObject.Find("Player").GetComponent<AudioFX>().error.Play();

                GameObject.Find("RedLamp1").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("RedLamp2").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("RedLamp3").GetComponent<MeshRenderer>().enabled = true;
            }
            else if (raetselScript.sperre == 0)
            {
                openDoors();
            }
        }
    }

    /*
     *  Gehört zu Rätsel / Hindernis 2 BetterRunFast
     *	
     *	Die lange Steinplatte bricht von der Stelle der Berührung aus in ihre Einzelteile herunter in den Abgrund.
	 */
    IEnumerator WorkAndWait(string name)
    {
        GameObject.Find("Player").GetComponent<AudioFX>().stones.Play();
        yield return new WaitForSeconds(0.05f);
        if (name == "detect1")
        {   
            GameObject.Find("Flagstone1").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone2").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone3").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone4").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone5").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone6").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone7").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone8").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone9").GetComponent<Rigidbody>().isKinematic = false;
        }

        else if (name == "detect2")
        {
            GameObject.Find("Flagstone2").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone1").GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Flagstone3").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone4").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone5").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone6").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone7").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone8").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone9").GetComponent<Rigidbody>().isKinematic = false;
        }

        else if (name == "detect3")
        {
            GameObject.Find("Flagstone3").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone2").GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Flagstone4").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone1").GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Flagstone5").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone6").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone7").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone8").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone9").GetComponent<Rigidbody>().isKinematic = false;
        }

        else if (name == "detect4")
        {
            GameObject.Find("Flagstone4").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone3").GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Flagstone5").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone2").GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Flagstone6").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone1").GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Flagstone7").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone8").GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Flagstone9").GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}