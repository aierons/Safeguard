using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexManager : MonoBehaviour {

	//pollution variables
	private int pollution;

	//building variables
	public Text buildingText;
	private bool HasBuilding;
	private bool buildingLife;
	private int adjBuilds;

	//public Sprite Sprite0;
	//public Sprite SpriteGCD1;
	//public Sprite SpriteGCD2;
	//public Sprite SpriteGCD3;

	//gathering variables
	public Text GCDText;
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
		GCDText = (UnityEngine.UI.Text)Instantiate (GCDText);
		GCDText.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
		GCDText.text = "";

		buildingText = (UnityEngine.UI.Text)Instantiate (buildingText);
		buildingText.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
		buildingText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void DisplayGCD() {
		Vector3 offset = new Vector3 (770, 350, 0);
		GCDText.transform.position = transform.position + offset;
		GCDText.text = GetGCoolDown().ToString ();
		/*
		if (GCoolDown == 0) {
			GetComponent<SpriteRenderer>().sprite = Sprite0; 
		}
		if (GCoolDown == 1) {
			GetComponent<SpriteRenderer>().sprite = SpriteGCD1; 
		}
		if (GCoolDown == 2) {
			GetComponent<SpriteRenderer>().sprite = SpriteGCD2; 
		}
		if (GCoolDown == 3) {
			GetComponent<SpriteRenderer>().sprite = SpriteGCD3; 
		}
		*/
	}

	public void DisplayBuilding() {
		Vector3 offset = new Vector3 (750, 325, 0);
		buildingText.transform.position = transform.position + offset;
		buildingText.text = GetGCoolDown().ToString ();
	}
}
