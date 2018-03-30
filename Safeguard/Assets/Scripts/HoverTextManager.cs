using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTextManager : MonoBehaviour {
	int timeAlive;
	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().sortingOrder = 11;
		GetComponent<Renderer> ().sortingLayerName = "Player";
		GetComponentInChildren<SpriteRenderer> ().sortingLayerName = "Player";
		GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		GetComponentInParent<TextMesh> ().color = new Color (1, 1, 1, 0);
		GetComponentInChildren<SpriteRenderer> ().sortingOrder = 10;
		timeAlive = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeAlive > 20) {
			GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			GetComponentInParent<TextMesh> ().color = new Color (0, 0, 0, 1);
		}
		if (HexManager.textStatus == false) {
			Destroy (gameObject);
		}
		timeAlive++;
	}
}
 