using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager: MonoBehaviour
{
	//following public variable is used to store the hex model prefab;
	//instantiate it by dragging the prefab on this variable using unity editor
	public GameObject Hex;
	public GameObject Factory;
	//next two variables can also be instantiated using unity editor
	public int gridSideLength;

	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;
	private GameObject mouseHex;

	private GameObject hexGridGO;

	static IList<GameObject> grid;

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

		for (int y = 0; y < (gridSideLength * 2) - 1; y++)
		{
			for (int x = 0; x < (gridSideLength * 2) - 1; x++)
			{
				if(x + y == gridSideLength - 1 && factory1) {
					GameObject factory = (GameObject)Instantiate (Factory);
					Vector2 gridPos = new Vector2 (x, y);
					factory.transform.position = calcWorldCoord (gridPos);
					factory.transform.parent = hexGridGO.transform;
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
					factory.transform.parent = hexGridGO.transform;
					FactoryManager fm = factory.GetComponent<FactoryManager> ();
					fm.x = x;
					fm.y = y;
					grid.Add (factory);
					factory2 = false;
					continue;
				}
				if (x + y >= gridSideLength - 1 && x + y <= 3 * (gridSideLength - 1)) {
					//GameObject assigned to Hex public variable is cloned
					GameObject hex = (GameObject)Instantiate (Hex);
					//Current position in grid
					Vector2 gridPos = new Vector2 (x, y);
					hex.transform.position = calcWorldCoord (gridPos);
					hex.transform.parent = hexGridGO.transform;
					HexManager hm = hex.GetComponent<HexManager> ();
					hm.x = x;
					hm.y = y;
					grid.Add (hex);
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
	}
}