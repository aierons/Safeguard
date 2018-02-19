using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Text bankText; 
	public Text actText;
	public Button gatherButton;
<<<<<<< HEAD
	public Button buildButton;
=======
	public Button moveButton;
>>>>>>> 1222d8efa3c46e0d4f4aea8409a6d9794547250f
	public int movement;
	public GameObject buildingSprite;

	private bool canMove;
	private bool active;
	private int ore; 
	private int wood;
	private int actionCount;
	private Vector3 targetPos;
	private Vector3 buildingPos;

	private GameObject currentHex;


	// Use this for initialization
	void Start () {
		ore = 0;
		wood = 0;
		bankText.text = "";
		actionCount = 10;
		targetPos = transform.position;
<<<<<<< HEAD
		buildingPos = buildingSprite.GetComponent<Transform> ().position;

		gatherButton.onClick.AddListener (Gather);
		buildButton.onClick.AddListener (Build);
=======
		canMove = false;
		active = false;

		gatherButton.onClick.AddListener (Gather);
		moveButton.onClick.AddListener (Move);
>>>>>>> 1222d8efa3c46e0d4f4aea8409a6d9794547250f
	}
	
	// Update is called once per frame
	void Update () {
		bankText.text = "Action Count: " + actionCount.ToString() + "\nMovement Left:" + movement.ToString() + "\nOre: " + ore.ToString() + "\nWood: " + wood.ToString();

		if (active && movement > 0 && canMove) {
			print ("t");
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
		}
	}

	void Gather() {
		if (actionCount >= 2 && active) {
			ore += 1;
			wood += 1;

			actionCount--;
		}
	}

<<<<<<< HEAD
	void Build() {
		if ((ore >= 3 && wood >=2) || (ore >= 2 && wood >= 3)) {
			buildingPos = this.transform.position;
			print (buildingPos);
		}	
	}
=======
	void Move() {
		if (active) {
			canMove = true;
		}
	}

	void OnMouseDown() {
		active = true;
		actText.text = "Active Character: Jayson\nActions: " + actionCount.ToString();
	}

>>>>>>> 1222d8efa3c46e0d4f4aea8409a6d9794547250f
}