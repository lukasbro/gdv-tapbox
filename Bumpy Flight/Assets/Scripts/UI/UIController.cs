﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text 		scoreText;		// Der Text, in dem der Score angezeigt werden soll
	public int 			score;			// Der aktuelle Score
	public GameObject	menuScreen;		// Das Empty des Menüs

	// Use this for initialization
	void Start () {
		scoreText.text 	= "0";
		score 			= -12;
	}

	/*
	*	Fügt den Wert val dem aktuellen Score hinzu
	*
	*	@val: Wert, der hinzugefügt werden soll
	 */
	public void AddToScore( int val ) {
		score += val;
		scoreText.text = score.ToString();
	}

	public void GameOn() {
		Time.timeScale = 1f;
		Cursor.visible = false;
		menuScreen.SetActive(false);
	}
}
