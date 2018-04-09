using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour {

	public GameObject window;
	//public Text message;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Show() {
	//	message.text = "buildingrecipe";
		window.SetActive (true);
	}

	public void Hide() {
		window.SetActive (false);
	}


}
