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
	public Text pollText;
	public Text badText;

	public int replacedFactories;
	public int pollutionLimit;

	public Button gatherButton;
	public Button buildButton;
	public Button moveButton;
	public Button cleanButton;
	public Button endTurnButton;
	public Button rushButton;

	public int ore; 
	public int wood;
	public int clay;
	public int sand;
	public int shell;
	public int leather;

	public double totalHexes;
	public double pollutedHexes;


	// Use this for initialization
	void Start () {
		totalHexes = Mathf.Infinity;
		pollutedHexes = 0;

		Jayson = GameObject.FindGameObjectWithTag ("Jayson").GetComponent<Player> ();
		Zoya = GameObject.FindGameObjectWithTag ("Zoya").GetComponent<Player> ();
		Mariana = GameObject.FindGameObjectWithTag ("Mariana").GetComponent<Player>();
		ore = 0;
		wood = 0;
		sand = 0;
		clay = 0;
		shell = 0; //can replace ore or clay
		leather = 0; //can replace woo or sand



		bankText.text = "";
		badText.text = "";

		gatherButton.onClick.AddListener (Gather);
		buildButton.onClick.AddListener (Build);
		moveButton.onClick.AddListener (Move);
		cleanButton.onClick.AddListener (Clean);
		endTurnButton.onClick.AddListener (EndTurn);
		rushButton.onClick.AddListener (Rush);

		replacedFactories = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		//bankText.text = "Ore: " + ore.ToString() + "\nWood: " + wood.ToString();
		bankText.text = "x" + ore.ToString() +"\n\n" + "x" + wood.ToString() + "\n\n" + "x" + sand.ToString() + "\n\n" + "x" + clay.ToString();
		badText.text = "Shell x" + shell.ToString () + " Leather x" + leather.ToString ();

		if (getActivePlayer ()) {
			actText.text = "Active Character: " + getActivePlayer ().tag + "\nActions Left: " + getActivePlayer ().getActionCount ().ToString ()
			+ "\nMovements Left:" + getActivePlayer ().movement.ToString ();
		}
		else {
			actText.text = "No Active Character";
			bankText.text = "x" + ore.ToString() +"\n\n" + "x" + wood.ToString() + "\n\n" + "x" + sand.ToString() + "\n\n" + "x" + clay.ToString();
			badText.text = "Shell x" + shell.ToString () + " Leather x" + leather.ToString ();
		}

		if (replacedFactories >= 4) {
			actText.text = "You win!"; 
		}

		if ((int)((pollutedHexes/totalHexes)*100) >= pollutionLimit) {
			actText.text = "You lose!";
		}
		pollText.text = ((int)((pollutedHexes / totalHexes) * 100)).ToString () + "% of Tiles Polluted\n(Limit = " + pollutionLimit.ToString () + "%)";
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
		if (getActivePlayer() != null) {
			getActivePlayer ().Gather ();
		}
	}

	void Build() {
		if (getActivePlayer() != null) {
			getActivePlayer ().Build ();
		}
	}

	void Move() {
		if (getActivePlayer() != null) {
			getActivePlayer ().Move ();
		}
	}

	void Clean() {
		if (getActivePlayer() != null) {
			getActivePlayer ().Clean ();
		}
	}

	void Rush(){
		if (getActivePlayer () != null) {
			getActivePlayer ().Rush ();
		}
	}

	void EndTurn() {
		Jayson.EndTurn ();
		Zoya.EndTurn ();
		Mariana.EndTurn ();

		GameObject gm = GameObject.Find ("GameManager");
		IList<GameObject> grid = gm.GetComponent<GridManager> ().getGrid ();
		foreach(GameObject hex in grid) {
			if (hex != null) {
				hex.GetComponent<HexManager> ().pollute ();
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
			}
		}
		double hexes = 0;
		double pHexes = 0;
		foreach (GameObject hex in grid) {
			if (hex != null) {
				++hexes;
				hex.GetComponent<HexManager> ().refresh ();
				if (hex.GetComponent<HexManager> ().getPollution () > 0) {
					++pHexes;
				}
			}
		}
		totalHexes = hexes;
		pollutedHexes = pHexes;
	}
}
