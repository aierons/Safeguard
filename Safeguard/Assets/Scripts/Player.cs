using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Text bankText; 
	public Button gatherButton;
	public int movement;

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

		gatherButton.onClick.AddListener (Gather);
	}
	
	// Update is called once per frame
	void Update () {
		bankText.text = "Action Count: " + actionCount.ToString() + "\nOre: " + ore.ToString() + "\nWood: " + wood.ToString();

		//if (Input.GetKeyDown (KeyCode.Mouse0)) {
			//if (Camera.main.ScreenToWorldPoint (Input.mousePosition) == transform) {
				//active = true;
			//}
		//}

		if (active && canMove) {
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				targetPos.z = transform.position.z;
			}
		}
		transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * 10);
	}

	void Gather() {
		if (actionCount >= 2) {
			ore += 1;
			wood += 1;

			actionCount--;
		}
	}
}