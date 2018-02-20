using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Text bankText; 
	public Text actText;
	public Text msgText;
	public int movement;

	public Button gatherButton;
	public Button buildButton;
	public Button moveButton;
	public Button cleanButton;
	public Button endTurnButton;

	//public GameObject buildingSprite;

	//private GameObject building;

	private bool canMove;
	private bool active;
	private int ore; 
	private int wood;
	private int actionCount;
	private Vector3 targetPos;

	private GameObject currentHex;

	// Use this for initialization
	void Start () {
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		currentHex = gridManager.getHex (0, 0);
		transform.position = currentHex.transform.position;
		transform.position -= new Vector3 (0, 0, 1);

		ore = 0;
		wood = 0;
		bankText.text = "";
		msgText.text = "";
		actionCount = 3;
		targetPos = transform.position;
		canMove = false;
		active = false;

		gatherButton.onClick.AddListener (Gather);
		buildButton.onClick.AddListener (Build);
		moveButton.onClick.AddListener (Move);
		cleanButton.onClick.AddListener (Clean);
		endTurnButton.onClick.AddListener (EndTurn);
	}
	
	// Update is called once per frame
	void Update () {
		bankText.text = "Action Count: " + actionCount.ToString() + "\nMovement Left:" + movement.ToString() + "\nOre: " + ore.ToString() + "\nWood: " + wood.ToString();
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		GameObject mouseHex = gridManager.getMouseHex ();
		if (active && movement > 0 && canMove && mouseHex != null) {
			HexManager mhMan = mouseHex.GetComponent<HexManager> ();
			if (Input.GetKeyDown (KeyCode.Mouse0) && mhMan.checkLegalMove (currentHex)) {
				targetPos = mouseHex.transform.position;
				transform.position = targetPos;
				transform.position -= new Vector3 (0, 0, 1);
				currentHex = mouseHex;
				movement--;
				canMove = false;
			}
		}

		if (actionCount == 0) {
			active = false;
		}
			
		if (!active) {
			actText.text = "No Active Character";
			bankText.text = "Action Count: " + "\nMovement Left:" + "\nOre: " + ore.ToString() + "\nWood: " + wood.ToString();
		}
	}

	void Gather() {
		if (currentHex.GetComponent<HexManager> ().GetGCoolDown () == 0
			&& currentHex.GetComponent<HexManager>().getPollution() == 0) {
			if (actionCount >= 2 && active) {
				int o = Random.Range (3, 3);
				ore += o;
				int w = Random.Range (3, 3);
				wood += w;

				actionCount -= 2;
				currentHex.GetComponent<HexManager> ().StartGCoolDown ();
				currentHex.GetComponent<HexManager> ().DisplayGCD();
				msgText.text= "You have gathered " + o.ToString() + " ore, and " + w.ToString() + " wood";
			}
		}
	}

	void Build() {
		bool hasBuilding = currentHex.GetComponent<HexManager> ().getHasBuilding ();
		if (currentHex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0 && hasBuilding == false
			&& currentHex.GetComponent<HexManager>().getPollution() == 0) {
			if (active && actionCount > 0) {
				if ((ore >= 3 && wood >= 2)) {
					ore -= 3;
					wood -= 2;
					//building = (GameObject)Instantiate (buildingSprite);
					//building.transform.position = this.transform.position;
					actionCount--;

					currentHex.GetComponent<HexManager> ().MakeBuilding ();
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().DisplayBuilding ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();

					GameObject gaman = GameObject.Find ("GameManager");
					GridManager gridManager = gaman.GetComponent<GridManager> ();
					int fx = currentHex.GetComponent<HexManager>().x - gridManager.gridSideLength + 1;
					int fy = currentHex.GetComponent<HexManager>().y - gridManager.gridSideLength + 1;
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

				} else if ((ore >= 2 && wood >= 3)) {
					ore -= 2;
					wood -= 3;
					//GameObject building = (GameObject)Instantiate (buildingSprite);
					//building.transform.position = this.transform.position;
					actionCount--;

					currentHex.GetComponent<HexManager> ().MakeBuilding ();
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().DisplayBuilding ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();
				}
			}
		}
	}

	void Move() {
		if (active) {
			canMove = true;
		}
	}

	void Clean() {
		if (currentHex.GetComponent<HexManager> ().getPollution() != 0) {
			currentHex.GetComponent<HexManager> ().decrementPollution ();
			--actionCount;
		}
	}

	void OnMouseDown() {
		active = true;
		actText.text = "Active Character: Jayson\nActions: " + actionCount.ToString();
	}

	void EndTurn() {
		active = false;
		actionCount = 3;
		movement = 2;
		msgText.text = "";

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
			}
		}


	}
}