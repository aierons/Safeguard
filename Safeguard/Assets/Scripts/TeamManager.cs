using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour {

	private Player Jayson;
	private Player Zoya;
	private Player Mariana;

	public Text bankText; 
	public Text actText;

	public int replacedFactories;

	public Button gatherButton;
	public Button buildButton;
	public Button moveButton;
	public Button cleanButton;
	public Button endTurnButton;

	public int ore; 
	public int wood;


	// Use this for initialization
	void Start () {

		Jayson = GameObject.FindGameObjectWithTag ("Jayson").GetComponent<Player> ();
		Zoya = GameObject.FindGameObjectWithTag ("Zoya").GetComponent<Player> ();
		Mariana = GameObject.FindGameObjectWithTag ("Mariana").GetComponent<Player>();
		ore = 0;
		wood = 0;


		bankText.text = "";

		gatherButton.onClick.AddListener (Gather);
		buildButton.onClick.AddListener (Build);
		moveButton.onClick.AddListener (Move);
		cleanButton.onClick.AddListener (Clean);
		endTurnButton.onClick.AddListener (EndTurn);

		replacedFactories = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		bankText.text = "Ore: " + ore.ToString() + "\nWood: " + wood.ToString();

		if (getActivePlayer ()) {
			actText.text = "Active Character:" + getActivePlayer ().tag + "\nAction Count: " + getActivePlayer ().getActionCount ().ToString ()
			+ "\nMovement Left:" + getActivePlayer ().movement.ToString ();
		}
		else {
			actText.text = "No Active Character";
			bankText.text = "Ore: " + ore.ToString() + "\nWood: " + wood.ToString();
		}
	}
		

	Player getActivePlayer() {
		if (Jayson.isActive ()) {
			return Jayson;
		} else if (Zoya.isActive ()) {
			return Zoya;
		} else if (Mariana.isActive ()) {
			return Mariana;
		} else {
			return null;
		}
	}

	void Gather() {
		getActivePlayer ().Gather ();
	}

	void Build() {
		getActivePlayer ().Build ();
	}

	void Move() {
		getActivePlayer ().Move ();
	}

	void Clean() {
		getActivePlayer ().Clean ();
	}

	void EndTurn() {
		Jayson.EndTurn ();
		Zoya.EndTurn ();
		Mariana.EndTurn ();

		GameObject gm = GameObject.Find ("GameManager");
		IList<GameObject> grid = gm.GetComponent<GridManager> ().getGrid ();
		foreach(GameObject hex in grid) {
			if (hex != null) {
				if (hex.GetComponent<HexManager> ().GetGCoolDown () > 0) {
					hex.GetComponent<HexManager> ().LowerGCoolDown ();
					hex.GetComponent<HexManager> ().DisplayGCD ();
				}

				if (hex.GetComponent<HexManager> ().GetBuildingCoolDown () > 0) {
					hex.GetComponent<HexManager> ().LowerBuildingCoolDown ();
					hex.GetComponent<HexManager> ().DisplayBuilding ();
				}
				if (hex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0
					&& hex.GetComponent<HexManager> ().getHasBuilding ()) {
					//building.SetActive (false);
					hex.GetComponent<HexManager> ().RemoveBuilding();
					hex.GetComponent<HexManager> ().SwitchHasBuilding ();

					GridManager gridManager = gm.GetComponent<GridManager> ();
					int fx = hex.GetComponent<HexManager>().x - gridManager.gridSideLength + 1;
					int fy = hex.GetComponent<HexManager>().y - gridManager.gridSideLength + 1;
					GameObject n1 = gridManager.getHex (fx + 1, fy);
					GameObject n2 = gridManager.getHex (fx, fy + 1);
					GameObject n3 = gridManager.getHex (fx - 1, fy);
					GameObject n4 = gridManager.getHex (fx, fy - 1);
					GameObject n5 = gridManager.getHex (fx - 1, fy + 1);
					GameObject n6 = gridManager.getHex (fx + 1, fy - 1);
					if (n1 != null) {
						n1.GetComponent<HexManager> ().decrementBuilding ();
					}
					if (n2 != null) {
						n2.GetComponent<HexManager> ().decrementBuilding ();
					}
					if (n3 != null) {
						n3.GetComponent<HexManager> ().decrementBuilding ();
					}
					if (n4 != null) {
						n4.GetComponent<HexManager> ().decrementBuilding ();
					}
					if (n5 != null) {
						n5.GetComponent<HexManager> ().decrementBuilding ();
					}
					if (n6 != null) {
						n6.GetComponent<HexManager> ().decrementBuilding ();
					}

				}
				hex.GetComponent<HexManager> ().pollute ();
				if (replacedFactories >= 2) {
					actText.text = "You win!"; 
				}
			}
		}
		foreach (GameObject hex in grid) {
			if (hex != null) {
				hex.GetComponent<HexManager> ().refresh ();
			}
		}
	}
}
