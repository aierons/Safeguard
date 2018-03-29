using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTextManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().sortingOrder = 11;
		GetComponent<Renderer> ().sortingLayerName = "Player";
		GetComponentInChildren<SpriteRenderer> ().sortingLayerName = "Player";
		GetComponentInChildren<SpriteRenderer> ().sortingOrder = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if (HexManager.textStatus == false) {
			Destroy (gameObject);
		}
	}
}
 