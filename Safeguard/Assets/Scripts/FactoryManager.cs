using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : HexManager {

	// Use this for initialization
	void Start () {
		this.GetComponent<HexManager> ().incrememntPollution ();
		this.GetComponent<HexManager> ().incrememntPollution ();
		this.GetComponent<HexManager> ().incrememntPollution ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.getHasBuilding()) {
			//pollution = 0;
		}
	}

}
