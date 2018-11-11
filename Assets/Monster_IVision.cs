using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_IVision : MonoBehaviour {

    public Monster self;
    // Use this for initialization
    void Start()
    {
        self = GetComponentInParent<Monster>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (self.state != Monster.ENEMY_STATES.STUNNED && self.attack_rate >= Time.fixedTime - self.last_attack)
            {
                self.last_attack = Time.fixedTime;
                self.last_sight = self.player.transform.position;
                self.channeling_move = true;
                self.state = Monster.ENEMY_STATES.ATTACKING;
            }
            else
            {
                self.state = Monster.ENEMY_STATES.CHASING;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (self.state != Monster.ENEMY_STATES.STUNNED && self.attack_rate >= Time.fixedTime - self.last_attack)
            {
                self.last_attack = Time.fixedTime;
                self.last_sight = self.player.transform.position;
                self.channeling_move = true;
                self.state = Monster.ENEMY_STATES.ATTACKING;
            }
            else
            {
                self.state = Monster.ENEMY_STATES.CHASING;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (self.state != Monster.ENEMY_STATES.STUNNED && self.attack_rate >= Time.fixedTime - self.last_attack)
            {
                self.last_attack = Time.fixedTime;
                self.last_sight = self.player.transform.position;
                self.state = Monster.ENEMY_STATES.ATTACKING;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
