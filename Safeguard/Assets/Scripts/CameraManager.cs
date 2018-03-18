using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	void Start () {
		GameObject gm = GameObject.Find ("Main Camera");
		Camera c = gm.GetComponent<Camera> ();
		GameObject dm = GameObject.Find ("GameManager");
		int length = dm.GetComponent<GridManager> ().gridSideLength;
		c.transform.position = new Vector3 (dm.GetComponent<GridManager> ().getGrid () [((length * 2) - 1) * (length - 1) + length - 1].GetComponent<Transform> ().position.x, 
			dm.GetComponent<GridManager> ().getGrid () [((length * 2) - 1) * (length - 1) + length].GetComponent<Transform> ().position.y, -10);
		c.orthographicSize = 16 + (length - 4) * 5;
	}

}
