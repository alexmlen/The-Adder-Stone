using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour {
  private IEnumerator coroutine;
  private SkinnedMeshRenderer mr;

	void Start () {
    mr = gameObject.GetComponent<SkinnedMeshRenderer>();
		coroutine = Blink();
    StartCoroutine(coroutine);
	}

  private IEnumerator Blink(){
    while (true){
      yield return new WaitForSeconds(Random.value*10 + 3);
      mr.enabled = true;
      yield return new WaitForSeconds(0.1f);
      mr.enabled = false;
    }
  }
	
  private void Update(){

  }
}
