using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

  private Transform player;
  [SerializeField]
  private float panSpeed;
  [SerializeField]
  private float height;
  [SerializeField]
  private float distance;
	void Start () {
    player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		var newPosition = new Vector3(player.position.x, player.position.y + height, player.position.z - distance);
    transform.position = Vector3.Lerp(transform.position, newPosition, panSpeed * Time.deltaTime);
	}
}
