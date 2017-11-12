using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	Text winText;
	Text timeText;

	void Start () {
		winText = GameObject.Find ("WinText").GetComponent<Text>();
		timeText = GameObject.Find ("TimeText").GetComponent<Text>();
	}

	static UIController GetInstance () {
		return GameObject.Find ("Canvas").GetComponent<UIController> ();
	}

	public static void SetWinText (string text) {
		GetInstance ().winText.text = text;
	}

	public static void SetTimeText (string text) {
		GetInstance ().timeText.text = text;
	}
}
