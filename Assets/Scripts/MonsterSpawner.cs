using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

  [SerializeField]
  private GameObject monster;
	// Use this for initialization
	void Start () {
		StartCoroutine(Spawn());
	}
	
  IEnumerator Spawn(){
    while (true){
      yield return new WaitForSeconds(5);
      GameObject spawn = Instantiate(monster) as GameObject;
      spawn.transform.position = transform.position;
    }
  }
}
