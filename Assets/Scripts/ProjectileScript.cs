using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void Awake () {
    Destroy (gameObject, 2);
  }

  private void OnCollisionEnter(Collision collision){
    if(string.Equals(gameObject.tag, "projectile")){
      GetComponent<Rigidbody>().velocity = Vector3.zero;
      GetComponent<Rigidbody>().isKinematic = true;
      GetComponent<BoxCollider>().isTrigger = true;
      transform.SetParent(collision.transform);// = collision.transform.parent.gameObject.transform;
    }
    //var joint = gameObject.AddComponent<FixedJoint>();
    //joint.connectedBody = collision.rigidbody;
    //BoxCollider collider = gameObject.GetComponent<BoxCollider>();
    //collider.enabled = false;
  }
}
