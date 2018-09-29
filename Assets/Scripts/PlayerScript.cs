using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

  [SerializeField]
	private float movespeed;
  private Animator anim;
  private Rigidbody rb;
  private bool moving;
  private float xaxis, zaxis, lastx, lastz;
	void Start () {
		anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    xaxis = Input.GetAxisRaw("Horizontal");
    zaxis = Input.GetAxisRaw("Vertical");
    moving = (xaxis != 0 || zaxis != 0);
    anim.SetFloat("XAxis", xaxis);
    anim.SetFloat("ZAxis", zaxis);
    anim.SetBool("Moving", moving);

    if(moving == true){
      anim.SetFloat("LastX", xaxis);
      lastx = xaxis;
      anim.SetFloat("LastZ", zaxis);
      lastz = zaxis;
    }

    if(lastx == -1){
      GetComponent<SpriteRenderer>().flipX = true;
    } else {
      GetComponent<SpriteRenderer>().flipX = false;
    }

    rb.AddForce(movespeed * Time.deltaTime * zaxis * transform.forward, ForceMode.VelocityChange);
    rb.AddForce(movespeed * Time.deltaTime * xaxis * transform.right, ForceMode.VelocityChange);
	}
}
