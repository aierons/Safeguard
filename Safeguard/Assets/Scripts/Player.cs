using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Text bankText; 
	public Text actText;
	public int movement;

	public Button gatherButton;
	public Button moveButton;
	public Button endTurnButton;

	private bool canMove;
	private bool active;
	private int ore; 
	private int wood;
	private int actionCount;
	private Vector3 targetPos;

	private GameObject currentHex;

	// Use this for initialization
	void Start () {
		ore = 0;
		wood = 0;
		bankText.text = "";
		actionCount = 3;
		targetPos = transform.position;
		canMove = false;
		active = false;

		gatherButton.onClick.AddListener (Gather);
		moveButton.onClick.AddListener (Move);
		endTurnButton.onClick.AddListener (EndTurn);
	}
	
	// Update is called once per frame
	void Update () {
		bankText.text = "Action Count: " + actionCount.ToString() + "\nMovement Left:" + movement.ToString() + "\nOre: " + ore.ToString() + "\nWood: " + wood.ToString();

		if (active && movement > 0 && canMove) {
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				targetPos.z = transform.position.z;
				transform.position = targetPos;
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
		if (actionCount >= 2 && active) {
			ore += Random.Range(0, 3);
			wood += Random.Range(0,3);

			actionCount -= 2;
		}
	}

	void Move() {
		if (active) {
			canMove = true;
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
	}

}