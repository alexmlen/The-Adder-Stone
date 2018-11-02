using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour {

    //Field to enter scene to be loaded
    [SerializeField] private string loadLevel;

    private void OnTriggerEnter(Collider other)
    {
        //if the player collides...
        if (other.CompareTag("Player"))
        {
            //load the new scene
            SceneManager.LoadScene(loadLevel);
        }
    }
}
