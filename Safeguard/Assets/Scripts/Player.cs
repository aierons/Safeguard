using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	public AudioClip moveSound;
	public AudioClip move2Sound;
	public AudioClip gatherSound;
	public AudioClip buildSound;
	public AudioClip build2Sound;
	public AudioClip cleanSound;
	public AudioClip clean2Sound;


	private TeamManager team;

	public Text msgText;

	//bars
	public Image actionLeft;
	public Image movementLeft;
	public Sprite bar0;
	public Sprite bar1;
	public Sprite bar2;
	public Sprite bar3;

	public float xoffset;
	public float yoffset;

	public int movement;
	public int buildingDiscount;
	public int gatherBonus;
	private int maxMovement;
	private bool active;
	private int actionCount;
	private Vector3 targetPos;

	private GameObject currentHex;
	private float minMove = Mathf.Infinity;

	// Use this for initialization
	void Start ()
	{
		team = GameObject.FindGameObjectWithTag ("Team").GetComponent<TeamManager> ();

		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		currentHex = gridManager.getHex (0, 0);
		transform.position = currentHex.transform.position;
		transform.position -= new Vector3 (0, 0, 1);
		transform.position += new Vector3 (xoffset, yoffset, 0);

		msgText.text = "";
		actionLeft.enabled = false;
		actionLeft.sprite = bar3;
		movementLeft.enabled = false;
		movementLeft.sprite = bar3; 

		actionCount = 3;
		targetPos = transform.position;
		active = false;

		maxMovement = movement;

	}
	
	// Update is called once per frame
	void Update ()
	{
		
		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		GameObject mouseHex = gridManager.getMouseHex ();
		if (active && movement > 0 && mouseHex != null && gridManager.getRaycastResults().Count == 0) {
			HexManager mhMan = mouseHex.GetComponent<HexManager> ();
			if (Input.GetKeyDown (KeyCode.Mouse0) && mhMan.checkLegalMove (currentHex, this, 1) && !mouseHex.Equals (currentHex)) {
				this.applyMinMove ();
				targetPos = mouseHex.transform.position;
				transform.position = targetPos;
				transform.position -= new Vector3 (0, 0, 1);
				transform.position += new Vector3 (xoffset, yoffset, 0);
				currentHex = mouseHex;
				SoundManager.instance.RandomizeSfx (moveSound, move2Sound);
			}
		}

		if (active) {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 150);
		}
	}

	public void setMinMove (int move)
	{
		if (move < minMove) {
			minMove = move;
		}
	}

	public void applyMinMove ()
	{
		movement -= (int)minMove;
		minMove = Mathf.Infinity;
	}

	public bool isActive ()
	{
		return active;
	}

	public void setActive (bool b)
	{
		active = b;
	}

	public void getActionCount ()
	{
		actionLeft.enabled = true;
		if (actionCount == 0) {
			actionLeft.sprite = bar0;
		} else if (actionCount == 1) {
			actionLeft.sprite = bar1;
		} else if (actionCount == 2) {
			actionLeft.sprite = bar2;
		} else if (actionCount == 3) {
			actionLeft.sprite = bar3;
		}
	}

	public void getMovementCount ()
	{
		movementLeft.enabled = true;
		if (movement == 0) {
			movementLeft.sprite = bar0;
		} else if (movement == 1) {
			movementLeft.sprite = bar1;
		} else if (movement == 2) {
			movementLeft.sprite = bar2;
		} else if (movement == 3) {
			movementLeft.sprite = bar3;
		}
	}

	public void Gather ()
	{
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if (currentHex.GetComponent<HexManager> ().GetGCoolDown () == 0
		    && currentHex.GetComponent<HexManager> ().getPollution () == 0
		    && !currentHex.GetComponent<HexManager> ().isFactory ()) {
			if (actionCount >= 2 && active) {
				if (currentHex.GetComponent<HexManager> ().tile == GridManager.TileType.ORE) {
					int o = Random.Range (2 + gatherBonus, 5 + gatherBonus);
					team.ore += o;
					actionCount -= 2;
					currentHex.GetComponent<HexManager> ().StartGCoolDown ();
					msgText.text = "You have gathered " + o.ToString () + " ore";
				} else if (currentHex.GetComponent<HexManager> ().tile == GridManager.TileType.TREE) {
					int w = Random.Range (2 + gatherBonus, 5 + gatherBonus);
					team.wood += w;
					actionCount -= 2;
					currentHex.GetComponent<HexManager> ().StartGCoolDown ();
					msgText.text = "You have gathered " + w.ToString () + " wood";
				} else if (currentHex.GetComponent<HexManager> ().tile == GridManager.TileType.CLAY) {
					int c = Random.Range (2 + gatherBonus, 5 + gatherBonus);
					team.clay += c;
					actionCount -= 2;
					currentHex.GetComponent<HexManager> ().StartGCoolDown ();
					msgText.text = "You have gathered " + c.ToString () + " clay";
				} else if (currentHex.GetComponent<HexManager> ().tile == GridManager.TileType.SAND) {
					int s = Random.Range (2 + gatherBonus, 5 + gatherBonus);
					team.sand += s;
					actionCount -= 2;
					currentHex.GetComponent<HexManager> ().StartGCoolDown ();
					msgText.text = "You have gathered " + s.ToString () + " sand";
				}
				team.gather_p += 50;
				team.gather_c++;
				SoundManager.instance.PlaySingle (gatherSound);

			} else if (actionCount <= 1 && active) {
				msgText.text = "Insufficient actions";
			}
		} else if (currentHex.GetComponent<HexManager> ().isFactory () && active) {
			msgText.text = "Cannot gather on factory tiles";
		} else if (currentHex.GetComponent<HexManager> ().getPollution () != 0 && active) {
			msgText.text = "Cannot gather on polluted tiles";
		} else if (currentHex.GetComponent<HexManager> ().GetGCoolDown () != 0 && active) {
			msgText.text = "Tile not ready to be gathered from again";
		}
	}

	public void Rush ()
	{
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if (currentHex.GetComponent<HexManager> ().GetGCoolDown () == 0
		    && currentHex.GetComponent<HexManager> ().getPollution () == 0
		    && !currentHex.GetComponent<HexManager> ().isFactory ()) {
			if (actionCount >= 1 && active) {
				int shell = Random.Range (2 + gatherBonus, 4 + gatherBonus);
				team.shell += shell;
				int leather = Random.Range (2 + gatherBonus, 4 + gatherBonus);
				team.leather += leather;

				actionCount -= 1;
				currentHex.GetComponent<HexManager> ().incrementPollution ();
				currentHex.GetComponent<HexManager> ().StartGCoolDown ();
				msgText.text = "You have Rush Gathered " + shell.ToString () + " shell and " + leather.ToString () + " leather. \n Pollution in this area has increased";
				team.rush_p -= 70;
				team.rush_c++;
				SoundManager.instance.PlaySingle (gatherSound);
			} else if (actionCount < 1 && active) {
				msgText.text = "Insufficient actions";
		}
		} else if (actionCount <= 1 && active) {
			msgText.text = "Insufficient actions";
		} else if (currentHex.GetComponent<HexManager> ().isFactory () && active) {
			msgText.text = "Cannot gather on factory tiles";
		} else if (currentHex.GetComponent<HexManager> ().getPollution () != 0 && active) {
			msgText.text = "Cannot gather on polluted tiles";
		} else if (currentHex.GetComponent<HexManager> ().GetGCoolDown () != 0 && active) {
			msgText.text = "Tile not ready to be gathered from again";
		}
	}

	public void BuildSingleWindmill ()
	{
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if ((currentHex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0 && currentHex.GetComponent<HexManager> ().getHasBuilding () == false
		    && currentHex.GetComponent<HexManager> ().getPollution () == 0) ||
		    !currentHex.GetComponent<HexManager> ().isFactory ()) {
			if (active && actionCount > 0) {
				if (currentHex.GetComponent<HexManager> ().isFactory ()) {
					return;
					//Windmill 3 ore 2 wood
				} else if ((team.ore >= 2 - buildingDiscount || team.shell >= 2 - buildingDiscount)
				           && (team.wood >= 1 - buildingDiscount || team.leather >= 1 - buildingDiscount)
				           && (team.clay >= 1 - buildingDiscount || team.shell >= 1 - buildingDiscount)) {

					msgText.text = "Built Windmill, used:\n ";
					if (team.shell >= 2 - buildingDiscount) {
						team.shell -= 2;
						msgText.text += "2 Shell, ";
					} else {
						team.ore -= 2;
						msgText.text += "2 Ore, ";
					}
					if (team.leather >= 1 - buildingDiscount) {
						team.leather -= 1;
						msgText.text += "1 Leather, ";
					} else {
						team.wood -= 1;
						msgText.text += "1 Wood, ";
					}
					if (team.shell >= 1 - buildingDiscount) {
						team.shell -= 1;
						msgText.text += "1 Shell, ";
					} else {
						team.clay -= 1;
						msgText.text += "1 Clay ";
					}

					//building = (GameObject)Instantiate (buildingSprite);
					//building.transform.position = this.transform.position;
					actionCount--;
					team.build_p += 100;
					team.build_c++;
					SoundManager.instance.RandomizeSfx (buildSound, build2Sound);

					currentHex.GetComponent<HexManager> ().MakeBuilding (HexManager.BuildingType.WIND);
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();

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
				}
			} else if (active) {
					msgText.text = "Insufficient actions";
			} 
		} else if (active && currentHex.GetComponent<HexManager> ().getHasBuilding () == true) {
			msgText.text = "Tile already has building";
		} else if (active && currentHex.GetComponent<HexManager> ().getPollution () != 0) {
			msgText.text = "Cannot build on a polluted tile";
		} else if (actionCount > 0 && active) {
			msgText.text = "Insufficient resources to build";
		}
	}

	public void BuildSingleSolarPanel ()
	{
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if ((currentHex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0 && currentHex.GetComponent<HexManager> ().getHasBuilding () == false
		    && currentHex.GetComponent<HexManager> ().getPollution () == 0) ||
		    currentHex.GetComponent<HexManager> ().isFactory ()) {
			if (active && actionCount > 0) {
				if (currentHex.GetComponent<HexManager> ().isFactory ()) {
					return;
					//Solar Panel 3 sand 2 clay
				} else if ((team.ore >= 1 - buildingDiscount || team.shell >= 1 - buildingDiscount)
				           && (team.sand >= 2 - buildingDiscount || team.leather >= 2 - buildingDiscount)
				           && (team.clay >= 1 - buildingDiscount || team.shell >= 1 - buildingDiscount)) {

					msgText.text = "Built Solar Panel, used:\n ";
					if (team.shell >= 1 - buildingDiscount) {
						team.shell -= 1;
						msgText.text += "1 Shell, ";
					} else {
						team.ore -= 1;
						msgText.text += "1 Ore, ";
					}
					if (team.leather >= 2 - buildingDiscount) {
						team.leather -= 2;
						msgText.text += "2 Leather, ";
					} else {
						team.sand -= 2;
						msgText.text += "2 Sand, ";
					}
					if (team.shell >= 1 - buildingDiscount) {
						team.shell -= 1;
						msgText.text += "1 Shell, ";
					} else {
						team.clay -= 1;
						msgText.text += "1 Clay ";
					}

					team.build_p += 100;
					team.build_c++;
					actionCount--;
					SoundManager.instance.RandomizeSfx (buildSound, build2Sound);
				
					currentHex.GetComponent<HexManager> ().MakeBuilding (HexManager.BuildingType.SOLAR);
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();

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
				}
			} else if (active) {
				msgText.text = "Insufficient actions";
			} 
		} else if (active && currentHex.GetComponent<HexManager> ().getHasBuilding () == true) {
			msgText.text = "Tile already has building";
		} else if (active && currentHex.GetComponent<HexManager> ().getPollution () != 0) {
			msgText.text = "Cannot build on a polluted tile";
		} else if (actionCount > 0 && active) {
			msgText.text = "Insufficient resources to build";
		}
	}

	public void BuildSingleRecyclingHouse ()
	{
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if ((currentHex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0 && currentHex.GetComponent<HexManager> ().getHasBuilding () == false
		    && currentHex.GetComponent<HexManager> ().getPollution () == 0) ||
		    currentHex.GetComponent<HexManager> ().isFactory ()) {
			if (active && actionCount > 0) {
				if (currentHex.GetComponent<HexManager> ().isFactory ()) {
					return;
				} else if ((team.sand >= 1 - buildingDiscount || team.leather >= 1 - buildingDiscount)
					&& (team.wood >= 2 - buildingDiscount || team.leather >= 2 - buildingDiscount)
					&& (team.clay >= 1 - buildingDiscount || team.shell >= 1 - buildingDiscount)) {

					msgText.text = "Built Compost Station, used:\n ";
					if (team.leather >= 1 - buildingDiscount) {
						team.leather -= 1;
						msgText.text += "1 Leather, ";
					} else {
						team.sand -= 1;
						msgText.text += "1 Sand, ";
					}
					if (team.leather >= 2 - buildingDiscount) {
						team.leather -= 2;
						msgText.text += "2 Leather, ";
					} else {
						team.wood -= 2;
						msgText.text += "2 Wood, ";
					}
					if (team.shell >= 1 - buildingDiscount) {
						team.shell -= 1;
						msgText.text += "1 Shell, ";
					} else {
						team.clay -= 1;
						msgText.text += "1 Clay ";
					}
						
					actionCount--;
					team.build_p += 100;
					team.build_c++;
					SoundManager.instance.RandomizeSfx (buildSound, build2Sound);
			
					currentHex.GetComponent<HexManager> ().MakeBuilding (HexManager.BuildingType.COMPOST);
					currentHex.GetComponent<HexManager> ().StartBuildingCoolDown ();
					currentHex.GetComponent<HexManager> ().SwitchHasBuilding ();

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
				}
			} else if (active) {
				msgText.text = "Insufficient actions";
			} 
		} else if (active && currentHex.GetComponent<HexManager> ().getHasBuilding () == true) {
			msgText.text = "Tile already has building";
		} else if (active && currentHex.GetComponent<HexManager> ().getPollution () != 0) {
			msgText.text = "Cannot build on a polluted tile";
		} else if (actionCount > 0 && active) {
			msgText.text = "Insufficient resources to build";
		}
	}

	public void BuildReplacement ()
	{
		GameObject gaman = GameObject.Find ("GameManager");
		GridManager gridManager = gaman.GetComponent<GridManager> ();

		if ((currentHex.GetComponent<HexManager> ().GetBuildingCoolDown () == 0 && currentHex.GetComponent<HexManager> ().getHasBuilding () == false
		    && currentHex.GetComponent<HexManager> ().getPollution () == 0) &&
		    currentHex.GetComponent<HexManager> ().isFactory ()) {
			if (active && actionCount > 0) {
				if (team.ore >= 3 - buildingDiscount && team.wood >= 3 - buildingDiscount && team.clay >= 3 - buildingDiscount && team.sand >= 3 - buildingDiscount
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
						msgText.text += "Cannot build replacement while there are adjacent polluted tiles";
						return;
					}
					if (n2 != null && n2.GetComponent<HexManager> ().getPollution () != 0) {
						msgText.text += "Cannot build replacement while there are adjacent polluted tiles";
						return;
					}
					if (n3 != null && n3.GetComponent<HexManager> ().getPollution () != 0) {
						msgText.text += "Cannot build replacement while there are adjacent polluted tiles";
						return;
					}
					if (n4 != null && n4.GetComponent<HexManager> ().getPollution () != 0) {
						msgText.text += "Cannot build replacement while there are adjacent polluted tiles";
						return;
					}
					if (n5 != null && n5.GetComponent<HexManager> ().getPollution () != 0) {
						msgText.text += "Cannot build replacement while there are adjacent polluted tiles";
						return;
					}
					if (n6 != null && n6.GetComponent<HexManager> ().getPollution () != 0) {
						msgText.text += "Cannot build replacement while there are adjacent polluted tiles";
						return;
					}
					if (team.replacedFactories == 0) {
						msgText.text += "Built Windmill Field";
						currentHex.GetComponent<SpriteRenderer> ().sprite = currentHex.GetComponent<HexManager> ().CD1sprite;
					} else if (team.replacedFactories == 1) {
						msgText.text += "Built Solar Panel Field";
						currentHex.GetComponent<SpriteRenderer> ().sprite = currentHex.GetComponent<HexManager> ().CD2sprite;
					} else if (team.replacedFactories == 2) {
						msgText.text += "Built Green Conservatory";
						currentHex.GetComponent<SpriteRenderer> ().sprite = currentHex.GetComponent<HexManager> ().CD3sprite;
					} else {
						msgText.text += "Built Recycling Center";
						currentHex.GetComponent<SpriteRenderer> ().sprite = currentHex.GetComponent<HexManager> ().sprite;
					}
					currentHex.GetComponent<HexManager> ().decrementPollution ();
					team.ore -= 3;
					team.wood -= 3;
					team.clay -= 3;
					team.sand -= 3;
					actionCount--;
					team.replacedFactories++;
					team.build_p += 400;
					team.build_c++;
					SoundManager.instance.RandomizeSfx (buildSound, build2Sound);
				} else if (currentHex.GetComponent<HexManager> ().isFactory () && actionCount > 0 && active) {
					msgText.text = "Insufficient resources to replace factory";
					return;
				}
			} else if (currentHex.GetComponent<HexManager> ().isFactory () && actionCount < 1 && active) {
					msgText.text = "Insufficient actions";
					return;
			}
		} 
	}

	public void Clean ()
	{
		if (active && currentHex.GetComponent<HexManager> ().getPollution () != 0
		    && !currentHex.GetComponent<HexManager> ().isFactory () && actionCount > 0) {
			currentHex.GetComponent<HexManager> ().decrementPollution ();
			if (currentHex.GetComponent<HexManager> ().getPollution () == 0) {
				GameObject t = GameObject.Find ("Team");
				TeamManager tm = t.GetComponent<TeamManager> ();
				--tm.pollutedHexes;
			}
			team.clean_p += 100;
			team.clean_c++;
			--actionCount;
			SoundManager.instance.RandomizeSfx (cleanSound, clean2Sound);

		} else if (currentHex.GetComponent<HexManager> ().isFactory () && active) {
			msgText.text = "Cannot clean factory tiles";
			return;
		} else if (currentHex.GetComponent<HexManager> ().getPollution () == 0 && active) {
			msgText.text = "Current tile is not polluted";
			return;
		} else if (actionCount == 0 && active) {
			msgText.text = "Insufficient actions";
			return;
		}
			
	}

	void OnMouseDown ()
	{
		active = true;
		for (int i = 0; i < 3; i++) {
			Transform Go = GameObject.FindGameObjectWithTag ("Team").transform.GetChild (i);
			if (Go.tag != this.tag) {
				Go.GetComponent<Player> ().setActive (false);
			}
		}
	}

	public void EndTurn ()
	{
		//active = false;
		actionCount = 3;
		movement = maxMovement;
		msgText.text = "";
	}
}