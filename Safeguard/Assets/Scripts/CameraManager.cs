using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	private float maxSize;
	private float maxX;
	private float maxY;
	private float minX;
	private float minY;

	void Start () {
		GameObject gm = GameObject.Find ("Main Camera");
		Camera c = gm.GetComponent<Camera> ();
		GameObject dm = GameObject.Find ("GameManager");
		int length = dm.GetComponent<GridManager> ().gridSideLength;
		c.transform.position = new Vector3 (dm.GetComponent<GridManager> ().getGrid () [((length * 2) - 1) * (length - 1) + length - 1].GetComponent<Transform> ().position.x, 
			dm.GetComponent<GridManager> ().getGrid () [((length * 2) - 1) * (length - 1) + length].GetComponent<Transform> ().position.y, -10);
		GameObject turtle = GameObject.Find ("turtlebackground 2_00000");
		turtle.transform.position = new Vector3 (c.transform.position.x + 2, c.transform.position.y - 2, 0);
		turtle.transform.localScale *= 14;

		maxSize = 120 + (length - 5) * 20;
		c.orthographicSize = maxSize - 40;
		maxX = c.transform.position.x + 40;
		maxY = c.transform.position.y + 40;
		minX = maxX - 80;
		minY = maxY - 80;
	}

	void Update() {
		float tmpScrollDelta = Input.mouseScrollDelta.y;
		Camera camera = this.GetComponent<Camera> ();
		if (tmpScrollDelta > 0) {
			if (camera.orthographicSize < maxSize) {
				camera.orthographicSize += 1;
				maxX -= 1;
				minX += 1;
				maxY -= 1;
				minY += 1;
			}
		} else if (tmpScrollDelta < 0) {
			if (camera.orthographicSize > 20) {
				camera.orthographicSize -= 1;
				maxX += 1;
				minX -= 1;
				maxY += 1;
				minY -= 1;
			}
		}

		Vector3 camPos = camera.transform.position;
		if (camPos.x > maxX) {
			camera.transform.position = new Vector3 (maxX, camPos.y, camPos.z);
			camPos = camera.transform.position;
		} else if (camPos.x < minX) {
			camera.transform.position = new Vector3 (minX, camPos.y, camPos.z);
			camPos = camera.transform.position;
		}
		if (camPos.y > maxY) {
			camera.transform.position = new Vector3 (camPos.x, maxY, camPos.z);
			camPos = camera.transform.position;
		} else if (camPos.y < minY) {
			camera.transform.position = new Vector3 (camPos.x, minY, camPos.z);
			camPos = camera.transform.position;
		}

		GameObject gm = GameObject.Find ("GameManager");
		GridManager gridManager = gm.GetComponent<GridManager> ();
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		if(mouseX < Screen.width && mouseX > 0 && mouseY < Screen.height && mouseY > 0) {
			if (mouseX > Screen.width * 0.75 && camPos.x < maxX) {
				camera.transform.position = new Vector3 (camPos.x + 1.5f*(mouseX - Screen.width*0.75f)/(Screen.width*0.25f), camPos.y, camPos.z);
				camPos = camera.transform.position;
			} else if (mouseX < Screen.width * 0.25 && camPos.x > minX) {
				camera.transform.position = new Vector3 (camPos.x - (1.5f - 1.5f*mouseX/(Screen.width*0.25f)), camPos.y, camPos.z);
				camPos = camera.transform.position;
			}
			if (mouseY > Screen.height * 0.75 && camPos.y < maxY) {
				camera.transform.position = new Vector3 (camPos.x, camPos.y + 1.5f*(mouseY - Screen.height*0.75f)/(Screen.height*0.25f), camPos.z);
				camPos = camera.transform.position;
			} else if (mouseY < Screen.height * 0.25 && camPos.y > minY) {
				camera.transform.position = new Vector3 (camPos.x, camPos.y - (1.5f - 1.5f*mouseY/(Screen.height*0.25f)), camPos.z);
				camPos = camera.transform.position;
			}
		}
	}



}
