using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_OVision : MonoBehaviour {
    public Monster self;
	// Use this for initialization
	void Start () {
        self = GetComponentInParent<Monster>();
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            
            if(self.state != Monster.ENEMY_STATES.STUNNED || self.state != Monster.ENEMY_STATES.ATTACKING) self.state = Monster.ENEMY_STATES.CHASING;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (self.state != Monster.ENEMY_STATES.STUNNED) self.state = Monster.ENEMY_STATES.IDLE;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
