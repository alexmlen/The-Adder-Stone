﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 public class ChangeScene : MonoBehaviour {
     [SerializeField] private string level;
     public void SwitchScene()
    {
      SceneManager.LoadScene(level);
    }
}