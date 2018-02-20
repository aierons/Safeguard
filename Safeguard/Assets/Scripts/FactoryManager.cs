using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : HexManager {

	// Use this for initialization
	void Start () {
		this.GetComponent<HexManager> ().setPollution (3);
		this.GetComponent<HexManager> ().refresh ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.getHasBuilding()) {
			//pollution = 0;
		}
	}

}
