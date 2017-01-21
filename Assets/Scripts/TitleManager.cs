﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

  public Camera camera;

  public Transform titlePosition;
  public Transform creditsPosition;
  public Transform scorePosition;
  public Transform transitionPosition;
  public Transform gamePosition;

  public GameObject titleScreen;
  public GameObject creditsScreen;
  public GameObject scoreScreen;

  GameObject currentScreen;

  private void Start() {
    titleScreen.SetActive(true);
    creditsScreen.SetActive(false);
    scoreScreen.SetActive(false);
    camera.transform.position = titlePosition.position;
    currentScreen = titleScreen;
  }

  public void StartGame() {
    StartCoroutine(GoToScreen(gamePosition.position, null, true));
  }

  IEnumerator loadSceneAsync(string scene) {
    AsyncOperation async = SceneManager.LoadSceneAsync(scene);
    while (!async.isDone) {
      yield return null;
    }
  }


  public void ExitGame() {
    Application.Quit();
  }

  public void ShowCredits() {
    StartCoroutine(GoToScreen(creditsPosition.position, creditsScreen));
  }

  public void ShowTitleScreen() {
    StartCoroutine(GoToScreen(titlePosition.position, titleScreen));
  }

  IEnumerator GoToScreen(Vector3 to, GameObject showMenu, bool startGame = false) {
    currentScreen.SetActive(false);
    Vector3 from = camera.transform.position;
    float i = 0;
    while (i < 1) {
      i += Time.deltaTime;
      Vector3 p = GameManager.QuadInterp(from, transitionPosition.position, to, i);
      camera.transform.position = p;
      yield return new WaitForEndOfFrame();
    }
    currentScreen = showMenu;
    if (!startGame) {
      currentScreen.SetActive(true);
    }else {
      yield return loadSceneAsync("GameScene");
    }
  }

  public void ShowScore() {
    StartCoroutine(GoToScreen(scorePosition.position, scoreScreen));
  }
}