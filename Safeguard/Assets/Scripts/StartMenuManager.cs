﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {

	public void PlayGame() {
		SceneManager.LoadScene (1);
		//SceneManager.SetActiveScene (SceneManager.GetSceneByName ("MainScene"));
	}
}
