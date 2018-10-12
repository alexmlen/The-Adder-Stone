using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PlayerScript : MonoBehaviour {

  [SerializeField]
	private float movespeed;
  [SerializeField]
  private GameObject nearestInteractable, cursor, cube;
  [SerializeField]
  private float range, cursorHeight, bowPower;
  [SerializeField] 
  private bool interacting, controlling, inMenu;
  [SerializeField] 
  public Flowchart mainFlowchart;
  private Animator anim;
  private Rigidbody rb;
  private bool moving;
  private float xaxis, zaxis, lastx, lastz;
  private Camera mainCamera;


	void Start () {
		anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
    mainCamera = FindObjectOfType<Camera>();
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
    if(!interacting && Input.GetButtonDown("Fire3") && nearestInteractable != null){
      Interact();
    }
    if(Input.GetButtonDown("Fire1") && Input.GetButton("Fire2") && controlling){
      //On Mouse Left Button Press & Mouse Right is held down
      Shoot();
    }
	}

  void Shoot(){
    GameObject projectile = Instantiate(cube) as GameObject;
    projectile.transform.position = new Vector3(rb.transform.position.x, 1, rb.transform.position.z);

    //projectile = projectile.transform.GetChild(0).gameObject;

    Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    float enter = 0.0f;
    if(groundPlane.Raycast(cameraRay, out enter)){
      Vector3 hitPoint = cameraRay.GetPoint(enter);
      Vector3 direction = hitPoint - transform.position;

      //Some math to make magnitude of velocity vector uniform.
      float sqrLen = direction.sqrMagnitude;
      float distanceConstant = Mathf.Sqrt(Mathf.Pow(bowPower, 2)/sqrLen);

      Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
      Rigidbody rib = projectile.GetComponent<Rigidbody>();
      rib.velocity = new Vector3(direction.x*distanceConstant, transform.position.y + 5, direction.z*distanceConstant);
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
