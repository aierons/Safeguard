using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {


	//public GameObject buildingSprite;

	//private GameObject building;

	private TeamManager team;

	public Text msgText;

	public float xoffset;
	public float yoffset;

	public int movement;
	public int buildingDiscount;
	public int gatherBonus;
	private int maxMovement;
	private bool canMove;
	private bool active;
	private int actionCount;
	private Vector3 targetPos;

	private GameObject currentHex;

	// Use this for initialization
	void Start () {
		team = GameObject.FindGameObjectWithTag ("Team").GetComponent<TeamManager> ();

		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		currentHex = gridManager.getHex (0, 0);
		transform.position = currentHex.transform.position;
		transform.position -= new Vector3 (0, 0, 1);
		transform.position += new Vector3 (xoffset, yoffset, 0);

		msgText.text = "";

		actionCount = 3;
		targetPos = transform.position;
		canMove = false;
		active = false;

		maxMovement = movement;

	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		GameObject mouseHex = gridManager.getMouseHex ();
		if (active && movement > 0 && canMove && mouseHex != null) {
			HexManager mhMan = mouseHex.GetComponent<HexManager> ();
			if (Input.GetKeyDown (KeyCode.Mouse0) && mhMan.checkLegalMove (currentHex, this, 1) && !mouseHex.Equals (currentHex)) {
				targetPos = mouseHex.transform.position;
				transform.position = targetPos;
				transform.position -= new Vector3 (0, 0, 1);
				transform.position += new Vector3 (xoffset, yoffset, 0);
				currentHex = mouseHex;
				canMove = false;
			}
		}
		/*
		if (actionCount == 0) {
			active = false;
		}
		*/
	}

	public bool isActive() {
		return active;
	}

	public void setActive(bool b) {
		active = b;
	}

	public int getActionCount() {
		return actionCount;
	}

	public void Gather() {
		canMove = false;
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if (currentHex.GetComponent<HexManager> ().GetGCoolDown () == 0
			&& currentHex.GetComponent<HexManager>().getPollution() == 0
			&& !(currentHex.transform.position == gridManager.getHex (0, -3).transform.position
				|| currentHex.transform.position == gridManager.getHex (0, 3).transform.position)) {
			if (actionCount >= 2 && active) {
				int o = Random.Range (1 + gatherBonus, 3 + gatherBonus);
				team.ore += o;
				int w = Random.Range (1 + gatherBonus, 3 + gatherBonus);
				team.wood += w;
				int c = Random.Range (1 + gatherBonus, 3 + gatherBonus);
				team.clay += c;
				int s = Random.Range (1 + gatherBonus, 3 + gatherBonus);
				team.sand += s;

				actionCount -= 2;
				currentHex.GetComponent<HexManager> ().StartGCoolDown ();
				currentHex.GetComponent<HexManager> ().DisplayGCD();
				msgText.text= "You have gathered " + o.ToString() + " ore, " + w.ToString() + " wood, \n"  + c.ToString() + " clay, and " + s.ToString() + " sand.";
			}
		}
	}

	public void Build() {
		canMove = false;
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if ((currentHex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0 && currentHex.GetComponent<HexManager> ().getHasBuilding () == false
			&& currentHex.GetComponent<HexManager>().getPollution() == 0) ||
			currentHex.GetComponent<HexManager>().isFactory()) {
			if (active && actionCount > 0) {
				//build factory
				if (team.ore >= 5 - buildingDiscount && team.wood >= 5 - buildingDiscount && team.sand >= 5 - buildingDiscount && team.clay >= 5 - buildingDiscount
				    && currentHex.GetComponent<HexManager> ().isFactory ()) {
					int fx = currentHex.GetComponent<HexManager> ().x - gridManager.gridSideLength + 1;
					int fy = currentHex.GetComponent<HexManager> ().y - gridManager.gridSideLength + 1;
					GameObject n1 = gridManager.getHex (fx + 1, fy);
					GameObject n2 = gridManager.getHex (fx, fy + 1);
					GameObject n3 = gridManager.getHex (fx - 1, fy);
					GameObject n4 = gridManager.getHex (fx, fy - 1);
					GameObject n5 = gridManager.getHex (fx - 1, fy + 1);
					GameObject n6 = gridManager.getHex (fx + 1, fy - 1);
					if (n1 != null && n1.GetComponent<HexManager> ().getPollution () != 0) {
						return;
					}
					if (n2 != null && n2.GetComponent<HexManager> ().getPollution () != 0) {
						return;
					}
					if (n3 != null && n3.GetComponent<HexManager> ().getPollution () != 0) {
						return;
					}
					if (n4 != null && n4.GetComponent<HexManager> ().getPollution () != 0) {
						return;
					}
					if (n5 != null && n5.GetComponent<HexManager> ().getPollution () != 0) {
						return;
					}
					if (n6 != null && n6.GetComponent<HexManager> ().getPollution () != 0) {
						return;
					}
					currentHex.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0);
					currentHex.GetComponent<HexManager> ().decrementPollution ();
					team.ore -= 5;
					team.wood -= 5;
					team.clay -= 5;
					team.sand -= 5;
					actionCount--;
					team.replacedFactories++;
					msgText.text = "Built Factory, Ore -5, Wood -5, Clay -5, Sand -5";
				} else if (currentHex.GetComponent<HexManager> ().isFactory ()) {
					return;
					//Windmill 3 ore, 2 wood
				} else if ((team.ore >= team.wood || team.shell >= team.wood || team.shell >= team.leather) 
					&& ((team.ore >= 3 - buildingDiscount || team.shell >= 3 - buildingDiscount)
					&& (team.wood >= 2 - buildingDiscount || team.leather >= 2 - buildingDiscount))) {
					
					if (team.shell >= 3 - buildingDiscount) {
						team.shell -= 3;
					} else {
						team.ore -= 3;
					}
					if (team.leather >= 2 - buildingDiscount) {
						team.leather -= 2;
					} else {
						team.wood -= 2;
					}

					//building = (GameObject)Instantiate (buildingSprite);
					//building.transform.position = this.transform.position;
					actionCount--;
					msgText.text = "Built Windmill";

					currentHex.GetComponent<HexManager> ().MakeBuilding ();
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().DisplayBuilding ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();

					//GameObject gaman = GameObject.Find ("GameManager");
					//GridManager gridManager = gaman.GetComponent<GridManager> ();
					int fx = currentHex.GetComponent<HexManager> ().x - gridManager.gridSideLength + 1;
					int fy = currentHex.GetComponent<HexManager> ().y - gridManager.gridSideLength + 1;
					GameObject n1 = gridManager.getHex (fx + 1, fy);
					GameObject n2 = gridManager.getHex (fx, fy + 1);
					GameObject n3 = gridManager.getHex (fx - 1, fy);
					GameObject n4 = gridManager.getHex (fx, fy - 1);
					GameObject n5 = gridManager.getHex (fx - 1, fy + 1);
					GameObject n6 = gridManager.getHex (fx + 1, fy - 1);
					if (n1 != null) {
						n1.GetComponent<HexManager> ().incrementBuilding ();
					}
					if (n2 != null) {
						n2.GetComponent<HexManager> ().incrementBuilding ();
					}
					if (n3 != null) {
						n3.GetComponent<HexManager> ().incrementBuilding ();
					}
					if (n4 != null) {
						n4.GetComponent<HexManager> ().incrementBuilding ();
					}
					if (n5 != null) {
						n5.GetComponent<HexManager> ().incrementBuilding ();
					}
					if (n6 != null) {
						n6.GetComponent<HexManager> ().incrementBuilding ();
					}
					//Water Wheel 3 wood, 2 ore
				} else if (((team.ore >= 2 - buildingDiscount || (team.shell >= 2 - buildingDiscount))
					&& (team.wood >= 3 - buildingDiscount || team.leather >= 3 - buildingDiscount))) {
					if (team.shell >= 2 - buildingDiscount) {
						team.shell -= 2;
					} else {
						team.ore -= 2;
					}
					if (team.leather >= 3 - buildingDiscount) {
						team.leather -= 3;
					} else {
						team.wood -= 3;
					}
					//GameObject building = (GameObject)Instantiate (buildingSprite);
					//building.transform.position = this.transform.position;
					actionCount--;
					msgText.text = "Built Water Wheel";

					currentHex.GetComponent<HexManager> ().MakeBuilding ();
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().DisplayBuilding ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();
				}
				//Solar Panel 3 sand 2 clay
				else if ((team.sand >= team.clay || team.leather >= team.wood || team.leather >= team.shell) 
					&& ((team.sand >= 3 - buildingDiscount || team.leather >= 3 - buildingDiscount)
						&& (team.clay >= 2 - buildingDiscount || team.shell >= 2 - buildingDiscount))) {
					if (team.shell >= 2 - buildingDiscount) {
						team.shell -= 2;
					} else {
						team.clay -= 2;
					}
					if (team.leather >= 3 - buildingDiscount) {
						team.leather -= 3;
					} else {
						team.sand -= 3;
					}

					actionCount--;
					msgText.text = "Built Solar Panel";

					currentHex.GetComponent<HexManager> ().MakeBuilding ();
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().DisplayBuilding ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();
				//Compost Station 3 clay, 2 sand
				} else if (((team.sand >= 2 - buildingDiscount || (team.leather >= 2 - buildingDiscount))
					&& (team.clay >= 3 - buildingDiscount || team.shell >= 3 - buildingDiscount))) {

					if (team.shell >= 3- buildingDiscount) {
						team.shell -= 3;
					} else {
						team.clay -= 3;
					}
					if (team.leather >= 2 - buildingDiscount) {
						team.leather -= 2;
					} else {
						team.sand -= 2;
					}
					actionCount--;
					msgText.text = "Built Compost Station, Clay -3, Sand -2";

					currentHex.GetComponent<HexManager> ().MakeBuilding ();
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().DisplayBuilding ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();
				}
			}
		}
	}

	public void Move() {
		if (active) {
			canMove = true;
		}
	}

	public void Clean() {
		if (active && currentHex.GetComponent<HexManager> ().getPollution() != 0
			&& !currentHex.GetComponent<HexManager>().isFactory() && actionCount > 0) {
			currentHex.GetComponent<HexManager> ().decrementPollution ();
			if (currentHex.GetComponent<HexManager> ().getPollution () == 0) {
				GameObject t = GameObject.Find ("Team");
				TeamManager tm = t.GetComponent<TeamManager> ();
				--tm.pollutedHexes;
			}
			--actionCount;
		}
	}

	public void Rush() {
		canMove = false;
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if (currentHex.GetComponent<HexManager> ().GetGCoolDown () == 0
			&& currentHex.GetComponent<HexManager>().getPollution() == 0
			&& !(currentHex.transform.position == gridManager.getHex (0, -3).transform.position
				|| currentHex.transform.position == gridManager.getHex (0, 3).transform.position)) {
			if (actionCount >= 1 && active) {
				int shell = Random.Range (2 + gatherBonus, 4 + gatherBonus);
				team.shell += shell;
				int leather = Random.Range (2 + gatherBonus, 4 + gatherBonus);
				team.leather += leather;

				actionCount -= 1;
				currentHex.GetComponent<HexManager> ().incrememntPollution ();
				currentHex.GetComponent<HexManager> ().StartGCoolDown ();
				currentHex.GetComponent<HexManager> ().DisplayGCD();
				msgText.text= "You have Rush Gathered " + shell.ToString() + " shell and " + leather.ToString() + " leather. \n Pollution in this area has increased";
			}
		}
	}

	void OnMouseDown() {
		active = true;
		for(int i = 0; i < 3; i++)
		{
			Transform Go = GameObject.FindGameObjectWithTag ("Team").transform.GetChild (i);
			if (Go.tag != this.tag) {
				Go.GetComponent<Player> ().setActive (false);
			}
		}
	}

	public void EndTurn() {
		//active = false;
		actionCount = 3;
		movement = maxMovement;
		msgText.text = "";
	}
}