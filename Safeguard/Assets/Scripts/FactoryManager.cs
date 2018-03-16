using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : HexManager {

	// Use this for initialization
	void Start () {
		this.GetComponent<HexManager> ().setPollution (3);
		this.GetComponent<HexManager> ().refresh ();
		this.factory = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void decrementPollution() {
		this.pollution = 0;
	}

}
