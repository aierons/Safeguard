using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupWindow : MonoBehaviour {

	public GameObject window;
	private int timeAway;

	void Start () {
		timeAway = 0;
	}

	void OnEnable() {
		timeAway = 0;
	}

	void Update () {
		bool hit = false;
		List<RaycastResult> results = GameObject.Find ("GameManager").GetComponent<GridManager> ().getRaycastResults ();
		foreach (RaycastResult result in results) {
			if (result.gameObject == this.gameObject) {
				hit = true;
				timeAway = 0;
			}
		}
		if (!hit && timeAway > 70) {
			window.SetActive (false);
		}
		++timeAway;
	}

	public void Show() {
		window.SetActive (true);
	}

	public void Hide() {
		window.SetActive (false);
	}

}
