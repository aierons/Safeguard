using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GridManager: MonoBehaviour
{
	//following public variable is used to store the hex model prefab;
	//instantiate it by dragging the prefab on this variable using unity editor
	public GameObject Hex;
	public GameObject Factory;

	public Sprite OreSprite;
	public Sprite OreCD3;
	public Sprite OreCD2;
	public Sprite OreCD1;

	public Sprite ClaySprite;
	public Sprite ClayCD3;
	public Sprite ClayCD2;
	public Sprite ClayCD1;

	public Sprite SandSprite;
	public Sprite SandCD3;
	public Sprite SandCD2;
	public Sprite SandCD1;

	public Sprite TreeSprite;
	public Sprite TreeCD3;
	public Sprite TreeCD2;
	public Sprite TreeCD1;

	public Sprite EmptySprite;
	public Sprite EmptyCD3;
	public Sprite EmptyCD2;
	public Sprite EmptyCD1;

	//next two variables can also be instantiated using unity editor
	public int gridSideLength;

	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;
	public GameObject mouseHex;

	private GameObject hexGridGO;

	private GraphicRaycaster graphicRaycaster;
	private PointerEventData pointerEventData;
	private EventSystem eventSystem;
	private List<RaycastResult> raycastResults;

	static IList<GameObject> grid;

	public enum TileType
	{
		ORE, TREE, CLAY, SAND, FACTORY, EMPTY
	}

	public GameObject sheep;

	public int GetSideLength() {
		return gridSideLength;
	}

	public GameObject getHexGridGO() {
		return hexGridGO;
	}

	public IList<GameObject> getGrid() {
		return grid;
	}

	public GameObject getHex(int x, int y) {
		int realX = x + gridSideLength - 1;
		int realY = y + gridSideLength - 1;
		if (realX >= 0 && realX < (gridSideLength*2 - 1) && realY >= 0
			&& realY < (gridSideLength*2 - 1)) {
			return grid[realX + ((gridSideLength * 2) - 1) * realY];
		} else {
			return null;
		}
	}

	public GameObject getMouseHex() {
		return mouseHex;
	}

	public void setMouseHex(GameObject hex) {
		mouseHex = hex;
	}

	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth = Hex.GetComponent<Renderer>().bounds.size.x;
		hexHeight = Hex.GetComponent<Renderer>().bounds.size.y;
	}

	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 calcInitPos()
	{
		Vector3 initPos;
		//the initial position will be in the left upper corner
		/*initPos = new Vector3(-hexWidth * (gridSideLength + 6) / 2f + hexWidth / 2,
			-hexHeight * (gridSideLength + 2) / 2f + hexHeight / 2, 0);*/
		initPos = new Vector3 (0, 0, 0);

		return initPos;
	}

	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = calcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0;
		offset = hexWidth / 2;
		offset *= gridPos.y;

		float x =  initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float y = initPos.y + gridPos.y * hexHeight * 0.75f;
		return new Vector3(x, y, 0);
	}

	//Finally the method which initialises and positions all the tiles
	void createGrid()
	{
		//Game object which is the parent of all the hex tiles
		hexGridGO = new GameObject("HexGrid");
		bool factory1 = true;
		bool factory2 = true;
		bool factory3 = true;
		bool factory4 = true;

		int sort = 0;
		for (int y = 0; y < (gridSideLength * 2) - 1; y++)
		{
			for (int x = 0; x < (gridSideLength * 2) - 1; x++)
			{
				if(x + y == gridSideLength - 1 && factory1) {
					GameObject factory = (GameObject)Instantiate (Factory);
					Vector2 gridPos = new Vector2 (x, y);
					factory.transform.position = calcWorldCoord (gridPos);
					factory.transform.SetParent(hexGridGO.transform);
					factory.GetComponent<SpriteRenderer> ().sortingLayerName = "Hex2";
					factory.GetComponent<SpriteRenderer> ().sortingOrder = sort;
					FactoryManager fm = factory.GetComponent<FactoryManager> ();
					fm.x = x;
					fm.y = y;
					grid.Add (factory);
					factory1 = false;
					continue;
				}
				if (y == (gridSideLength * 2) - 2 && x == gridSideLength - 1 && factory2) {
					GameObject factory = (GameObject)Instantiate (Factory);
					Vector2 gridPos = new Vector2 (x, y);
					factory.transform.position = calcWorldCoord (gridPos);
					factory.transform.SetParent(hexGridGO.transform);
					factory.GetComponent<SpriteRenderer> ().sortingLayerName = "Hex2";
					factory.GetComponent<SpriteRenderer> ().sortingOrder = sort;
					FactoryManager fm = factory.GetComponent<FactoryManager> ();
					fm.x = x;
					fm.y = y;
					grid.Add (factory);
					factory2 = false;
					continue;
				}
				if (y == 0 && x == (gridSideLength * 2) - 2 && factory3) {
					GameObject factory = (GameObject)Instantiate (Factory);
					Vector2 gridPos = new Vector2 (x, y);
					factory.transform.position = calcWorldCoord (gridPos);
					factory.transform.SetParent(hexGridGO.transform);
					factory.GetComponent<SpriteRenderer> ().sortingLayerName = "Hex2";
					factory.GetComponent<SpriteRenderer> ().sortingOrder = sort;
					FactoryManager fm = factory.GetComponent<FactoryManager> ();
					fm.x = x;
					fm.y = y;
					grid.Add (factory);
					factory3 = false;
					continue;
				}
				if (y == (gridSideLength * 2) - 2 && x == 0 && factory4) {
					GameObject factory = (GameObject)Instantiate (Factory);
					Vector2 gridPos = new Vector2 (x, y);
					factory.transform.position = calcWorldCoord (gridPos);
					factory.transform.SetParent(hexGridGO.transform);
					factory.GetComponent<SpriteRenderer> ().sortingLayerName = "Hex2";
					factory.GetComponent<SpriteRenderer> ().sortingOrder = sort;
					FactoryManager fm = factory.GetComponent<FactoryManager> ();
					fm.x = x;
					fm.y = y;
					grid.Add (factory);
					factory4 = false;
					continue;
				}
				if (x + y >= gridSideLength - 1 && x + y <= 3 * (gridSideLength - 1)) {
					//GameObject assigned to Hex public variable is cloned
					GameObject hex = (GameObject)Instantiate (Hex);
					//Current position in grid
					Vector2 gridPos = new Vector2 (x, y);
					float rng = Random.value;
					if (rng <= 0.20) {
						hex.GetComponent<HexManager> ().tile = TileType.ORE;
						hex.GetComponent<HexManager> ().sprite = OreSprite;
						hex.GetComponent<HexManager> ().CD3sprite = OreCD3;
						hex.GetComponent<HexManager> ().CD2sprite = OreCD2;
						hex.GetComponent<HexManager> ().CD1sprite = OreCD1;
						hex.GetComponent<SpriteRenderer> ().sprite = OreSprite;
						hex.GetComponent<SpriteRenderer> ().sortingLayerName = "Hex2";
						hex.GetComponent<SpriteRenderer> ().sortingOrder = sort;
					} else if (rng <= 0.40) {
						hex.GetComponent<HexManager> ().tile = TileType.CLAY;
						hex.GetComponent<HexManager> ().sprite = ClaySprite;
						hex.GetComponent<HexManager> ().CD3sprite = ClayCD3;
						hex.GetComponent<HexManager> ().CD2sprite = ClayCD2;
						hex.GetComponent<HexManager> ().CD1sprite = ClayCD1;
						hex.GetComponent<SpriteRenderer> ().sprite = ClaySprite;
					} else if (rng <= 0.60) {
						hex.GetComponent<HexManager> ().tile = TileType.TREE;
						hex.GetComponent<HexManager> ().sprite = TreeSprite;
						hex.GetComponent<HexManager> ().CD3sprite = TreeCD3;
						hex.GetComponent<HexManager> ().CD2sprite = TreeCD2;
						hex.GetComponent<HexManager> ().CD1sprite = TreeCD1;
						hex.GetComponent<SpriteRenderer> ().sprite = TreeSprite;
						hex.GetComponent<SpriteRenderer> ().sortingLayerName = "Hex2";
						hex.GetComponent<SpriteRenderer> ().sortingOrder = sort;
					} else if (rng <= 0.80) {
						hex.GetComponent<HexManager> ().tile = TileType.SAND;
						hex.GetComponent<HexManager> ().sprite = SandSprite;
						hex.GetComponent<HexManager> ().CD3sprite = SandCD3;
						hex.GetComponent<HexManager> ().CD2sprite = SandCD2;
						hex.GetComponent<HexManager> ().CD1sprite = SandCD1;
						hex.GetComponent<SpriteRenderer> ().sprite = SandSprite;
					} else {
						hex.GetComponent<HexManager> ().tile = TileType.EMPTY;
						hex.GetComponent<HexManager> ().sprite = EmptySprite;
						hex.GetComponent<HexManager> ().CD3sprite = EmptyCD3;
						hex.GetComponent<HexManager> ().CD2sprite = EmptyCD2;
						hex.GetComponent<HexManager> ().CD1sprite = EmptyCD1;
						hex.GetComponent<SpriteRenderer> ().sprite = EmptySprite;
					}
					hex.transform.position = calcWorldCoord (gridPos);
					hex.transform.SetParent(hexGridGO.transform);
					HexManager hm = hex.GetComponent<HexManager> ();
					hm.x = x;
					hm.y = y;
					grid.Add (hex);
					if (Random.value * 1000 < 1 && !hm.isFactory ()) {
						sheep.transform.position = new Vector3 (hex.transform.position.x, hex.transform.position.y + 9, hex.transform.position.z);
						hm.sheepPresent = true;
						Instantiate (sheep);
					} else {
						hm.sheepPresent = false;
					}
					sort--;
				} else {
					grid.Add(null);
				}
			}
		}
	}

	//The grid should be generated on game start
	void Awake()
	{
		if (grid == null) {
			grid = new List<GameObject> ();
		}
		setSizes();
		createGrid();
		mouseHex = getHex (0, 0);
		graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster> ();
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		raycastResults = new List<RaycastResult> ();
	}

	void Update() {
		pointerEventData = new PointerEventData (eventSystem);
		pointerEventData.position = Input.mousePosition;
		raycastResults = new List<RaycastResult> ();
		graphicRaycaster.Raycast (pointerEventData, raycastResults);
	}

	public List<RaycastResult> getRaycastResults() {
		return raycastResults;
	}
}