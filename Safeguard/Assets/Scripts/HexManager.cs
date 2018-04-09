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
	public Text buildingText;
	private bool hasBuilding;
	private int buildingLife;
	private int adjBuilds;

	public GameObject buildingSprite;

	private GameObject building;

	private float hexWidth;
	private float hexHeight;

	//gathering variables
	public Text GCDText;
	private int GCoolDown;

	public int x;
	public int y;

	public Sprite sprite;
	public Sprite CD3sprite;
	public Sprite CD2sprite;
	public Sprite CD1sprite;

	public Text infoText;
	public Transform popupText;
	public static bool textStatus = false;

	public int getPollution() {
		return pollution;
	}

	public void setPollution(int poll) {
		pollution = poll;
	}

	public bool isFactory() {
		return tile == GridManager.TileType.FACTORY;
	}

	public void incrememntPollution() {
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

		GCDText = (UnityEngine.UI.Text)Instantiate (GCDText);
		GCDText.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
		GCDText.text = "";

		buildingText = (UnityEngine.UI.Text)Instantiate (buildingText);
		buildingText.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
		buildingText.text = "";

		hasBuilding = false;
		pollution = 0;
	}
	
	// Update is called once per frame
	void Update () {
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
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (255, 175, 255, 255);
		}
		if (pollution == 2) {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (255, 125, 255, 255);
		}
		if (pollution == 3) {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (255, 25, 255, 255);
		}
		if (hasBuilding) {
			if (buildingLife == 3) {
				building.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
			}
			if (buildingLife == 2) {
				building.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0);
			}
			if (buildingLife == 1) {
				building.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
			}
		}
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
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

	/*void pls() {
		popupText.GetComponent<TextMesh> ().text = "Gathering Cooldown: " + GetGCoolDown ().ToString () + "\nBuilding Cooldown: " + GetBuildingCoolDown ().ToString ();
		textStatus = true;
		Instantiate (popupText, new Vector3 (transform.position.x, transform.position.y + 2, 0), popupText.rotation);
	}

	void DisplayCooldownInfo() {
		Vector3 offset = new Vector3 (700, 300, 0);
		if (Input.GetMouseButton (0)) {
			if (!textStatus) { 
				infoText.text = "Gathering Cooldown: " + GetGCoolDown ().ToString () + "\nBuilding Cooldown: " + GetBuildingCoolDown ().ToString ();
				Debug.Log ("mouse down");
				textStatus = true;
			} else if (textStatus) {
				textStatus = false; 
				infoText.text = "";
			}
		}
	}

	void DisplayPopUp() {
		if (textStatus == false) {
			popupText.GetComponent<TextMesh> ().text = "Gathering Cooldown: " + GetGCoolDown ().ToString () + "\nBuilding Cooldown: " + GetBuildingCoolDown ().ToString ();
			Instantiate (popupText, new Vector3 (transform.position.x, transform.position.y + 2, 0), popupText.rotation);
		}

		if (textStatus == true) {
			textStatus = false;
			Instantiate (popupText, new Vector3 (transform.position.x, transform.position.y + 2, 0), popupText.rotation);
			//popupText.GetComponent<TextMesh> ().text = "";
		}
	}*/
				
	void OnMouseExit() {
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		gridManager.setMouseHex (null);

		if (textStatus && !this.isFactory()) {
			textStatus = false;
		}
	}

	public void MakeBuilding() {
		building = (GameObject)Instantiate (buildingSprite);
		building.transform.position = this.transform.position;
	}

	public void RemoveBuilding() {
		building.SetActive (false);
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
				n1.GetComponent<HexManager> ().incrememntPollution ();
			}
			if (n2 != null && n2.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n2.GetComponent<HexManager>().getHasBuilding() && !n2.GetComponent<HexManager>().isFactory()) {
				n2.GetComponent<HexManager> ().incrememntPollution ();
			}
			if (n3 != null && n3.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n3.GetComponent<HexManager>().getHasBuilding() && !n3.GetComponent<HexManager>().isFactory()) {
				n3.GetComponent<HexManager> ().incrememntPollution ();
			}
			if (n4 != null && n4.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n4.GetComponent<HexManager>().getHasBuilding() && !n4.GetComponent<HexManager>().isFactory()) {
				n4.GetComponent<HexManager> ().incrememntPollution ();
			}
			if (n5 != null && n5.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n5.GetComponent<HexManager>().getHasBuilding() && !n5.GetComponent<HexManager>().isFactory()) {
				n5.GetComponent<HexManager> ().incrememntPollution ();
			}
			if (n6 != null && n6.GetComponent<HexManager>().getAdjBuilds() == 0
				&& !n6.GetComponent<HexManager>().getHasBuilding() && !n6.GetComponent<HexManager>().isFactory()) {
				n6.GetComponent<HexManager> ().incrememntPollution ();
			}
		}
	}

	public void refresh() {
		pollutedThisTurn = false;
	}
}
