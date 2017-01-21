﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
  Collider collider;

  public float breakRadius;
  public float speed;

  public int pointsWorth = 10;

  float interp = 0;
  Vector3 strikePosition;
  Vector3 targetPosition;

  bool deaccelerate = false;

  void Start() {
    collider = GetComponent<Collider>();
  }

  float outInCubic(float t) {
    if (t < (0.5f)) {
      return 0.5f * (Mathf.Pow(t * 2 - 1, 3) + 1);
    }
    return 0.5f * Mathf.Pow(t * 2, 3) + 1 / 2;
    //return t * t * ((s + 1) * t - s);
  }

  virtual public void Handle() {
    if (deaccelerate) {
      transform.position = Vector3.Lerp(strikePosition, targetPosition, outInCubic(interp));
      interp += 0.2f * Time.deltaTime;
    } else {
      if (Vector3.Distance(transform.position, GameManager.Instance().shieldRenderer.gameObject.transform.position) < breakRadius) {
        deaccelerate = true;
        strikePosition = transform.position;
        targetPosition = GameManager.Instance().shieldRenderer.gameObject.transform.position;
      } else {
        transform.position += speed * transform.forward * Time.deltaTime;
      }
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.tag == "Planet") {
      GameManager.Instance().Damage(1);
      GameManager.Instance().enemyList.Remove(this);
      Destroy(gameObject);
    } else if (other.tag == "Player Bullets") {
      if (Mathf.Abs(transform.position.y) < Screen.height / 2 && Mathf.Abs(transform.position.x) < Screen.width / 2) {
        GameManager.Instance().enemyList.Remove(this);
        Destroy(gameObject);
        GameManager.Instance().AwardPoints(pointsWorth);
      }
    }
  }
}