using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

	public GameObject tutorial01;
	public GameObject tutorial02;
	public GameObject tutorial03;
	public GameObject tutorial04;
	public GameObject tutorial05;
	public Camera main;
	public Button next;
	public Button prev;

	private int tutorialNumber; 

	public void Start() {
		tutorialNumber = 1;
	}

	public void nextScene() {
		if (tutorialNumber == 1) {
			tutorial01.gameObject.SetActive (false);
			tutorial02.gameObject.SetActive (true); 
			tutorialNumber++;
		} else if (tutorialNumber == 2) {
			tutorial02.gameObject.SetActive (false);
			tutorial03.gameObject.SetActive (true); 
			tutorialNumber++;
		} else if (tutorialNumber == 3) {
			tutorial03.gameObject.SetActive (false);
			tutorial04.gameObject.SetActive (true); 
			tutorialNumber++;
		} else if (tutorialNumber == 4) {
			tutorial04.gameObject.SetActive (false);
			tutorial05.gameObject.SetActive (true); 
			tutorialNumber++;
		} else {
			SceneManager.LoadScene (2);
		}
	}
	/*
	public void backScene() {
		if (tutorialNumber == 1) {
			SceneManager.LoadScene (0);
		} else if (tutorialNumber == 2) {
			tutorial02.gameObject.SetActive (false);
			tutorial01.gameObject.SetActive (true); 
			tutorialNumber--;
		} else if (tutorialNumber == 3) {
			tutorial03.gameObject.SetActive (false);
			tutorial02.gameObject.SetActive (true); 
			tutorialNumber--;
		} else if (tutorialNumber == 4) {
			tutorial04.gameObject.SetActive (false);
			tutorial03.gameObject.SetActive (true); 
			tutorialNumber--;
		} else {
			tutorial05.gameObject.SetActive (false);
			tutorial04.gameObject.SetActive (true); 
			tutorialNumber--;
		}
	}
	*/

	public void backScene() {
		if (tutorialNumber == 5) {
			tutorial05.gameObject.SetActive (false);
			tutorial04.gameObject.SetActive (true); 
			tutorialNumber--;
		} else if (tutorialNumber == 4) {
			tutorial04.gameObject.SetActive (false);
			tutorial03.gameObject.SetActive (true); 
			tutorialNumber--;
		} else if (tutorialNumber == 3) {
			tutorial03.gameObject.SetActive (false);
			tutorial02.gameObject.SetActive (true); 
			tutorialNumber--;
		} else if (tutorialNumber == 2) {
			tutorial02.gameObject.SetActive (false);
			tutorial01.gameObject.SetActive (true); 
			tutorialNumber--;
		} 
	}

	public void Update() {
		if (tutorialNumber != 1) {
			main.backgroundColor = new Color32 (230, 230, 230, 255);
			next.image.color = Color.black;
			prev.image.color = Color.black;
			prev.image.enabled = true;
		} else {
			main.backgroundColor = Color.black;
			next.image.color = Color.white;
			prev.image.enabled = false;
		}
	}
}
