using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserStatsService;

public class UIController : MonoBehaviour {
	public Button showStatsButton;
	public GameObject statListItemPrefab;
	public GameObject credentialsPanel;
	public GameObject statsPanel;
	public GameObject statsContent;

	Text winText;
	Text timeText;

	Text login;
	Text password;
	Toggle toggle;

	void Start () {
		winText = GameObject.Find ("WinText").GetComponent<Text>();
		timeText = GameObject.Find ("TimeText").GetComponent<Text>();

		login = GameObject.Find ("Login").GetComponentInChildren<Text>();
		password = GameObject.Find ("Password").GetComponentInChildren<Text>();
		toggle = GameObject.Find ("Toggle").GetComponent<Toggle>();

		showStatsButton.onClick.AddListener (_onShowStatsClick);
	}

	public static UIController GetInstance () {
		return GameObject.Find ("Canvas").GetComponent<UIController> ();
	}

	public static void SetWinText (string text) {
		GetInstance ().winText.text = text;
	}

	public static void SetTimeText (string text) {
		GetInstance ().timeText.text = text;
	}

	public static UserCredentialsModel GetUserCredentialsModel () {
		return GetInstance ()._getUserCredentialsModel ();
	}

	public static void ToggleCredentialsActive () {
		GetInstance ()._toggleCredentialsActive ();
	}

	public static void UpdateStatsTable (LoginResultModel model) {
		GetInstance ()._updateTable (model);
	}

	private void _updateTable (LoginResultModel model) {
		RaceResultModel[] results = model.results;
		int i = 0;

		foreach (RaceResultModel result in results) {
			GameObject statListItem = Instantiate (statListItemPrefab);

			statListItem.transform.SetParent (statsContent.transform);

			RectTransform statListItemTransform = statListItem.GetComponent<RectTransform> ();
			statListItemTransform.offsetMin = new Vector2 (0, - 20 * i++);
			statListItemTransform.offsetMax = new Vector2 (140, -20 - 20 * i++);
		}
	}

	private void _onShowStatsClick () {
		statsPanel.SetActive (!statsPanel.activeSelf);
	}

	private UserCredentialsModel _getUserCredentialsModel () {
		var loginText = login.text;
		var passwordText = password.text;
		var isNewPlayer = toggle.isOn;

		return new UserCredentialsModel {
			login = loginText,
			password = passwordText,
			isNewPlayer = isNewPlayer
		};
	}

	private void _toggleCredentialsActive () {
		credentialsPanel.SetActive (!credentialsPanel.activeSelf);
	}
}
