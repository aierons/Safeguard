using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTextManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().sortingOrder = 10;
		GetComponent<Renderer> ().sortingLayerID = SortingLayer.GetLayerValueFromName ("Default");
	}
	
	// Update is called once per frame
	void Update () {
		if (HexManager.textStatus == false) {
			Destroy (gameObject);
		}
	}
}
 