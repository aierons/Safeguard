using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexManager : MonoBehaviour {

	//pollution variables
	protected int pollution;
	private bool pollutedThisTurn = false;

	public GridManager.TileType tile = GridManager.TileType.EMPTY;

	//building variables
	private bool hasBuilding;
	private int buildingLife;
	private int adjBuilds;

	public Sprite buildingSolarPanel;
	public Sprite buildingCompost;
	public Sprite buildingWindmill;

	public GameObject building;
	private GameObject buildingclone;

	private float hexWidth;
	private float hexHeight;

	//gathering variables
	private int GCoolDown;

	public int x;
	public int y;

	public Sprite sprite;
	public Sprite CD3sprite;
	public Sprite CD2sprite;
	public Sprite CD1sprite;

	public Transform popupText;
	public static bool textStatus = false;

	public enum BuildingType
	{
		SOLAR, WIND, COMPOST
	}

	public bool sheepPresent;

	public int getPollution() {
		return pollution;
	}

	public void setPollution(int poll) {
		pollution = poll;
	}

	public bool isFactory() {
		return tile == GridManager.TileType.FACTORY;
	}

	public void incrementPollution() {
		if (pollution < 3 && !pollutedThisTurn) {
			++pollution;
			pollutedThisTurn = true;
		}
	}

	public void decrementPollution() {
		if (pollution > 0) {
			--pollution;
		}
	}

	public int getAdjBuilds() {
		return adjBuilds;
	}

	public void incrementBuilding() {
		++adjBuilds;
	}

	public void decrementBuilding() {
		if (adjBuilds > 0) {
			--adjBuilds;
		}
	}

	public bool checkLegalMove(GameObject other, Player mover, int depth) {
		if (other == null) {
			return false;
		}
		HexManager otherHex = other.GetComponent<HexManager> ();
		int xa = Mathf.Abs (this.x - otherHex.x);
		int ya = Mathf.Abs (this.y - otherHex.y);
		if (xa <= 1 && ya <= 1 && Mathf.Abs ((otherHex.x - this.x) + (otherHex.y - this.y)) < 2) {
			mover.setMinMove (depth);
			return true;
		} else if (mover.movement > depth) {
			GameObject gm = GameObject.Find ("GameManager");
			GridManager gridManager = gm.GetComponent<GridManager> ();
			int fx = other.GetComponent<HexManager> ().x - gridManager.gridSideLength + 1;
			int fy = other.GetComponent<HexManager> ().y - gridManager.gridSideLength + 1;
			GameObject n1 = gridManager.getHex (fx + 1, fy);
			GameObject n2 = gridManager.getHex (fx, fy + 1);
			GameObject n3 = gridManager.getHex (fx - 1, fy);
			GameObject n4 = gridManager.getHex (fx, fy - 1);
			GameObject n5 = gridManager.getHex (fx - 1, fy + 1);
			GameObject n6 = gridManager.getHex (fx + 1, fy - 1);
			bool legal = false;
			if (this.checkLegalMove (n1, mover, depth + 1)) {
				legal = true;
			}
			if (this.checkLegalMove (n2, mover, depth + 1)) {
				legal = true;
			}
			if (this.checkLegalMove (n3, mover, depth + 1)) {
				legal = true;
			}
			if (this.checkLegalMove (n4, mover, depth + 1)) {
				legal = true;
			}
			if (this.checkLegalMove (n5, mover, depth + 1)) {
				legal = true;
			}
			if (this.checkLegalMove (n6, mover, depth + 1)) {
				legal = true;
			}
			return legal;
		}
		return false;
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

		hexWidth = GetComponent<Renderer>().bounds.size.x;
		hexHeight = GetComponent<Renderer>().bounds.size.y;

		hasBuilding = false;
		pollution = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isFactory()) {
			if (GCoolDown == 3) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = CD3sprite;
			}
			if (GCoolDown == 2) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = CD2sprite;
			}
			if (GCoolDown == 1) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = CD1sprite;
			}
			if (GCoolDown == 0) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
			}
			if (pollution == 0) {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			}
			if (pollution == 1) {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (155, 125, 155, 255);
			}
			if (pollution == 2) {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (125, 100, 125, 255);
			}
			if (pollution == 3) {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (100, 75, 100, 255);
			}
			if (hasBuilding) {
				if (buildingLife == 3) {
					building.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
				}
				if (buildingLife == 2) {
					building.GetComponent<SpriteRenderer> ().color = new Color32 (200, 200, 200, 255);
				}
				if (buildingLife == 1) {
					building.GetComponent<SpriteRenderer> ().color = new Color32 (90, 90, 90, 255);
				}
			}
			GameObject thisPopup = GameObject.Find ("PopupText(Clone)");
			if (thisPopup != null) {
				GameObject gm = GameObject.Find ("GameManager");
				GridManager gridManager = gm.GetComponent<GridManager> ();
				if (gridManager.getRaycastResults() != null && gridManager.getRaycastResults().Count != 0) {
					thisPopup.transform.localScale = Vector3.zero;
				} else {
					thisPopup.transform.localScale = new Vector3 (1, 1, 1);
				}
			}

		}
	}

	public void SwitchHasBuilding() {
		hasBuilding = !hasBuilding;
	}

	void OnMouseEnter() {
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		gridManager.setMouseHex (this.gameObject);

		GameObject pop = GameObject.Find ("PopupText(Clone)");
		if (pop != null) {
			Destroy (pop);
		}

		if (!textStatus && !this.isFactory()) {
			popupText.GetComponent<TextMesh> ().text = 
				tile.ToString() +
				"\nPollution Level: " + getPollution().ToString() +
				"\nGathering Cooldown: " + GetGCoolDown ().ToString () + 
				"\nBuilding Cooldown: " + GetBuildingCoolDown ().ToString ();
			textStatus = true;
			Instantiate (popupText, new Vector3 (transform.position.x, transform.position.y + 2, 0), popupText.rotation);
		}

		Vector3 offset = new Vector3 (700, 300, 0);
	}
				
	void OnMouseExit() {
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		gridManager.setMouseHex (null);

		if (textStatus && !this.isFactory()) {
			textStatus = false;
		}
	}

	public void MakeBuilding(BuildingType type) {
		building.SetActive (true);
		buildingLife = 3;
		building.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
		building.transform.position = this.gameObject.transform.position;
		switch(type) {
		case BuildingType.SOLAR:
			building.GetComponent<SpriteRenderer> ().sprite = buildingSolarPanel;
			break;
		case BuildingType.WIND:
			building.GetComponent<SpriteRenderer> ().sprite = buildingWindmill;
			break;
		case BuildingType.COMPOST:
			building.GetComponent<SpriteRenderer> ().sprite = buildingCompost;
			break;
		}
		buildingclone = Instantiate(building);
	}

	public void RemoveBuilding() {
		building.SetActive (false);
		Destroy (buildingclone);
	}

	public void pollute() {
		if (pollution >= 3 && !pollutedThisTurn) {
			GameObject gm = GameObject.Find ("GameManager");
			GridManager gridManager = gm.GetComponent<GridManager> ();
			int fx = x - gridManager.gridSideLength + 1;
			int fy = y - gridManager.gridSideLength + 1;
			GameObject n1 = gridManager.getHex (fx + 1, fy);
			GameObject n2 = gridManager.getHex (fx, fy + 1);
			GameObject n3 = gridManager.getHex (fx - 1, fy);
			GameObject n4 = gridManager.getHex (fx, fy - 1);
			GameObject n5 = gridManager.getHex (fx - 1, fy + 1);
			GameObject n6 = gridManager.getHex (fx + 1, fy - 1);
			if (n1 != null && n1.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n1.GetComponent<HexManager>().getHasBuilding() && !n1.GetComponent<HexManager>().isFactory()) {
				n1.GetComponent<HexManager> ().incrementPollution ();
			}
			if (n2 != null && n2.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n2.GetComponent<HexManager>().getHasBuilding() && !n2.GetComponent<HexManager>().isFactory()) {
				n2.GetComponent<HexManager> ().incrementPollution ();
			}
			if (n3 != null && n3.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n3.GetComponent<HexManager>().getHasBuilding() && !n3.GetComponent<HexManager>().isFactory()) {
				n3.GetComponent<HexManager> ().incrementPollution ();
			}
			if (n4 != null && n4.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n4.GetComponent<HexManager>().getHasBuilding() && !n4.GetComponent<HexManager>().isFactory()) {
				n4.GetComponent<HexManager> ().incrementPollution ();
			}
			if (n5 != null && n5.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n5.GetComponent<HexManager>().getHasBuilding() && !n5.GetComponent<HexManager>().isFactory()) {
				n5.GetComponent<HexManager> ().incrementPollution ();
			}
			if (n6 != null && n6.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n6.GetComponent<HexManager>().getHasBuilding() && !n6.GetComponent<HexManager>().isFactory()) {
				n6.GetComponent<HexManager> ().incrementPollution ();
			}
		}
	}

	public void refresh() {
		pollutedThisTurn = false;
	}
}
