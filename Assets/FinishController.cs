using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishController : MonoBehaviour {
	void OnTriggerEnter(Collider other)
	{
		Text text = GameObject.Find ("WinText").GetComponent<Text>();
		text.text = "You Win!";
	}
}
