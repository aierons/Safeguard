using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexManager : MonoBehaviour {

	//pollution variables
	private int pollution;

	//building variables
	private bool HasBuilding;
	private bool buildingLife;
	private int adjBuilds;
	public Text GCDText;

	//gathering variables
	private int GCoolDown;

	//factory variables
	private bool hasFactory;

	public int GetGCoolDown() {
		return GCoolDown;
	}

	public void StartGCoolDown() {
		GCoolDown = 3;
	}

	public void LowerGCoolDown() {
		GCoolDown -= 1;
	}

	// Use this for initialization
	void Start () {
		GCDText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		GCDText = (UnityEngine.UI.Text)Instantiate (GCDText);
		GCDText.text = "";
	}

	public void DisplayGCD() {
		GCDText.transform.position = transform.position;
		GCDText.text = GetGCoolDown().ToString ();
	}
}
