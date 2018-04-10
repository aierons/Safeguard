using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour {

	//pollution bar
	public Slider pollutionBar; 

	public int build_p;
	public int rush_p;
	public int gather_p;
	public int clean_p;
	public int build_c;
	public int rush_c;
	public int gather_c;
	public int clean_c;
	public int turn_c;
	public int turn_p;
	private int total_p;

	private Player Jayson;
	private Player Zoya;
	private Player Mariana;

	public Text bankText; 
	public Text actText;
	public Text pollText;

	public int replacedFactories;
	public int pollutionLimit;

	public Button gatherButton;
	//public Button buildButton;
	public Button cleanButton;
	public Button endTurnButton;
	public Button rushButton;

	//build specific temp buildings buttons
	public Button buildSingleWindmillButton;
	public Button buildSingleSolarPanelButton; 
	public Button buildSingleRecyclingHouseButton; 

	//build specific eco-friendly replacement buttons
	public Button buildReplacementButton;
	public int ore; 
	public int wood;
	public int clay;
	public int sand;
	public int shell;
	public int leather;

	public double totalHexes;
	public double pollutedHexes;

	private string team;
	// Use this for initialization
	void Start () {
		totalHexes = Mathf.Infinity;
		pollutedHexes = 0;

		Jayson = GameObject.FindGameObjectWithTag ("Jayson").GetComponent<Player> ();
		Zoya = GameObject.FindGameObjectWithTag ("Zoya").GetComponent<Player> ();
		Mariana = GameObject.FindGameObjectWithTag ("Mariana").GetComponent<Player>();
		ore = 0;
		wood = 0;
		clay = 0;
		sand = 0;
		shell = 0;
		leather = 0;

		build_p = 0;
		rush_p = 0;
		gather_p = 0;
		clean_p = 0;
		turn_p = 0;

		build_c = 0;
		rush_c = 0;
		gather_c = 0;
		clean_c = 0;
		turn_c = 0;

		total_p = 0;

		bankText.text = "";

		gatherButton.onClick.AddListener (Gather);
//		buildButton.onClick.AddListener (Build);
		cleanButton.onClick.AddListener (Clean);
		endTurnButton.onClick.AddListener (EndTurn);
		rushButton.onClick.AddListener (Rush);

		buildSingleWindmillButton.onClick.AddListener(BuildSingleWindmill);
		buildSingleSolarPanelButton.onClick.AddListener(BuildSingleSolarPanel); 
		buildSingleRecyclingHouseButton.onClick.AddListener(BuildSingleRecyclingHouse); 

		//build specific eco-friendly replacement buttons
		buildReplacementButton.onClick.AddListener(BuildReplacement);

		replacedFactories = 0;

		team = "JAYSONZOYAMARIANA";

		pollutionBar.value = calculatePollution();

	}
	
	// Update is called once per frame
	void Update () {

		//bankText.text = "Ore: " + ore.ToString() + "\nWood: " + wood.ToString();
		bankText.text = "x" + ore.ToString() +"\n\n" + "x" + wood.ToString() + "\n\n" + "x" + clay.ToString() +"\n\n" + "x" + sand.ToString()
			+ "\n\n" + "x" + shell.ToString () + "\n\n" + "x" + leather.ToString ();
		/*
		 * if (getActivePlayer ().Equals (Jayson)) {
				actText.text = team.Substring (0, 6) + "\nAction Count: " + getActivePlayer ().getActionCount ().ToString ()
				+ "\nMovement Left: " + getActivePlayer ().movement.ToString ();
			}*/

		if (getActivePlayer ()) {
			if (getActivePlayer ().Equals (Jayson)) {
				actText.text = team.Substring (0, 6) + "\nACTIONS LEFT: " + "\nMOVEMENT LEFT: ";
				Jayson.getActionCount ();
				Jayson.getMovementCount ();
			}
			if (getActivePlayer ().Equals (Zoya)) {
				actText.text = team.Substring (6, 4) + "\nACTIONS LEFT: " + "\nMOVEMENT LEFT: ";
				Zoya.getActionCount ();
				Zoya.getMovementCount ();
			}
			if (getActivePlayer ().Equals (Mariana)) {
				actText.text = team.Substring (10, 7) + "\nACTIONS LEFT: " + "\nMOVEMENT LEFT: ";
				Mariana.getActionCount ();
				Mariana.getMovementCount ();
			}
			/*
			if (getActivePlayer().tag == "Jayson") {
				actText.text += "\nMobility: His quick feet gives him 1 extra movement every turn";
			} else if (getActivePlayer().tag == "Zoya") {
				actText.text += "\nBuilder: Her expertise with construction allows her to decrease the cost of building by 1 of each resource required.";
			} else {
				actText.text += "\nGatherer: Her experience with minimizing waste allows her to gather more resources (on average).";
			}
			*/
		} else {
			actText.text = team;
			bankText.text = "x" + ore.ToString() +"\n\n" + "x" + wood.ToString() + "\n\n" + "x" + clay.ToString() +"\n\n" + "x" + sand.ToString()
				+ "\n\n" + "x" + shell.ToString () + "\n\n" + "x" + leather.ToString ();
		}
		total_p = build_p + gather_p + rush_p + clean_p + turn_p;

		if (replacedFactories >= 4) {
			actText.text = "You win!"; 
			actText.text += "\nBuilding Built : " + build_c.ToString () + "\n\tPoints : " + build_p.ToString() + 
				"\nTimes Gathered : " + gather_c.ToString () + "\n\tPoints : " + gather_p.ToString() +
				"\nTimes Rushed : " + rush_c.ToString () + "\n\tPoints : " + rush_p.ToString() + 
				"\nTimes Cleaned : " + clean_c.ToString () + "\n\tPoints : " + clean_p.ToString() +
				"\nTurn Count : " + turn_c.ToString() + "\n\tPoints : " + turn_p.ToString() + 
				"\nTotal Score : " + total_p.ToString();
		}

		if ((int)((pollutedHexes/totalHexes)*100) >= pollutionLimit) {
			actText.text = "You lose!";
			actText.text += "\nBuilding Built : " + build_c.ToString () + "\n\tPoints : " + build_p.ToString() + 
				"\nTimes Gathered : " + gather_c.ToString () + "\n\tPoints : " + gather_p.ToString() +
				"\nTimes Rushed : " + rush_c.ToString () + "\n\tPoints : " + rush_p.ToString() + 
				"\nTimes Cleaned : " + clean_c.ToString () + "\n\tPoints : " + clean_p.ToString() +
				"\nTurn Count : " + turn_c.ToString() + "\n\tPoints : " + turn_p.ToString() + 
				"\nTotal Score : " + total_p.ToString();
		}
		pollText.text = ((int)((pollutedHexes / totalHexes) * 100)).ToString () + "% of Tiles Polluted\n(Limit = " + pollutionLimit.ToString () + "%)";

		pollutionBar.value = calculatePollution();
	}

	float calculatePollution() {
		return (float) (pollutedHexes / totalHexes); 
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

	void Rush() {
		if (getActivePlayer() != null) {
			getActivePlayer ().Rush ();
		}
	}

	/*
	void Build() {
		if (getActivePlayer() != null) {
			getActivePlayer ().Build ();
		}
	}
	*/

	void Clean() {
		if (getActivePlayer() != null) {
			getActivePlayer ().Clean ();
		}
	}

	void BuildSingleWindmill() {
		if (getActivePlayer () != null) {
			getActivePlayer ().BuildSingleWindmill ();
		}	
	}

	void BuildSingleSolarPanel() {
		if (getActivePlayer () != null) {
			getActivePlayer ().BuildSingleSolarPanel ();
		}
	}

	void BuildSingleRecyclingHouse() {
		if (getActivePlayer () != null) {
			getActivePlayer ().BuildSingleRecyclingHouse ();
		}
	}

	void BuildReplacement() {
		if (getActivePlayer () != null) {
			getActivePlayer ().BuildReplacement ();
		}
	}

	void EndTurn() {
		Jayson.EndTurn ();
		Zoya.EndTurn ();
		Mariana.EndTurn ();
		turn_c++;
		turn_p -= 10;

		GameObject gm = GameObject.Find ("GameManager");
		IList<GameObject> grid = gm.GetComponent<GridManager> ().getGrid ();
		foreach(GameObject hex in grid) {
			if (hex != null) {
				hex.GetComponent<HexManager> ().pollute ();
				if (hex.GetComponent<HexManager> ().GetGCoolDown () > 0) {
					hex.GetComponent<HexManager> ().LowerGCoolDown ();
				}
				if (hex.GetComponent<HexManager> ().GetBuildingCoolDown () > 0) {
					hex.GetComponent<HexManager> ().LowerBuildingCoolDown ();
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
