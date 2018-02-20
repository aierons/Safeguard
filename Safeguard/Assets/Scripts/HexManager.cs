using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexManager : MonoBehaviour {

	//pollution variables
	private int pollution;

	//building variables
	public Text buildingText;
	private bool hasBuilding;
	private int buildingLife;
	private int adjBuilds;

	//public Sprite Sprite0;
	//public Sprite SpriteGCD1;
	//public Sprite SpriteGCD2;
	//public Sprite SpriteGCD3;

	//gathering variables
	public Text GCDText;
	private int GCoolDown;

	public int x;
	public int y;

	public bool checkLegalMove(GameObject other) {
		HexManager otherHex = other.GetComponent<HexManager> ();
		int xa = Mathf.Abs (this.x - otherHex.x);
		int ya = Mathf.Abs (this.y - otherHex.y);
		return xa <= 1 && ya <= 1 && Mathf.Abs((otherHex.x - this.x) + (otherHex.y - this.y)) < 2;
	}

	public int GetGCoolDown() {
		return GCoolDown;
	}

	public void StartGCoolDown() {
		GCoolDown = 3;
	}

	public void LowerGCoolDown() {
		GCoolDown -= 1;
	}

	public int GetBuildingCoolDown() {
		return buildingLife;
	}

	public void StartBuildingCoolDown() {
		buildingLife = 3;
	}

	public void LowerBuildingCoolDown() {
		buildingLife -= 1;
	}

	public bool getHasBuilding() {
		return hasBuilding;
	}
		
	// Use this for initialization
	void Start () {
		GCDText = (UnityEngine.UI.Text)Instantiate (GCDText);
		GCDText.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
		GCDText.text = "";

		buildingText = (UnityEngine.UI.Text)Instantiate (buildingText);
		buildingText.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
		buildingText.text = "";

		hasBuilding = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void DisplayGCD() {
		Vector3 offset = new Vector3 (730, 300, 0);
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
		Vector3 offset = new Vector3 (700, 300, 0);
		buildingText.transform.position = transform.position + offset;
		buildingText.text = GetBuildingCoolDown().ToString ();
	}

	public void SwitchHasBuilding() {
		hasBuilding = !hasBuilding;
	}

	void OnMouseEnter() {
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		gridManager.setMouseHex (this.gameObject);
	}

	void OnMouseExit() {
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		gridManager.setMouseHex (null);
	}
}
