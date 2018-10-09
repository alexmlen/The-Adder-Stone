using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PlayerScript : MonoBehaviour {

  [SerializeField]
	private float movespeed;
  [SerializeField]
  private GameObject nearestInteractable, cursor;
  [SerializeField]
  private float range, cursorHeight;
  [SerializeField] 
  private bool interacting, controlling, inMenu;
  [SerializeField] 
  public Flowchart mainFlowchart;
  private Animator anim;
  private Rigidbody rb;
  private bool moving;
  private float xaxis, zaxis, lastx, lastz;
	void Start () {
		anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
    SearchInteractables();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    if(inMenu){
      return;
    }
    if (controlling){
      Move();
    }
    if(!interacting && Input.GetButtonDown("Fire1") && nearestInteractable != null){
      Interact();
    }
	}

  void Move(){
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
        SearchInteractables();
      }

      if(lastx == -1){
        GetComponent<SpriteRenderer>().flipX = true;
      } else {
        GetComponent<SpriteRenderer>().flipX = false;
      }

      rb.AddForce(movespeed * Time.deltaTime * zaxis * transform.forward, ForceMode.VelocityChange);
      rb.AddForce(movespeed * Time.deltaTime * xaxis * transform.right, ForceMode.VelocityChange);
  }

  void Interact(){
    Flowchart flowchart = nearestInteractable.GetComponent<Flowchart>();
    if(flowchart != null){
      interacting = true;
      controlling = false;
      GetComponent<Rigidbody>().velocity = Vector3.zero;
      cursor.SetActive(false);
      flowchart.ExecuteBlock("Start");
    }
  }

  void EndInteract(){
    interacting = false;
    controlling = true;
    nearestInteractable = null;
    SearchInteractables();
  }

  void SearchInteractables(){
    var collidersInRange = Physics.OverlapSphere(transform.position, range);
    if(interacting || collidersInRange.Length == 0){
      cursor.SetActive(false);
      return;
    }
    float distance = range;
    Collider closestCollider = null;
    Vector3 position = transform.position;
    foreach (Collider currentCollider in collidersInRange){
      if (currentCollider.tag == "Interactable"){
        Vector3 diff = currentCollider.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance){
          closestCollider = currentCollider;
          distance = curDistance;
        }
      }
    }
    if(closestCollider == null){
      cursor.SetActive(false);
      nearestInteractable = null;
      return;
    }
    if(closestCollider.gameObject != nearestInteractable){
      nearestInteractable = closestCollider.gameObject;
      cursor.transform.position = new Vector3(nearestInteractable.transform.position.x, nearestInteractable.transform.position.y + cursorHeight, nearestInteractable.transform.position.z);
      cursor.SetActive(true);
    }
  }
}
